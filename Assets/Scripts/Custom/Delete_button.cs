using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete_button : MonoBehaviour{
    
    public static bool delete=false;
    
    void Start(){
    }

    void Update(){   
    }

    public void Click(){
        
        //末尾から選択中のアイテムまでに対して処理を実行
        for(int i=Queue.itemlist.Count-1;i>=Queue.nowActive;i--){

            Items _items=Queue.itemlist[i].GetComponent<Items>();
            Icon_inqueue _icon_inqueue=Queue.icon_list[i].GetComponent<Icon_inqueue>();

            if(_items.mynumber==Queue.nowActive){

                Queue.itemlist.RemoveAt(i);
                _items.Delete_items();

                Queue.icon_list.RemoveAt(i);
                _icon_inqueue.Delete_icon();

            }else if(_items.mynumber>Queue.nowActive){

                Vector2 nowposition=Queue.icon_list[i].transform.position;
                nowposition.y+=1.0f;
                Queue.icon_list[i].transform.position=nowposition;

                _items.mynumber--;
                _icon_inqueue.mynumber--;

            }

        }

    }

}
