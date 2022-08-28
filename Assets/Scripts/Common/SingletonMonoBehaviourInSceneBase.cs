using System;
using UnityEngine;

// �V�[����1�������݂��Ȃ��I�u�W�F�N�g�̃V���O���g��
// �p������ƁA�N���X��.Instance�ŃC���X�^���X�ɃA�N�Z�X�\�ɂȂ�
public class SingletonMonoBehaviourInSceneBase<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if(null != Instance && Instance != this)
        {
            // �V�[����2�ȏ�C���X�^���X�����݂���ꍇ�́A�G���[���o��
            throw new Exception("�V�[�����ɑ��̃C���X�^���X�����݂��Ă��܂��I");
        }

        Instance = this as T;
    }
}
