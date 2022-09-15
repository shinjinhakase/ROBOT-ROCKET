using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����̌��̐����̃X�v���C�g���Ǘ�����Component
public class NumbersManager : MonoBehaviour
{
    [SerializeField] private int Number;
    [SerializeField] private List<NumberBoard> numbers = new List<NumberBoard>();

    // �������X�V����
    public void UpdateNum(int Num)
    {
        Num = Mathf.Abs(Num);
        for (int i = numbers.Count - 1; i >= 0; i--)
        {
            numbers[i].SetNumber(Num % 10);
            Num /= 10;
        }
    }
}
