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

            // パーツの方を生成する
            GameObject newitem=Instantiate(item, custom_panel.transform) as GameObject;
            PartsPerformance _performance = _data.getData(myList[i].id);
            float initDrawAngle = -90f;
            if (_performance.forceType == PartsPerformance.E_ForceType.Glider) initDrawAngle = 0f;
            newitem.GetComponent<Items>().InitialSetting(itemlist.Count, _performance, myList[i].angle, initDrawAngle);

            // アイコンの方を生成する
            GameObject newicon=Instantiate(icon, transform) as GameObject;
            newicon.GetComponent<Icon_inqueue>().InitialSetting(itemlist.Count, _performance);

            itemlist.Add(newitem);
            icon_list.Add(newicon);

            nowActive=itemlist.Count-1;

        }

        Customize.obj_angle = myList[nowActive].angle;

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
            newparts.angle=angle;
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