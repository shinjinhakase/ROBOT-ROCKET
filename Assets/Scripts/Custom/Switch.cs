using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour{
    
    SpriteRenderer _renderer;

    public static bool selected=false;
    
    void Start(){

        _renderer=GetComponent<SpriteRenderer>();
        _renderer.color=new Color32(255,255,255,255);
    }

    void Update(){

        if(selected==false){

            _renderer.color=new Color32(255,255,255,255);

        }
        
    }

    public void Click(){

        if(Queue.itemlist.Count==0){

            return;

        }else{
            
            if(selected==false){
                
                selected=true;
                _renderer.color=new Color32(255,195,0,255);

            }else{

                selected=false;
                _renderer.color=new Color32(255,255,255,255);

            }
            
            
        }

    }

}