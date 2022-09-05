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
        draw_icon.transform.position=new Vector2(8,Queue.draw_position);
        Queue.draw_position-=1.0f;

        Queue.testlist.Add(pop_item);
        Queue.icon_list.Add(draw_icon);

        Queue.nowActive=Queue.testlist.Count-1;

    }

}