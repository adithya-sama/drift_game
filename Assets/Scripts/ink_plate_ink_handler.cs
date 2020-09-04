using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ink_plate_ink_handler : MonoBehaviour
{
    public level_1_handler.tomb_color ink_color;
    float time_watcher = 2;
    void FixedUpdate(){
        time_watcher -= Time.fixedDeltaTime;
        if(time_watcher < 0){
            on_animation_end();
            this.enabled = false;
        }
    }
    void on_animation_end(){
        level_1_handler.instance.ink_plate_animation_end();
    }
    void OnTriggerEnter2D(Collider2D collider){
        if(collider.CompareTag("Player")){
            level_1_handler.instance.set_current_ink(ink_color);
        }
    }
}
