using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class icon : MonoBehaviour{
    
    public GameObject pop;
    
    void Start(){
    }

    void Update(){
    }

    public void Click(){

        GameObject pop_item=Instantiate(pop) as GameObject;
        Vector3 pop_position=new Vector3(3.5f,0f,0f);
        pop_item.transform.position=pop_position;

    }

}