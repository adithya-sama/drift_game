using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level_2_door_handler : MonoBehaviour, Ipullable
{
    public Transform key;
    public float pull_speed, retract_speed, retract_pos, idle_pos, open_pos;
    [SerializeField]
    public UltEvents.UltEvent on_reject, on_open;
    bool reject = true, open_key = false, retract = false;
    Vector2 to_pos;
    Vector3 open_pos_vector;
    grapple_handle_handler gh_handler = null;

    void Awake(){
        open_pos_vector = transform.TransformPoint(new Vector3(0, open_pos, 0));
    }

    void FixedUpdate(){
        if(retract){
            key.position = Vector2.MoveTowards(key.position, to_pos, retract_speed * Time.fixedDeltaTime);
            if((Vector2)key.position == to_pos){
                retract = false;
            }
        }else if(open_key){
            key.position = Vector2.MoveTowards(key.position, to_pos, retract_speed * Time.fixedDeltaTime);
            if((Vector2)key.position == to_pos){
                open_key = false;
                gh_handler.become_grapplelable();
            }
        }
    }
    public void pull()
    {
        if(reject){
            gh_handler.become_ungrapplelable();
            gh_handler.is_open();
            gh_handler.forced_detach();
            reject = false;
            to_pos = transform.TransformPoint(new Vector2(0, retract_pos));
            retract = true;
            on_reject.Invoke();
        }else{
            key.position = Vector2.MoveTowards(key.position, open_pos_vector, pull_speed * Time.fixedDeltaTime);
            if(key.position == open_pos_vector){
                gh_handler.become_ungrapplelable();
                gh_handler.is_open();
                gh_handler.forced_detach();
                on_open.Invoke();
            }
        }
    }
    public void set_grapple_handler(grapple_handle_handler handler)
    {
        gh_handler = handler;
    }
    public void give_passage(){
        to_pos = transform.TransformPoint(new Vector2(0, idle_pos));
        open_key = true;
    }
    void Ipullable.grappled(){}
    void Ipullable.ungrappled(){}
}