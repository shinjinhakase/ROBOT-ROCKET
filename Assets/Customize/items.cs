using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour{

    //アクティブ状態かどうか
    private bool isActive=false;

    public int mynumber=0;
    private float myangle=0;

    private bool change_alpha=false;
    SpriteRenderer _renderer;
    
    void Awake(){

        isActive=true;
        _renderer=GetComponent<SpriteRenderer>();

    }

    void Update(){

        if(mynumber==Queue.nowActive){
            
            if(isActive==false){

                Customize.obj_angle=myangle;

            }
            
            isActive=true;
            change_alpha=false;

        }else{

            if(isActive==true){

                this.transform.rotation=Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, myangle);

            }
            
            isActive=false;

        }
        
        if(isActive){

            this.transform.rotation=Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Customize.obj_angle);
            _renderer.color=new Color32(255,255,255,255);
            myangle=Customize.obj_angle;
            

        }else{

            if(change_alpha==false){

                _renderer.color=new Color32(255,255,255,50);
                change_alpha=true;

            }

        }
        
    }

    public void Delete_items(){
        
        Destroy(this.gameObject);

    }

}