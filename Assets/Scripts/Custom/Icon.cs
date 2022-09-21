using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Icon : MonoBehaviour{
    
    public GameObject pop;
    public GameObject add_queue;

    GameObject queue_obj;

    public PartsPerformance.E_PartsID id;
    public PartsPerformanceData _data;
    private PartsPerformance _performance;

    public int tabnumber;

    public GameObject custom_panel;

    public Text drawtext;
    [NonSerialized] public string description;
    
    void Start(){

        queue_obj=null;
        _performance=_data.getData(id);
        SpriteRenderer _renderer=this.GetComponent<SpriteRenderer>();
        _renderer.sprite=_performance.iconSprite;
        description = _performance.description;
    }

    void Update(){

        if(tabnumber==Tab.tabnumber){

            this.gameObject.GetComponent<BoxCollider2D>().enabled=true;

        }else{

            this.gameObject.GetComponent<BoxCollider2D>().enabled=false;

        }

        if(queue_obj==null){

            queue_obj=custom_panel.transform.Find("queue").gameObject;
            
        }

    }

    public void Click(){

        // パーツの方を生成する
        GameObject pop_item=Instantiate(pop, custom_panel.transform) as GameObject;
        float initDrawAngle = -90f;
        if (_performance.forceType == PartsPerformance.E_ForceType.Glider) initDrawAngle = 0f;
        pop_item.GetComponent<Items>().InitialSetting(Queue.itemlist.Count, _performance, initDrawAngle: initDrawAngle);

        // アイコンの方を生成する
        GameObject draw_icon=Instantiate (add_queue, queue_obj.transform) as GameObject;
        draw_icon.GetComponent<Icon_inqueue>().InitialSetting(Queue.itemlist.Count, _performance);

        Queue.itemlist.Add(pop_item);
        Queue.icon_list.Add(draw_icon);

        Queue.nowActive=Queue.itemlist.Count-1;

    }

    public void DrawDescription(){

        drawtext.text=description;

    }

}