using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class items : MonoBehaviour{

    void Start(){

    }

    void Update(){

        this.transform.rotation=Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, customize.obj_angle);

    }

}