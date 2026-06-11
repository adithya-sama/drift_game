using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class steering_animation_handler : MonoBehaviour
{

    public bool play_tut = false;
    public GameObject grapple_button;
    public Animator tut_rotation_animation_controller;
    public animation_handler anim_handler;

    void Start()
    {

        if(!play_tut || global_variables._disable_steering_animation){
            grapple_button.SetActive(true);
            anim_handler.animation_end(0);
            return;
        }

        tut_rotation_animation_controller.Play("transition_in");

        if(PlayerPrefs.GetInt("hand_orientation") == 1){
            tut_rotation_animation_controller.Play("steering_transition_in_right_hand_rotation");
        }else{
            tut_rotation_animation_controller.Play("steering_transition_in_left_hand_rotation");
        }

    }

}
