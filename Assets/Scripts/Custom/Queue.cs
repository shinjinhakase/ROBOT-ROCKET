using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour{
    
    private PartsInfo _partsInfo;
    public PartsPerformanceData _data;
    private List<PartsInfo.PartsData> myList; 

    //アイテム用配列
    public static List<GameObject> itemlist;

    //アイコン用配列
    public static List<GameObject> icon_list;

    public static float draw_position=4;

    //今何番目のアイテムがアクティブになっているか
    public static int nowActive=0;

    Vector2 height;

    public GameObject item;
    public GameObject icon;

    public GameObject custom_panel;

    public static bool reset_flag = false;
    [SerializeField] private bool IsDestroyAfterUpdate = true;

    void Awake(){

        _partsInfo = PartsInfo.Instance;
        myList=_partsInfo.GetPartsList();

        //テスト用配列
        itemlist=new List<GameObject>();
        icon_list=new List<GameObject>();

        //myListをGameObjectの配列に変形
        for(int i=0;i<myList.Count;i++){
            
            GameObject newitem=Instantiate(item, custom_panel.transform) as GameObject;
            SpriteRenderer newitem_renderer=newitem.GetComponent<SpriteRenderer>();
            newitem_renderer.sprite=_data.getData(myList[i].id).partsSprite;

            //グライダーの場合大きさを修正
            /*
            if(myList[i].id==PartsPerformance.E_PartsID.Glider){

                newitem.transform.localScale=new Vector3(5,5,1);

            }
            */

            Items newitem_script=newitem.GetComponent<Items>();
            newitem_script.mynumber=itemlist.Count;

            if(myList[i].angle==180){

                newitem_script.myangle=0;

            }else{

                newitem_script.myangle=myList[i].angle-90f;
                
            }

            newitem.transform.localPosition=new Vector3(3.5f,0f,0f);

            GameObject newicon=Instantiate(icon, transform) as GameObject;
            Icon_inqueue newicon_script=newicon.GetComponent<Icon_inqueue>();
            newicon_script.mynumber=itemlist.Count;
            newicon_script.id=myList[i].id;
            SpriteRenderer newicon_renderer=newicon.GetComponent<SpriteRenderer>();
            newicon_renderer.sprite=_data.getData(myList[i].id).iconSprite;
            newicon.transform.parent=this.transform;

            if(icon_list.Count==0){

                draw_position=4.0f;
            
            }else{
            
                GameObject last_icon=icon_list[icon_list.Count-1];
                Vector2 last_position=last_icon.transform.localPosition;
                draw_position=last_position.y-=1.0f;

            }

            newicon.transform.localPosition=new Vector2(7.6f,draw_position);

            itemlist.Add(newitem);
            icon_list.Add(newicon);

            nowActive=itemlist.Count-1;

        }

        height=this.transform.localPosition;

    }

    void Update(){

        /*
        if(itemlist.Count>=10){

            float max_height=0.7f+(itemlist.Count-10f);
            
            height=this.transform.position;
            height.y=max_height-max_height*Scroll_bar.current_height;
            transform.localPosition = height;

        }

        if(reset_flag){

            Reset();
            reset_flag=false;
            
        }
        */

    }

    [ContextMenu("デバッグ")]
    public void Update_PartsInfo(){

        _partsInfo.GetPartsList().Clear();

        for(int i=0;i<itemlist.Count;i++){

            PartsPerformance.E_PartsID id=icon_list[i].GetComponent<Icon_inqueue>().id;
            float angle=itemlist[i].GetComponent<Items>().myangle;
            PartsInfo.PartsData newparts=new PartsInfo.PartsData();
            newparts.id=id;
            newparts.angle=angle+90;
            if (_data.getData(id).forceType == PartsPerformance.E_ForceType.Glider) newparts.angle -= 90;
            _partsInfo.AddParts(newparts);

        }

        _partsInfo.Save();

        Custom_button.panel_on=false;

        if (IsDestroyAfterUpdate) Destroy(custom_panel);

        //テスト用
        //SceneManager.LoadScene("Play");

    }

    void Reset(){

        Vector3 currentPosition=this.transform.position;
        currentPosition.y=0f;
        this.transform.position=currentPosition;

    }

}