using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class icon : MonoBehaviour{
    
    public GameObject pop;
    public GameObject add_queue;

    GameObject queue_obj;
    
    void Start(){

        queue_obj=GameObject.Find("queue");

    }

    void Update(){
    }

    public void Click(){
        
        if(queue.nowActive==queue.testlist.Count){

            queue.nowActive++;
            
        }
        
        GameObject pop_item=Instantiate(pop) as GameObject;
        Vector3 pop_position=new Vector3(3.5f,0f,0f);
        pop_item.transform.position=pop_position;
        queue.testlist.Add(pop_item);

        GameObject draw_icon=Instantiate (add_queue) as GameObject;
        draw_icon.transform.parent=queue_obj.transform;
        draw_icon.transform.position=new Vector2(8,queue.draw_position);
        queue.draw_position-=1.0f;

        

    }

}