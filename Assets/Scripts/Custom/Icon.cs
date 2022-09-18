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
    public string description;
    
    void Start(){

        queue_obj=null;
        _performance=_data.getData(id);
        SpriteRenderer _renderer=this.GetComponent<SpriteRenderer>();
        _renderer.sprite=_performance.iconSprite;

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

        GameObject pop_item=Instantiate(pop, custom_panel.transform) as GameObject;
        Vector3 pop_position=new Vector3(3.5f,0f,0f);
        pop_item.GetComponent<SpriteRenderer>().sprite=_performance.partsSprite;
        Items _items=pop_item.GetComponent<Items>();
        _items.mynumber=Queue.itemlist.Count;
        pop_item.transform.localPosition=pop_position;

        GameObject draw_icon=Instantiate (add_queue, queue_obj.transform) as GameObject;
        Icon_inqueue icon_script=draw_icon.GetComponent<Icon_inqueue>();
        icon_script.id=id;
        icon_script.mynumber=Queue.itemlist.Count;
        draw_icon.GetComponent<SpriteRenderer>().sprite=_performance.iconSprite;

        if(Queue.icon_list.Count==0){

            Queue.draw_position=4.0f;
            
        }else{
            
            GameObject last_icon=Queue.icon_list[Queue.icon_list.Count-1];
            Vector2 last_position=last_icon.transform.localPosition;
            Queue.draw_position=last_position.y-=1.0f;

        }

        draw_icon.transform.localPosition=new Vector2(7.6f,Queue.draw_position);

        Queue.itemlist.Add(pop_item);
        Queue.icon_list.Add(draw_icon);

        Queue.nowActive=Queue.itemlist.Count-1;

    }

    public void DrawDescription(){

        drawtext.text=description;

    }

}