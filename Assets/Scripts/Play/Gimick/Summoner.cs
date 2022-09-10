using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : MonoBehaviour
{
    [SerializeField] private SummonableObject summonPrefab;

    public void Summon()
    {
        SummonableObject summonObject = Instantiate(summonPrefab, transform.position, Quaternion.identity);
        summonObject.Summon(null, transform);
    }
}
