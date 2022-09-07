using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour{
    
    private PartsInfo _partsInfo;
    private List<PartsInfo.PartsData> myList; 

    //テスト用配列
    public static List<GameObject> testlist;

    //アイコン用配列
    public static List<GameObject> icon_list;

    public static float draw_position=4;

    //今何番目のアイテムがアクティブになっているか
    public static int nowActive=0;

    Vector2 height;

    void Start(){

        _partsInfo = PartsInfo.Instance;
        myList=_partsInfo.GetPartsList();

        //テスト用配列
        testlist=new List<GameObject>();

        icon_list=new List<GameObject>();

        height=this.transform.position;

    }

    void Update(){

        //10koijyou +0.7　soreikouha 1きざみ
        if(testlist.Count>=10){

            float max_height=0.7f+(testlist.Count-10f);
            height.y=max_height-max_height*Scroll_bar.current_height;
            transform.position=height;

        }

    }

    [ContextMenu("デバッグ")]
    public void Update_PartsInfo(){

        _partsInfo.GetPartsList().Clear();

        for(int i=0;i<testlist.Count;i++){

            PartsPerformance.E_PartsID id=icon_list[i].GetComponent<Icon_inqueue>().id;
            float angle=testlist[i].GetComponent<Items>().myangle;
            PartsInfo.PartsData newparts=new PartsInfo.PartsData();
            newparts.id=id;
            newparts.angle=angle+90;
            _partsInfo.AddParts(newparts);

        }

        _partsInfo.Save();

    }

}