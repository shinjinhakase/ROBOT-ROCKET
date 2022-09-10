using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Custom_button : MonoBehaviour{

    public GameObject custom_panel;
    public static bool panel_on=false;
    SpriteRenderer _renderer;

    void Start(){

        _renderer=this.GetComponent<SpriteRenderer>();

    }

    void Update(){

        if(panel_on==false){

            _renderer.color=new Color32(255,255,255,255);
            this.gameObject.GetComponent<BoxCollider2D>().enabled=true;

        }else{

            _renderer.color=new Color32(255,255,255,0);
            this.gameObject.GetComponent<BoxCollider2D>().enabled=false;

        }

    }

    public void Click(){

        panel_on=true;
        GameObject pop_panel=Instantiate(custom_panel) as GameObject;

    }

}