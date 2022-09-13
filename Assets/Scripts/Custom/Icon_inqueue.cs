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
    
    void Start(){

        isActive=true;

        _renderer=GetComponent<SpriteRenderer>();

        //自身にクリックされたら反応するEventTriggerを追加
        this.gameObject.AddComponent<EventTrigger>();
        EventTrigger _trigger=this.GetComponent<EventTrigger>();
        EventTrigger.Entry _entry=new EventTrigger.Entry();
        _entry.eventID=EventTriggerType.PointerClick;
        _entry.callback.AddListener((eventData)=>{Click();});
        _trigger.triggers.Add(_entry);

    }

    void Update(){

        //透明度の更新
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

        //入れ替え中でなければアクティブ状態の番号にmynumberを渡す
        if(Switch.selected==false){

            Queue.nowActive=mynumber;

        //入れ替え中であればアイコンの入れ替え処理を行う
        }else{

            //--------------------
            //アイコンの入れ替え処理
            //--------------------

            //選択中のアイテムの情報を取り出す
            GameObject selected_item=Queue.itemlist[Queue.nowActive];
            GameObject selected_icon=Queue.icon_list[Queue.nowActive];

            //選択中のアイテムのスクリプトを取得
            Items selected_item_script=selected_item.GetComponent<Items>();
            Icon_inqueue selected_icon_script=selected_icon.GetComponent<Icon_inqueue>();

            if(selected_item_script.mynumber==mynumber){

                return;

            }else{

                //元のアイテムを削除
                Queue.itemlist.RemoveAt(Queue.nowActive);
                Queue.icon_list.RemoveAt(Queue.nowActive);
                
                //Queueに追加
                Queue.itemlist.Insert(mynumber,selected_item);
                Queue.icon_list.Insert(mynumber,selected_icon);

                Scroll_bar.reset_flag=true;
                Queue.reset_flag=true;

                for(int i=0;i<Queue.itemlist.Count;i++){

                    Items move_item_script=Queue.itemlist[i].GetComponent<Items>();
                    move_item_script.mynumber=i;
                    

                    Icon_inqueue move_icon=Queue.icon_list[i].GetComponent<Icon_inqueue>();
                    move_icon.mynumber=i;
                    Vector2 new_drawposition=new Vector2(7.6f,4.0f-i);
                    move_icon.transform.localPosition = new_drawposition;

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
