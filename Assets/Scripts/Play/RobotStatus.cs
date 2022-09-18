using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// ロボットの状態を管理するクラス。アニメーションなどを処理したりする。
// 操作キャラ・リプレイ・シャドウの基盤。
[RequireComponent(typeof(PurgeManager))]
public class RobotStatus : MonoBehaviour
{
    // ロボットの状態を示す列挙型
    private enum E_RobotStatus
    {
        Ready,      // ゲーム開始前の待機状態
        Fly,        // 飛行状態（通常の待機状態）
        UseParts,   // アイテム使用中
        Cooldown,   // クールタイム待機中
        EndFly,     // 飛行終了（ゲームクリアやゲームオーバーなど）
    }
    private E_RobotStatus _status = E_RobotStatus.Ready;
    private int cooltime = 0;       // クールタイム

    private Sprite usingPartsSprite = null; // 生成している使用中パージスプライトの参照

    // ロボットの状態判定メソッド
    public bool IsWaitingForFly => _status == E_RobotStatus.Ready;  // 飛行未開始判定
    public bool IsPartsUsable => _status == E_RobotStatus.Fly;  // 装備パーツの使用可能判定
    public bool IsUsingParts => _status == E_RobotStatus.UseParts;  // パーツの使用中判定
    public bool IsFlying => _status != E_RobotStatus.Ready && _status != E_RobotStatus.EndFly;  // 飛行中判定（ゲーム中判定）
    public bool IsEndFly => _status == E_RobotStatus.EndFly;    // 飛行終了判定


    // キャッシュ等
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _usePartsAudioSource;
    [SerializeField] private AudioSource _purgePartsAudioSource;
    private AudioClip _purgePartsSE;
    private PurgeManager _purgeManager;

    [SerializeField] private ParticleSystem _usePartsEffect = null;

    // ゲームオーバー時のパージロボットスプライトデータ
    [SerializeField] private List<Sprite> GameOverRobotPurgeData = new List<Sprite>();

    [Header("装備パーツ情報")]
    [SerializeField] private SpriteRenderer _partsPrefab;
    private SpriteRenderer _partsObject;
    [SerializeField] private Vector2 _propellerLocate = Vector2.zero;   // プロペラの出現位置
    [SerializeField] private Vector2 _rocketLocate = Vector2.zero;      // ロケットの出現位置
    [SerializeField] private Vector2 _gliderLocate = Vector2.zero;      // グライダーの出現位置

    [SerializeField] private float _bombExplodeDistance = 0f;   // 爆発の出現距離
    [SerializeField] private ParticleSystem _explodesPrefab;    // 爆発エフェクトのPrefab

    // パーツの使用終了とともに破棄するオブジェクトのリスト
    private List<GameObject> _destroyWithPartsObjects = new List<GameObject>();

    [Header("イベント系統")]
    [Tooltip("パーツの使用開始時に呼ばれるメソッド")]
    [SerializeField] private UnityEvent startUsePartsEvent = new UnityEvent();
    [Tooltip("パーツの使用終了時に呼ばれるメソッド")]
    [SerializeField] private UnityEvent endUsePartsEvent = new UnityEvent();

    private void Awake()
    {
        _purgeManager = GetComponent<PurgeManager>();
    }

    private void FixedUpdate()
    {
        // クールタイム消費（実装方法はコルーチンのWaitForSecondsとか使うべきか迷ってる）
        if (_status == E_RobotStatus.Cooldown && cooltime > 0)
        {
            cooltime--;
            if (cooltime <= 0)
            {
                endCooltime();
            }
        }
    }

    // ゲーム開始時に呼ぶメソッド
    public void startGame()
    {
        if(_status != E_RobotStatus.Ready)
        {
            Debug.LogWarning("ゲーム開始前以外にゲーム開始メソッドが呼ばれました。");
            return;
        }

        _status = E_RobotStatus.Fly;

        // 飛行のアニメーションへ遷移（OnGroundで勝手に遷移する）
    }

    // パーツの使用開始
    public void startUseParts(PartsPerformance performance, PartsInfo.PartsData data)
    {
        // アイテムが使用できる状態か判定
        if (!IsPartsUsable)
        {
            Debug.LogWarning("パーツの使用状態に移れませんでした。");
            return;
        }

        startUsePartsEvent.Invoke();

        // アイテム使用状態へ状態遷移
        _status = E_RobotStatus.UseParts;

        // パージする際に投げ出すパーツの見た目
        usingPartsSprite = performance.partsSprite;

        // クールタイムを計算しておく
        cooltime = Mathf.RoundToInt(performance.cooltime / Time.fixedDeltaTime);

        // アイテムの種類によって特有のアニメーションへ遷移
        BuildUsePartsObject(performance, data);
        if (_usePartsEffect)
        {
            _usePartsEffect.gameObject.SetActive(true);
            _usePartsEffect.Play();
        }
        if (performance.forceType == PartsPerformance.E_ForceType.Rocket)
        {
            _animator.SetTrigger("Rocket");
        }
        else if (performance.forceType == PartsPerformance.E_ForceType.Propeller)
        {
            _animator.SetTrigger("Propeller");
        }
        else if (performance.forceType == PartsPerformance.E_ForceType.Glider)
        {
            _animator.SetTrigger("Glider");
        }

        // SEを鳴らす
        if (performance.usePartsSE != null && _usePartsAudioSource != null && performance.forceType != PartsPerformance.E_ForceType.Bomb)
        {
            _usePartsAudioSource.clip = performance.usePartsSE;
            _usePartsAudioSource.Play();
        }
        if (_purgePartsAudioSource != null && performance.purgePartsSE != null)
        {
            _purgePartsSE = performance.purgePartsSE;
        }
    }

    // パーツの効果終了
    public void endUseParts()
    {
        if (!IsUsingParts)
        {
            Debug.LogWarning("パーツの使用時以外にパーツ使用終了メソッドが呼び出されました。");
            return;
        }

        // 状態遷移（クールタイムがあれば待機状態へ移行する）
        if (cooltime > 0)
        {
            _status = E_RobotStatus.Cooldown;
            _animator.SetBool("Cooltime", true);
        }
        else _status = E_RobotStatus.Fly;

        // 使用し終わったパーツをパージして投げ出す
        if (usingPartsSprite != null)
        {
            _purgeManager.AddPartsBySprite(usingPartsSprite);
            usingPartsSprite = null;
        }

        endUsePartsEvent.Invoke();
        // 飛行orクールタイムのアニメーションに遷移する
        if (_partsObject) Destroy(_partsObject.gameObject);
        _animator.SetTrigger("EndUse");

        // SEの処理
        if (_purgePartsAudioSource != null && _purgePartsSE != null)
        {
            _purgePartsAudioSource.PlayOneShot(_purgePartsSE);
            _purgePartsSE = null;
        }
        if (_usePartsAudioSource) _usePartsAudioSource.Stop();

        DestroyAllObjectsWithParts();
    }

    // クールタイムの終了
    public void endCooltime()
    {
        if (!IsFlying)
        {
            Debug.LogWarning("飛行中以外にクールダウン終了メソッドが呼ばれました。");
            return;
        }

        _status = E_RobotStatus.Fly;
        cooltime = 0;

        // 飛行アニメーションに遷移する
        _animator.SetBool("Cooltime", false);
    }

    // ゲームクリア時に呼び出されるメソッド
    public void GameClear()
    {
        if (!IsFlying)
        {
            Debug.LogWarning("クリアメソッドが飛行中以外に呼び出されました。");
            return;
        }
        _status = E_RobotStatus.EndFly;

        // クリア時のアニメーションなどのロボット関係の処理
        _animator.SetTrigger("Clear");
        if (_partsObject)
        {
            _purgeManager.AddPartsBySprite(usingPartsSprite);
            usingPartsSprite = null;
            Destroy(_partsObject.gameObject);
        }
        DestroyAllObjectsWithParts();
        if (_usePartsAudioSource) _usePartsAudioSource.Stop();
    }

    // ゲーム失敗時に呼び出されるメソッド
    public void GameOver()
    {
        if (!IsFlying)
        {
            Debug.LogWarning("ゲームオーバーメソッドが飛行中以外に呼び出されました。");
            return;
        }
        _status = E_RobotStatus.EndFly;

        // ゲーム失敗時のアニメーションなどのロボット関係の処理
        _purgeManager.AddPartsBySprite(GameOverRobotPurgeData);
        if (_partsObject)
        {
            _purgeManager.AddPartsBySprite(usingPartsSprite);
            usingPartsSprite = null;
            Destroy(_partsObject.gameObject);
        }
        DestroyAllObjectsWithParts();
        if (_usePartsAudioSource) _usePartsAudioSource.Stop();
    }

    // カスタムメニューを開いた際に呼び出されるメソッド
    public void OpenCustomMenu()
    {
        if (PlaySceneController.Instance.IsOpenableCustomMenu)
        {
            bodyCollider.enabled = false;
            _status = E_RobotStatus.EndFly;

            if (_usePartsAudioSource) _usePartsAudioSource.Stop();

            if (_partsObject)
            {
                _purgeManager.AddPartsBySprite(usingPartsSprite);
                usingPartsSprite = null;
                Destroy(_partsObject.gameObject);
            }
            DestroyAllObjectsWithParts();
        }
    }

    // 初めからやり直す際に呼び出されるメソッド
    public void ResetStatus()
    {
        // アニメーターの状態をリセットする
        _animator.SetBool("Cooltime", false);
        _animator.SetBool("OnGround", true);
        _animator.Play("robot");    // robotステートに切り替える

        bodyCollider.enabled = true;
        _status = E_RobotStatus.Ready;
        cooltime = 0;
    }



    // アニメーターに地面に接しているかを伝える
    public void SetOnGround(bool IsGround)
    {
        _animator.SetBool("OnGround", IsGround);
    }

    // パーツデータから装備するパーツを構築する
    public void BuildUsePartsObject(PartsPerformance performance, PartsInfo.PartsData data)
    {
        // 位置と角度を調整する
        Vector3 localPosition = Vector3.zero;
        float angle = data.angle - 90;
        switch (performance.forceType)
        {
            case PartsPerformance.E_ForceType.Rocket:
                localPosition += (Vector3)_rocketLocate;
                break;
            case PartsPerformance.E_ForceType.Propeller:
                localPosition += (Vector3)_propellerLocate;
                break;
            case PartsPerformance.E_ForceType.Glider:
                localPosition += (Vector3)_gliderLocate;
                angle += 90;
                break;
            // 爆弾は装備している瞬間がほぼ無いので、パーツオブジェクトは生成しない
            case PartsPerformance.E_ForceType.Bomb:
                // 爆発生成処理
                if (_explodesPrefab)
                {
                    Vector3 position = Quaternion.Euler(0, 0, data.angle - 180) * Vector3.right * _bombExplodeDistance;
                    var explodes = Instantiate(_explodesPrefab, transform.position + position, Quaternion.Euler(-90, 0, 0));
                    explodes.Play();
                    GimickManager.Instance.RegisterAsDeleteObject(explodes.gameObject);

                    // 使用時の効果音を鳴らす
                    if(explodes.TryGetComponent(out AudioSource audiouSource))
                    {
                        audiouSource.clip = performance.usePartsSE;
                        audiouSource.Play();
                    }
                }
                return;
            default:
                return;
        }

        // オブジェクトを生成する
        _partsObject = Instantiate(_partsPrefab, transform);
        _partsObject.transform.localPosition = localPosition;
        _partsObject.transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        if (performance.animatorController)
        {
            // アニメーターを設定する
            var partsAnimator = _partsObject.gameObject.AddComponent<Animator>();
            partsAnimator.runtimeAnimatorController = performance.animatorController;
        }
        else
        {
            // スプライト（静止画）を設定する
            _partsObject.sprite = performance.partsSprite;
        }
    }

    // パーツの使用終了と共に破棄されるパーツの登録
    public void RegisterObjectAsDestroyWithParts(GameObject target)
    {
        _destroyWithPartsObjects.Add(target);
    }
    private void DestroyAllObjectsWithParts()
    {
        // 終了と共に削除するオブジェクトの削除
        for (int i = 0; i < _destroyWithPartsObjects.Count; i++)
        {
            if (_destroyWithPartsObjects[i])
            {
                Destroy(_destroyWithPartsObjects[i]);
            }
        }
        _destroyWithPartsObjects.Clear();
    }
}
