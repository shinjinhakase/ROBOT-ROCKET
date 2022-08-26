using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class customize : MonoBehaviour{
    
    //テスト用テキスト
    public Text x_coordinate_text;
    public Text y_coordinate_text;

    //マウスカーソルの位置
    Vector2 mouse_position;

    void Start(){
        
        mouse_position=new Vector2(0,0);

    }

    void Update(){
        
        //テスト表示用
        x_coordinate_text.text="x:"+mouse_position.x.ToString();
        y_coordinate_text.text="y:"+mouse_position.y.ToString();

    }

    void OnMouseDrag(){
        
        Vector2 getMouseposition=new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        mouse_position=Camera.main.ScreenToWorldPoint(getMouseposition);

    }

}