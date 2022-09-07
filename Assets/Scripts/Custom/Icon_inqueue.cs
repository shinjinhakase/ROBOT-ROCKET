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
    public PartsPerformance.E_PartsID id;
    
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

            //選択中のアイテムのスクリプトを取得
            Items new_item=selected_item.GetComponent<Items>();
            Icon_inqueue new_icon=selected_icon.GetComponent<Icon_inqueue>();

            //座標の入れ替え
            //selected_icon.transform.position=address_icon.transform.position;

            if(new_item.mynumber==mynumber){

                return;

            }else{

                //元のアイテムを削除
                Queue.testlist.RemoveAt(Queue.nowActive);
                Queue.icon_list.RemoveAt(Queue.nowActive);
                
                //Queueに追加
                Queue.testlist.Insert(mynumber,selected_item);
                Queue.icon_list.Insert(mynumber,selected_icon);

                for(int i=0;i<Queue.testlist.Count;i++){

                    Items move_items=Queue.testlist[i].GetComponent<Items>();
                    move_items.mynumber=i;
                    

                    Icon_inqueue move_icon=Queue.icon_list[i].GetComponent<Icon_inqueue>();
                    move_icon.mynumber=i;
                    Vector2 new_drawposition=new Vector2(7.6f,4.0f-i);
                    move_icon.transform.position=new_drawposition;

                }

            }

            //入れ替えスイッチをオフにする
            Switch.selected=false;
            
        }
        

    }

    public void Delete_icon(){
        
        Destroy(this.gameObject);

    }

}
