using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tab : MonoBehaviour{
    
    private Rigidbody2D body;

    public SpriteRenderer tab1;
    public SpriteRenderer tab2;
    public SpriteRenderer tab3;
    
    void Start(){

        body=GetComponent<Rigidbody2D>();

    }

    void Update(){
    }

    public void Click_tab1(){

        Debug.Log("tab1");
        tab1.sortingOrder=1;
        tab2.sortingOrder=0;
        tab3.sortingOrder=0;

    }

    public void Click_tab2(){

        Debug.Log("tab2");
        tab1.sortingOrder=0;
        tab2.sortingOrder=1;
        tab3.sortingOrder=0;

    }

    public void Click_tab3(){

        Debug.Log("tab3");
        tab1.sortingOrder=0;
        tab2.sortingOrder=0;
        tab3.sortingOrder=1;

    }

}