using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour{
    
    private PartsInfo partsInfo;
    private List<PartsInfo.PartsData> myList; 

    //テスト用配列
    public static List<GameObject> testlist;

    //アイコン用配列
    public static List<GameObject> icon_list;

    public static float draw_position=4;

    //今何番目のアイテムがアクティブになっているか
    public static int nowActive=0;

    void Start(){

        partsInfo = PartsInfo.Instance;
        myList=partsInfo.GetPartsList();

        //テスト用配列
        testlist=new List<GameObject>();

        icon_list=new List<GameObject>();

    }

    void Update(){

        Debug.Log("nowActive:"+nowActive);

    }

}