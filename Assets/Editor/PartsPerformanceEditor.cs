#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PartsPerformance))]
public class PartsPerformanceEditor : Editor
{
    PartsPerformance _target;
    GUIContent guiC_m = new GUIContent("m", "���ʁB�����Robo�ɓn���đ��d��M���v�Z���A�p�[�W�����m�̕������y���Ȃ�B");
    GUIContent guiC_F = new GUIContent("F", "�͂̑傫���B�͂��傫�����d�����̂𕂂�������A��������������ł���B���d��M���傫���قǉ����x�͌���(ma=F���)�B");
    GUIContent guiC_k = new GUIContent("k", "�W���B��R�͂̌v�Z�ɗp����B���̒l���������A�I�[���x�͒x���Ȃ�(ma=F-kv���Ȃ킿v=F/k���)�B");
    GUIContent guiC_t = new GUIContent("t", "�������ԁB���̎��Ԃ̕������͂�����������B");
    GUIContent guiC_cooltime = new GUIContent("cooltime", "�N�[���^�C���B�A�C�e���̌��ʂ��I�����Ă���A���̃A�C�e�����g����悤�ɂȂ�܂ł̑ҋ@���ԁB");
    GUIContent guiC_name = new GUIContent("partsName", "�p�[�c���BUI�Ńp�[�c���\���ɗp���܂��B");
    GUIContent guiC_flipF = new GUIContent("F", "�g�͂̑傫���B�c�����̗͂Ɋ|����{���Ƃ��Čv�Z�ɗp����B");
    GUIContent guiC_dragF = new GUIContent("R", "�R�͂̑傫���B�������̗͂Ɋ|����{���Ƃ��Čv�Z�ɗp����B��{�g�͂�菬�����B");

    public override void OnInspectorGUI()
    {
        _target = target as PartsPerformance;

        // �\���\�z
        _target.id = (PartsPerformance.E_PartsID)EditorGUILayout.EnumPopup("�p�[�c����ʂ���ID", _target.id);
        _target.forceType = (PartsPerformance.E_ForceType)EditorGUILayout.EnumPopup("�͂̉������̒�`", _target.forceType);

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
                _target.F = EditorGUILayout.FloatField(guiC_flipF, _target.F);
                _target.t = EditorGUILayout.FloatField(guiC_t, _target.t);
                _target.R = EditorGUILayout.FloatField(guiC_dragF, _target.R);
                break;
            case PartsPerformance.E_ForceType.CollisionForce:
                EditorGUILayout.LabelField("�p�[�c�̗͂�CollisionForce�𒼐ڎw�肷�邱�Ƃ͂ł��܂���B");
                EditorGUILayout.LabelField("NoForce�ɂ��āA�����I�u�W�F�N�g����ForceGimick���������Ă��������B");
                break;
            case PartsPerformance.E_ForceType.NoForce:
                break;
        }
        SerializedProperty prop = serializedObject.FindProperty("summonObjects");
        EditorGUILayout.PropertyField(prop, true);

        // UI�֘A���ڂ̕\��
        EditorGUILayout.LabelField("UI�֘A����");
        _target.partsName = EditorGUILayout.TextField(guiC_name, _target.partsName);
        SerializedProperty partsSprite = serializedObject.FindProperty("partsSprite");
        EditorGUILayout.PropertyField(partsSprite, true);
        SerializedProperty animatorController = serializedObject.FindProperty("animatorController");
        EditorGUILayout.PropertyField(animatorController, true);
        SerializedProperty iconSprite = serializedObject.FindProperty("iconSprite");
        EditorGUILayout.PropertyField(iconSprite, true);
        SerializedProperty desc = serializedObject.FindProperty("description");
        EditorGUILayout.PropertyField(desc, true);

        // �T�E���h�֌W�̐ݒ�
        EditorGUILayout.LabelField("SE�֘A����");
        SerializedProperty usePartsSE = serializedObject.FindProperty("usePartsSE");
        EditorGUILayout.PropertyField(usePartsSE, false);
        SerializedProperty purgePartsSE = serializedObject.FindProperty("purgePartsSE");
        EditorGUILayout.PropertyField(purgePartsSE, false);

        // �ύX�����m�����ꍇ�A�ݒ�t�@�C���ɏ����o��
        if (EditorGUI.EndChangeCheck())
        {
            // Dirty�t���O�𗧂ĂāAUnity�I�����ɐݒ��.asset�ɏ����o��
            EditorUtility.SetDirty(target);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif