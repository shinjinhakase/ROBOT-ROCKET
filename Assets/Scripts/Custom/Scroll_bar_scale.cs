using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll_bar_scale : MonoBehaviour{
    
    SpriteRenderer _renderer;
    
    void Start(){

        _renderer=GetComponent<SpriteRenderer>();

    }

    void Update(){

        if(Queue.itemlist.Count<10){

            _renderer.color=new Color32(255,255,255,0);

        }else{

            _renderer.color=new Color32(255,255,255,50);

        }

    }

}