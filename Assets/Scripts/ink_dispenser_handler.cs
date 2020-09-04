using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ink_dispenser_handler : MonoBehaviour, Ipullable
{
    public UnityEvent on_open; 
    public Transform handle_bar;
    public float pull_speed, retract_speed, open_distance;
    bool retract_handle = false;
    Vector3 init_pos, open_pos;
    grapple_handle_handler gh_handler = null;
    void Start(){
        init_pos = handle_bar.position;
        open_pos = handle_bar.position + (handle_bar.up * open_distance);
    }
    void FixedUpdate(){
        if(retract_handle){
            handle_bar.position = Vector2.MoveTowards(handle_bar.position, init_pos, retract_speed * Time.fixedDeltaTime);
            if(handle_bar.position == init_pos){
                retract_handle = false;
                gh_handler.become_grapplelable();
            }
        }
    }
    public void pull()
    {
        handle_bar.position = Vector2.MoveTowards(handle_bar.position, open_pos, pull_speed * Time.fixedDeltaTime);
        if(open_pos == handle_bar.position){
            open();
        }
    }
    public void set_grapple_handler(grapple_handle_handler handler)
    {
        gh_handler = handler;
    }
    void open(){
        gh_handler.become_ungrapplelable();
        gh_handler.forced_detach();
        retract_handle = true;
        if(on_open != null){
            on_open.Invoke();
        }
    }
    void Ipullable.grappled(){}
    void Ipullable.ungrappled(){}
}