using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class items : MonoBehaviour{

    //アクティブ状態かどうか
    private bool isActive=false;

    private int mynumber=0;
    private float myangle=0;

    private bool change_alpha=false;

    SpriteRenderer _renderer;
    
    void Start(){

        isActive=true;
        mynumber=queue.testlist.Count;

        _renderer=GetComponent<SpriteRenderer>();
        

    }

    void Update(){

        if(mynumber==queue.nowActive){
            
            if(isActive==false){

                customize.obj_angle=myangle;

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

            this.transform.rotation=Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, customize.obj_angle);
            _renderer.color=new Color32(255,255,255,255);
            myangle=customize.obj_angle;
            

        }else{

            if(change_alpha==false){

                _renderer.color=new Color32(255,255,255,50);
                change_alpha=true;

            }

        }

        
        
    }

}