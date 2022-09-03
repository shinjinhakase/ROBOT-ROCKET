using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���̃v���C�̃��v���C�p�f�[�^���i�[����N���X�B�i�����ł�frame�Ƃ́AFixedUpdate�̏����P�ʎ��Ԃł̂��Ɓj
[Serializable]
public class ReplayData
{
    // �X�e�[�W�ԍ�
    public int StageNum = -1;
    // ���O�ɏ������Ă����p�[�c�̃��X�g
    public List<PartsInfo.PartsData> readyPartsList = new List<PartsInfo.PartsData>();
    // �����Ŋl�������p�[�c�̃��X�g
    public List<GetPartsData> getPartsList = new List<GetPartsData>();
    // �A�C�e���̎g�p�^�C�~���O�̃��X�g
    public List<int> usePartsFrame = new List<int>();

    // ���{�b�g�̍��W�f�[�^�̃��X�g
    public List<LocateData> locateDatas = new List<LocateData>();
    // ���{�b�g�ɉ�������͂̃��X�g
    public List<ForceData> forceDatas = new List<ForceData>();

    // �Q�[���I���^�C�~���O�ƋL�^
    public int finishFrame = 0;
    public float score = 0f;


    // �f�[�^�����������āA���������p�[�c�f�[�^��ǂݍ���
    public void ReadyPartsInfo(int StageNum)
    {
        this.StageNum = StageNum;
        readyPartsList = new List<PartsInfo.PartsData>(PartsInfo.Instance.GetPartsList());
        getPartsList.Clear();
        usePartsFrame.Clear();
        locateDatas.Clear();
        forceDatas.Clear();
        finishFrame = 0;
        score = 0f;
    }
    // �p�[�c�̎g�p�^�C�~���O���L�^����
    public void RegisterUseParts(int frame)
    {
        usePartsFrame.Add(frame);
    }
    // �l�������p�[�c�f�[�^���L�^����
    public void RegisterGetParts(int frame, PartsPerformance.E_PartsID id, float Angle)
    {
        getPartsList.Add(new GetPartsData(frame, id, Angle));
    }
    // ���{�b�g�̍��W�f�[�^���L�^����
    public void RegisterRobotTransform(int frame, Vector2 position, Vector2 velocity)
    {
        locateDatas.Add(new LocateData(frame, position, velocity));
    }
    // ���{�b�g�ɉ�������͂��L�^����
    public void RegisterRobotForce(int frame, IForce force)
    {
        // TODO�F�r���Ŏ蓮�p�[�W�����ꍇ�̎��Ԃ̑Ή��A�����蔻��^�̗͂̑Ή�
        Debug.LogWarning("�͂̃��v���C�f�[�^�ۑ��@�\�͂܂����S�Ɏ����ł��Ă��܂���B");
        if (typeof(PressForce) == force.GetType())
        {
            PressForce pressForce = (PressForce)force;
            ForceData forceData = new ForceData(frame, PartsPerformance.E_ForceType.Rocket,
                pressForce.Angle, t: pressForce.t, F: pressForce.F, k: pressForce.k);
            forceDatas.Add(forceData);
        }
        else if (typeof(ImpulseForce) == force.GetType())
        {
            ImpulseForce impulseForce = (ImpulseForce)force;
            ForceData forceData = new ForceData(frame, PartsPerformance.E_ForceType.Bomb,
                impulseForce.Angle, F: impulseForce.F);
            forceDatas.Add(forceData);
        }
        else if (typeof(GliderForce) == force.GetType())
        {
            GliderForce gliderForce = (GliderForce)force;
            ForceData forceData = new ForceData(frame, PartsPerformance.E_ForceType.Glider,
                gliderForce.Angle, t: gliderForce.t, F: gliderForce.F, k: gliderForce.R);
            forceDatas.Add(forceData);
        }
        else if (typeof(CollisionForce) == force.GetType())
        {
            // ���Ԃ̑Ή������������܂�
            CollisionForce collisionForce = (CollisionForce)force;
            ForceData forceData = new ForceData(frame, PartsPerformance.E_ForceType.CollisionForce,
                collisionForce.Angle, t: 0, F: collisionForce.F, k: collisionForce.k);
            forceDatas.Add(forceData);
        }
        throw new Exception("��������͂����v���C�ɕۑ��ł��܂���ł����B");
    }
    // �I�����ʂ��L�^����
    public void RegisterResult(int frame, float score)
    {
        finishFrame = frame;
        this.score = score;
    }

    // ���{�b�g�̈ʒu�����i�[����N���X
    [Serializable]
    public class LocateData
    {
        public int frame;   // �o�߃t���[����
        public Vector2 position;    // ���W
        public Vector2 velocity;    // ���x
        public LocateData(int frame, Vector2 position, Vector2 velocity)
        {
            this.frame = frame;
            this.position = position;
            this.velocity = velocity;
        }
    }
    // ��������͂��L�^����N���X
    [Serializable]
    public class ForceData
    {
        public int frame;   // �o�߃t���[����
        public PartsPerformance.E_ForceType forceType;
        public float t; // ���ԁi�b�j
        public float Angle;
        public float F;
        public float k;
        public ForceData(int frame, PartsPerformance.E_ForceType forceType, float Angle,
            float t = 0, float F = 0, float k = 0)
        {
            this.frame = frame;
            this.forceType = forceType;
            this.t = t;
            this.F = F;
            this.k = k;
        }
        // �f�[�^�����ɗ͂��\�z����
        public IForce buildForce()
        {
            switch (forceType)
            {
                // �u�ԓI�ȗ͂͊p�x��F�����ōČ��\
                case PartsPerformance.E_ForceType.Bomb:
                    return new ImpulseForce(Angle, F);
                // �p���I�ȗ͂̓p�[�W�����^�C�~���O�����ɍ\�z����
                // �����蔻��ɂ��Ă��͂�����������Ԃ��������Ă���̂ŁAPressForce�ōČ�����
                case PartsPerformance.E_ForceType.Rocket:
                case PartsPerformance.E_ForceType.Propeller:
                case PartsPerformance.E_ForceType.CollisionForce:
                    return new PressForce(Angle, F, t, k);
                // �O���C�_�[�͈���R�̏����t�B�[���hk�Ɋi�[���Ă���̂ŁAk�ŃC���X�^���X������
                case PartsPerformance.E_ForceType.Glider:
                    return new GliderForce(Angle, F, t, k);
                // �͖����͑Ή����Ă��Ȃ��̂ŃG���[
                case PartsPerformance.E_ForceType.NoForce:
                default:
                    throw new Exception("���v���C�p�̗͂̍\�z�Ɏ��s���܂����B");
            }
        }
    }
    // �l�������p�[�c�f�[�^���i�[����N���X
    [Serializable]
    public class GetPartsData
    {
        public int frame;
        public PartsPerformance.E_PartsID id;
        public float Angle;
        public GetPartsData(int frame, PartsPerformance.E_PartsID id, float Angle)
        {
            this.frame = frame;
            this.id = id;
            this.Angle = Angle;
        }
        // PartsInfo�p�̃p�[�c�f�[�^���\�z����
        public PartsInfo.PartsData buildPartsData()
        {
            var data = new PartsInfo.PartsData();
            data.id = id;
            data.angle = Angle;
            return data;
        }
    }
}
