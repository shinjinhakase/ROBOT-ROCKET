using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon : MonoBehaviour{
    
    public GameObject pop;
    public GameObject add_queue;

    GameObject queue_obj;

    public PartsPerformance.E_PartsID id;
    public PartsPerformanceData _data;
    private PartsPerformance _performance;

    public int tabnumber;

    public GameObject custom_panel;
    
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

        GameObject pop_item=Instantiate(pop) as GameObject;
        Vector3 pop_position=new Vector3(3.5f,0f,0f);
        pop_item.GetComponent<SpriteRenderer>().sprite=_performance.partsSprite;
        Items _items=pop_item.GetComponent<Items>();
        _items.mynumber=Queue.itemlist.Count;
        pop_item.transform.position=pop_position;
        pop_item.transform.parent=custom_panel.transform;
        

        GameObject draw_icon=Instantiate (add_queue) as GameObject;
        Icon_inqueue icon_script=draw_icon.GetComponent<Icon_inqueue>();
        icon_script.id=id;
        icon_script.mynumber=Queue.itemlist.Count;
        draw_icon.transform.parent=queue_obj.transform;
        draw_icon.GetComponent<SpriteRenderer>().sprite=_performance.iconSprite;

        if(Queue.icon_list.Count==0){

            Queue.draw_position=4.0f;
            
        }else{
            
            GameObject last_icon=Queue.icon_list[Queue.icon_list.Count-1];
            Vector2 last_position=last_icon.transform.position;
            Queue.draw_position=last_position.y-1.0f;

        }

        draw_icon.transform.position=new Vector2(7.6f,Queue.draw_position);

        Queue.itemlist.Add(pop_item);
        Queue.icon_list.Add(draw_icon);

        Queue.nowActive=Queue.itemlist.Count-1;

    }

}