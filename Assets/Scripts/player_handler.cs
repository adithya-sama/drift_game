using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_handler : MonoBehaviour
{

    public string current_color;
    public Animator intro_animation_controller;
    public animation_handler anim_handler;

    void Start() {
        if(global_variables._disable_vehicle_animation){
            anim_handler.animation_end(0);
            return;
        }
        intro_animation_controller.Play("transition_in");
    }

    public void set_tyre_color(string color){
        current_color = color;
    }


}
