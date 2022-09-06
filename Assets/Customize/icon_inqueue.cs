using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Icon_inqueue : MonoBehaviour{

    //アクティブ状態かどうか
    private bool isActive=false;

    public int mynumber=0;

    private bool change_alpha=false;

    SpriteRenderer _renderer;
    
    void Awake(){

        isActive=true;
        mynumber=Queue.testlist.Count;

        _renderer=GetComponent<SpriteRenderer>();

        this.gameObject.AddComponent<EventTrigger>();
        EventTrigger _trigger=this.GetComponent<EventTrigger>();
        EventTrigger.Entry _entry=new EventTrigger.Entry();
        _entry.eventID=EventTriggerType.PointerClick;
        _entry.callback.AddListener((eventData)=>{Click();});
        _trigger.triggers.Add(_entry);
        

    }

    void Update(){

        if(isActive){

            _renderer.color=new Color32(255,255,255,255);

        }else{

            if(change_alpha==false){

                _renderer.color=new Color32(255,255,255,100);
                change_alpha=true;

            }

        }

        if(mynumber==Queue.nowActive){

            isActive=true;
            change_alpha=false;

        }else{

            isActive=false;

        }
        
    }

    public void Click(){

        if(Switch.selected==false){

            Queue.nowActive=mynumber;

        }else{

            //--------------------
            //アイコンの入れ替え処理
            //--------------------

            //選択中のアイテムの情報を取り出す
            GameObject selected_item=Queue.testlist[Queue.nowActive];
            GameObject selected_icon=Queue.icon_list[Queue.nowActive];

            //宛先のアイテムの情報を取り出す
            GameObject address_item=Queue.testlist[mynumber];
            GameObject address_icon=Queue.icon_list[mynumber];

            //座標の入れ替え
            Vector2 tmp_coodinate=selected_icon.transform.position;
            selected_icon.transform.position=address_icon.transform.position;
            address_icon.transform.position=tmp_coodinate;

            //Queue内の順番の入れ替え
            Queue.testlist.Replace(Queue.nowActive,mynumber);
            Queue.icon_list.Replace(Queue.nowActive,mynumber);

            //mynumberの入れ替え
            Items selected_item_script=selected_item.GetComponent<Items>();
            Icon_inqueue selected_icon_script=selected_icon.GetComponent<Icon_inqueue>();
            Items address_item_script=address_item.GetComponent<Items>();
            Icon_inqueue address_icon_script=address_icon.GetComponent<Icon_inqueue>();
            int tmp_number=selected_item_script.mynumber;
            selected_item_script.mynumber=address_item_script.mynumber;
            selected_icon_script.mynumber=address_icon_script.mynumber;
            address_item_script.mynumber=tmp_number;
            address_icon_script.mynumber=tmp_number;
            Queue.nowActive=address_item_script.mynumber;

            //入れ替えスイッチをオフにする
            Switch.selected=false;
            
        }
        

    }

    public void Delete_icon(){
        
        Destroy(this.gameObject);

    }

}
