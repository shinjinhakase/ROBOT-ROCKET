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

    public override void OnInspectorGUI()
    {
        _target = target as PartsPerformance;

        _target.id = (PartsPerformance.E_PartsID)EditorGUILayout.EnumPopup("�p�[�c����ʂ���ID", _target.id);
        _target.forceType = (PartsPerformance.E_ForceType)EditorGUILayout.EnumPopup("�͂̉������̒�`", _target.forceType);

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
                EditorGUILayout.LabelField("�p�����[�^�v���o�����c");
                break;
            case PartsPerformance.E_ForceType.CollisionForce:
                EditorGUILayout.LabelField("�l�����c�B�X�e�[�W�M�~�b�N�Ɏg���\�肾�����B");
                break;
        }

        // �ύX�����m�����ꍇ�A�ݒ�t�@�C���ɏ����o��
        if (EditorGUI.EndChangeCheck())
        {
            // Dirty�t���O�𗧂ĂāAUnity�I�����ɐݒ��.asset�ɏ����o��
            EditorUtility.SetDirty(target);
        }
    }
}
#endif