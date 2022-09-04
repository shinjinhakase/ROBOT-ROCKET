using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class items : MonoBehaviour{

    //アクティブ状態かどうか
    private bool isActive=false;

    private int mynumber=0;

    private bool change_alpha=false;

    SpriteRenderer _renderer;
    
    void Start(){

        isActive=true;
        mynumber=queue.nowActive;

        _renderer=GetComponent<SpriteRenderer>();
        

    }

    void Update(){

        if(isActive){

            this.transform.rotation=Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, customize.obj_angle);
            _renderer.color=new Color32(255,255,255,255);

        }else{

            if(change_alpha==false){

                _renderer.color=new Color32(255,255,255,100);
                change_alpha=true;

            }

        }

        if(mynumber==queue.nowActive){

            isActive=true;
            change_alpha=false;

        }else{

            isActive=false;

        }
        
    }

}