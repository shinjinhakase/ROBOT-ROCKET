using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll_bar : MonoBehaviour{
    
    private Vector3 screenPoint;
    
    public static float current_height;

    SpriteRenderer _renderer;
    
    void Start(){

        current_height=0;
        _renderer=GetComponent<SpriteRenderer>();

    }

    void Update(){

        if(Queue.testlist.Count<10){

            _renderer.color=new Color32(255,255,255,0);

        }else{

            _renderer.color=new Color32(255,255,255,255);

        }

    }

    void OnMouseDown(){

        this.screenPoint=Camera.main.WorldToScreenPoint(transform.position);

    }

    void OnMouseDrag(){

        Vector3 currentScreenPoint=new Vector3(screenPoint.x,Input.mousePosition.y,screenPoint.z);
        Vector3 currentPosition=Camera.main.ScreenToWorldPoint(currentScreenPoint);

        if(-1.85<currentPosition.y&&currentPosition.y<4){
            
            transform.position=currentPosition;
            current_height=(currentPosition.y+1.85f)/5.85f;

        }

    }

}