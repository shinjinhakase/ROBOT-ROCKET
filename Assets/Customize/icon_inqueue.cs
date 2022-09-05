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

        Queue.nowActive=mynumber;

    }

    public void Delete_icon(){
        
        Destroy(this.gameObject);

    }

}
