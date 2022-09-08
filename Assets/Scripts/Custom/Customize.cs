using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customize : MonoBehaviour{

    //マウスの位置
    Vector2 mouse_position;

    //オブジェクトの角度
    public static float obj_angle;

    void Start(){
        
        mouse_position=new Vector2(0,0);

    }

    void Update(){

        //Debug.Log(delete_button.delete);

    }

    void OnMouseDrag(){
        
        Vector2 getMouseposition=new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        mouse_position=Camera.main.ScreenToWorldPoint(getMouseposition);
        obj_angle=GetAngle(new Vector2(3.5f,0f),mouse_position);

    }

    float GetAngle(Vector2 start,Vector2 target){

        Vector2 dt=target-start;
        float rad=Mathf.Atan2(dt.y,dt.x);
        float degree=rad*Mathf.Rad2Deg;

        return degree;

    }

}