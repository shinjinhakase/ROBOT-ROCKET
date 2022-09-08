using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Tab : MonoBehaviour{
    
    private Rigidbody2D body;

    public GameObject tab1;
    public GameObject tab2;
    public GameObject tab3;

    GameObject[] tabs;
    
    void Start(){

        body=GetComponent<Rigidbody2D>();
        tabs=new GameObject[]{tab1,tab2,tab3};

    }

    void Update(){
    }

    public void Click_tab1(){

        draw(0);

    }

    public void Click_tab2(){

        draw(1);

    }

    public void Click_tab3(){

        draw(2);

    }

    void draw(int tab_select){

        for(int i=0;i<3;i++){

            if(i==tab_select){

                tabs[i].GetComponent<SortingGroup>().sortingOrder=1;

            }else{

                tabs[i].GetComponent<SortingGroup>().sortingOrder=0;
            }

        }

    }

}