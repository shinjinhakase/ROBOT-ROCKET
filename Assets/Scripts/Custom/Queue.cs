using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//テスト用
using UnityEngine.SceneManagement;

public class Queue : MonoBehaviour{
    
    private PartsInfo _partsInfo;
    public PartsPerformanceData _data;
    private List<PartsInfo.PartsData> myList; 

    //テスト用配列
    public static List<GameObject> testlist;

    //アイコン用配列
    public static List<GameObject> icon_list;

    public static float draw_position=4;

    //今何番目のアイテムがアクティブになっているか
    public static int nowActive=0;

    Vector2 height;

    public GameObject item;
    public GameObject icon;

    public GameObject custom_panel;

    void Start(){

        _partsInfo = PartsInfo.Instance;
        myList=_partsInfo.GetPartsList();

        //テスト用配列
        testlist=new List<GameObject>();
        icon_list=new List<GameObject>();

        //myListをGameObjectの配列に変形
        for(int i=0;i<myList.Count;i++){
            
            GameObject newitem=Instantiate(item) as GameObject;
            SpriteRenderer newitem_renderer=newitem.GetComponent<SpriteRenderer>();
            newitem_renderer.sprite=_data.getData(myList[i].id).partsSprite;
            Items newitem_script=newitem.GetComponent<Items>();
            newitem_script.mynumber=testlist.Count;

            if(myList[i].angle==180){

                newitem_script.myangle=0;

            }else{

                newitem_script.myangle=myList[i].angle-90f;
                
            }

            newitem.transform.parent=custom_panel.transform;
            newitem.transform.position=new Vector3(3.5f,0f,0f);

            GameObject newicon=Instantiate(icon) as GameObject;
            Icon_inqueue newicon_script=newicon.GetComponent<Icon_inqueue>();
            newicon_script.mynumber=testlist.Count;
            newicon_script.id=myList[i].id;
            SpriteRenderer newicon_renderer=newicon.GetComponent<SpriteRenderer>();
            newicon_renderer.sprite=_data.getData(myList[i].id).iconSprite;
            newicon.transform.parent=custom_panel.transform;

            if(icon_list.Count==0){

                draw_position=4.0f;
            
            }else{
            
                GameObject last_icon=icon_list[icon_list.Count-1];
                Vector2 last_position=last_icon.transform.position;
                draw_position=last_position.y-=1.0f;

            }

            newicon.transform.position=new Vector2(7.6f,draw_position);

            testlist.Add(newitem);
            icon_list.Add(newicon);

            nowActive=testlist.Count-1;

        }

        height=this.transform.position;

    }

    void Update(){

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

        Custom_button.panel_on=false;

        Destroy(custom_panel);

        //テスト用
        //SceneManager.LoadScene("Play");

    }

}