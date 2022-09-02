using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDataBase", menuName = "ScriptableObjects/StageDataBase")]
public class StageDataBase : ScriptableObject
{
    public List<Stage> stageList = new List<Stage>();
}
