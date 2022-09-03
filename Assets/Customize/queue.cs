using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class queue : MonoBehaviour{
    
    private PartsInfo partsInfo;
    private List<PartsInfo.PartsData> myList; 

    //icon.csからオブジェクトを受け取る
    public static GameObject catch_icon=null;

    //テスト用配列
    private List<GameObject> testlist;

    public float draw_position;

    void Start(){

        partsInfo = PartsInfo.Instance;
        myList=partsInfo.GetPartsList();

        //テスト用配列
        testlist=new List<GameObject>();

    }

    void Update(){

        if(catch_icon!=null){

            //テスト用配列
            testlist.Add(catch_icon);

            Draw();
            catch_icon=null;

        }

    }

    //描画処理
    void Draw(){

        GameObject draw_icon=Instantiate(catch_icon) as GameObject;
        draw_icon.transform.position=new Vector2(8,draw_position);
        draw_position-=1.0f;

    }

}