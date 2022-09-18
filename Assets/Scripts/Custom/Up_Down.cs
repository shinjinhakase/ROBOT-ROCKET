using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Up_Down : MonoBehaviour{
    
    public bool up;
    public GameObject queue_obj;

    SpriteRenderer _renderer;
    
    void Start(){

        _renderer=GetComponent<SpriteRenderer>();
        _renderer.color=new Color32(255,255,255,255);

    }

    void Update(){

        if(Queue.itemlist.Count>=10){

            _renderer.color=new Color32(255,255,255,255);

        }else{

            _renderer.color=new Color32(255,255,255,0);
            
        }
        
    }

    public void Click(){

        if(!up){

            Vector3 move=queue_obj.transform.position;
            
            if(Queue.itemlist.Count>=10&&move.y<Queue.itemlist.Count-9){
                
                move.y+=1.0f;
                queue_obj.transform.position=move;

            }

        }else{

            Vector3 move=queue_obj.transform.position;
            
            if(Queue.itemlist.Count>=10&&move.y>0){

                
                move.y-=1.0f;
                queue_obj.transform.position=move;

            }

        }

    }

}