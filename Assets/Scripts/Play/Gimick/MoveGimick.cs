using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����I�Ȓ����ړ�������M�~�b�N��Component�B
public class MoveGimick : GimickBase
{
    [SerializeField] private List<MoveTransformData> moveTransformDatas = new List<MoveTransformData>();
    private int Length => moveTransformDatas.Count;

    private bool IsStarted = false;
    private Vector2 homePosition;       // �����ʒu
    private Vector2 subHomePosition;    // �����n�߂̈ʒu
    private Vector2 direction;          // ��������
    private int index = 0;      // �Q�ƃC���f�b�N�X
    private int lastFrame = 0;  // �I���t���[��
    private int frameCnt = 0;   // �t���[���J�E���g

    private void FixedUpdate()
    {
        if (IsStarted)
        {
            // �f�[�^�X�V
            if (frameCnt == 0)
            {
                // �ʒu���X�V
                MoveTransformData nowData = moveTransformDatas[index];
                MoveTransformData nextData = moveTransformDatas[(index + 1) % Length];
                subHomePosition = homePosition + nowData._position;
                direction = nextData._position - nowData._position;

                // �ŏI�t���[�����v�Z
                lastFrame = Mathf.RoundToInt(nowData._time / Time.fixedDeltaTime);
                if (lastFrame <= 0) lastFrame = 1;
            }

            // ���W���X�V
            transform.position = subHomePosition + direction * frameCnt / lastFrame;

            // �f�[�^�X�V����
            frameCnt++;
            if (frameCnt >= lastFrame)
            {
                index = (index + 1) % Length;
                frameCnt = 0;
            }
        }
    }

    // �V�[�����J�n�����ۂɌĂ΂�郁�\�b�h
    public override void OnSceneStart() {
        homePosition = new Vector3(transform.position.x, transform.position.y);
        if (Length > 0) transform.position = homePosition + moveTransformDatas[0]._position;
    }

    // ���{�b�g�������n�߂��ۂɓ����𓯊����郁�\�b�h
    public override void OnStartRobot() {
        index = 0;
        frameCnt = 0;
        if (Length > 1) IsStarted = true;
    }

    // �M�~�b�N�����Z�b�g���郁�\�b�h
    public override void ResetGimick() {
        IsStarted = false;
        if (Length > 1) transform.position = homePosition + moveTransformDatas[0]._position;
    }

    [Serializable]
    private class MoveTransformData
    {
        [Tooltip("�����ʒu����̑��΍��W")]
        public Vector2 _position;
        [Tooltip("���̈ʒu���玟�̈ʒu�Ɉړ�����̂Ɋ|���鎞��")]
        public float _time;
    }
}
