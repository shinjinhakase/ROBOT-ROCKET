using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll_bar : MonoBehaviour{
    
    public static bool reset_flag=false;

    public static float current_height;

    private Vector3 screenPoint;

    SpriteRenderer _renderer;
    
    void Start(){

        current_height=0;
        _renderer=GetComponent<SpriteRenderer>();

    }

    void Update(){

        if(Queue.itemlist.Count<10){

            _renderer.color=new Color32(255,255,255,0);

        }else{

            _renderer.color=new Color32(255,255,255,255);

        }

        if(reset_flag){

            Reset();
            reset_flag=false;

        }

    }

    void OnMouseDown(){

        this.screenPoint=Camera.main.WorldToScreenPoint(transform.position);

    }

    void OnMouseDrag(){

        var high = transform.parent.TransformPoint(Vector3.up * 4);
        var under = transform.parent.TransformPoint(Vector3.down * 1.85f);
        Vector3 currentScreenPoint=new Vector3(screenPoint.x,Input.mousePosition.y,screenPoint.z);
        Vector3 currentPosition=transform.position;


        if(under.y<currentPosition.y&&currentPosition.y<high.y){

            transform.position = currentPosition;
            currentPosition = transform.parent.InverseTransformPoint(currentPosition);
            current_height=(currentPosition.y+1.85f)/5.85f;
        }

    }

    void Reset(){

        Vector3 currentPosition=this.transform.position;
        currentPosition.y=4.0f;
        this.transform.position=currentPosition;

    }

}