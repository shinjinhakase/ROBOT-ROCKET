using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour{

    //アクティブ状態かどうか
    private bool isActive=false;

    public int mynumber=0;
    public float myangle=0;
    private float initDrawAngle = 0f;    // カスタムデータの角度とスプライト上での角度のずれ

    private bool change_alpha=false;
    SpriteRenderer _renderer;

    private Vector2 _initialLocalPosition = new Vector2(3.5f, 0);    // 生成時の初期位置

    void Start(){

        isActive=true;

    }

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();

    }

    void Update(){

        if(mynumber==Queue.nowActive){
            
            if(isActive==false){

                Customize.obj_angle=myangle;

            }
            
            isActive=true;
            change_alpha=false;

        }else{

            if(isActive==true){

                this.transform.rotation=Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, myangle + initDrawAngle);

            }
            
            isActive=false;

        }
        
        if(isActive){

            this.transform.rotation=Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Customize.obj_angle + initDrawAngle);
            _renderer.color=new Color32(255,255,255,255);
            myangle=Customize.obj_angle;
            

        }else{

            if(change_alpha==false){

                _renderer.color=new Color32(255,255,255,50);
                change_alpha=true;

            }

        }
        
    }

    public void Delete_items(){
        
        Destroy(this.gameObject);

    }

    // インスタンス化した後の初期化処理
    public void InitialSetting(int partsNo, PartsPerformance performance, float angle = 0f, float initDrawAngle = 0f)
    {
        // 管理関連の初期化
        mynumber = partsNo;
        myangle = angle;
        this.initDrawAngle = initDrawAngle;

        // 描画関連の初期化
        transform.localPosition = _initialLocalPosition;
        _renderer.sprite = performance.partsSprite;
    }
}