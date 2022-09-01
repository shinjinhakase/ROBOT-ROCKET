#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PartsPerformance))]
public class PartsPerformanceEditor : Editor
{
    PartsPerformance _target;
    GUIContent guiC_m = new GUIContent("m", "質量。これをRoboに渡して総重量Mを計算し、パージすればmの分だけ軽くなる。");
    GUIContent guiC_F = new GUIContent("F", "力の大きさ。力が大きい程重いものを浮かせたり、早く加速したりできる。総重量Mが大きいほど加速度は減る(ma=Fより)。");
    GUIContent guiC_k = new GUIContent("k", "係数。抵抗力の計算に用いる。この値が高い程、終端速度は遅くなる(ma=F-kvすなわちv=F/kより)。");
    GUIContent guiC_t = new GUIContent("t", "持続時間。この時間の分だけ力を加え続ける。");
    GUIContent guiC_cooltime = new GUIContent("cooltime", "クールタイム。アイテムの効果が終了してから、次のアイテムが使えるようになるまでの待機時間。");
    GUIContent guiC_name = new GUIContent("partsName", "パーツ名。UIでパーツ名表示に用います。");

    public override void OnInspectorGUI()
    {
        _target = target as PartsPerformance;

        // 表示構築
        _target.id = (PartsPerformance.E_PartsID)EditorGUILayout.EnumPopup("パーツを区別するID", _target.id);
        _target.forceType = (PartsPerformance.E_ForceType)EditorGUILayout.EnumPopup("力の加わり方の定義", _target.forceType);

        _target.cooltime = EditorGUILayout.FloatField(guiC_cooltime, _target.cooltime);
        _target.m = EditorGUILayout.FloatField(guiC_m, _target.m);
        switch (_target.forceType)
        {
            case PartsPerformance.E_ForceType.Bomb:
                _target.F = EditorGUILayout.FloatField(guiC_F, _target.F);
                break;
            case PartsPerformance.E_ForceType.Rocket:
                _target.F = EditorGUILayout.FloatField(guiC_F, _target.F);
                _target.t = EditorGUILayout.FloatField(guiC_t, _target.t);
                _target.k = EditorGUILayout.FloatField(guiC_k, _target.k);
                break;
            case PartsPerformance.E_ForceType.Propeller:
                _target.F = EditorGUILayout.FloatField(guiC_F, _target.F);
                _target.t = EditorGUILayout.FloatField(guiC_t, _target.t);
                _target.k = EditorGUILayout.FloatField(guiC_k, _target.k);
                break;
            case PartsPerformance.E_ForceType.Glider:
                _target.F = EditorGUILayout.FloatField(guiC_F, _target.F);
                _target.k = EditorGUILayout.FloatField(guiC_k, _target.k);
                EditorGUILayout.LabelField("パラメータ思い出し中…");
                break;
            case PartsPerformance.E_ForceType.CollisionForce:
                EditorGUILayout.LabelField("パーツの力にCollisionForceを直接指定することはできません。");
                EditorGUILayout.LabelField("NoForceにして、召喚オブジェクトからForceGimickを召喚してください。");
                break;
            case PartsPerformance.E_ForceType.NoForce:
                break;
        }
        SerializedProperty prop = serializedObject.FindProperty("summonObjects");
        EditorGUILayout.PropertyField(prop, true);

        // UI関連項目の表示
        EditorGUILayout.LabelField("UI関連項目");
        _target.partsName = EditorGUILayout.TextField(guiC_name, _target.partsName);
        SerializedProperty desc = serializedObject.FindProperty("description");
        EditorGUILayout.PropertyField(desc, true);

        // 変更を検知した場合、設定ファイルに書き出す
        if (EditorGUI.EndChangeCheck())
        {
            // Dirtyフラグを立てて、Unity終了時に設定を.assetに書き出す
            EditorUtility.SetDirty(target);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif