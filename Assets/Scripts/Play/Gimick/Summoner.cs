using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : MonoBehaviour
{
    [SerializeField] private SummonableObject summonPrefab;
    [SerializeField] private bool IsSetInitVelocity = false;
    [SerializeField] private Vector2 initVelocity = Vector2.zero;

    public void Summon()
    {
        SummonableObject summonObject = Instantiate(summonPrefab, transform.position, Quaternion.identity);
        if (IsSetInitVelocity)
        {
            summonObject.Summon(null, transform, initVelocity);
        }
        else
        {
            summonObject.Summon(null, transform);
        }
    }
}
