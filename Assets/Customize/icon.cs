using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon : MonoBehaviour{
    
    public GameObject pop;
    public GameObject add_queue;

    GameObject queue_obj;
    
    void Start(){

        queue_obj=GameObject.Find("queue");

    }

    void Update(){
    }

    public void Click(){

        GameObject pop_item=Instantiate(pop) as GameObject;
        Vector3 pop_position=new Vector3(3.5f,0f,0f);
        Items _items=pop_item.GetComponent<Items>();
        _items.mynumber=Queue.testlist.Count;
        pop_item.transform.position=pop_position;
        

        GameObject draw_icon=Instantiate (add_queue) as GameObject;
        draw_icon.transform.parent=queue_obj.transform;

        if(Queue.icon_list.Count==0){

            Queue.draw_position=4.0f;
            
        }else{
            
            GameObject last_icon=Queue.icon_list[Queue.icon_list.Count-1];
            Vector2 last_position=last_icon.transform.position;
            Queue.draw_position=last_position.y-=1.0f;

        }

        draw_icon.transform.position=new Vector2(7.6f,Queue.draw_position);

        Queue.testlist.Add(pop_item);
        Queue.icon_list.Add(draw_icon);

        Queue.nowActive=Queue.testlist.Count-1;

    }

}