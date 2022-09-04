using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class queue : MonoBehaviour{
    
    private PartsInfo partsInfo;
    private List<PartsInfo.PartsData> myList; 

    //テスト用配列
    private List<GameObject> testlist;

    public static float draw_position=4;

    //今何番目のアイテムがアクティブになっているか
    public static int nowActive=0;

    void Start(){

        partsInfo = PartsInfo.Instance;
        myList=partsInfo.GetPartsList();

        //テスト用配列
        testlist=new List<GameObject>();

    }

    void Update(){

    }

}