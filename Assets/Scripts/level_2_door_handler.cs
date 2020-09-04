using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level_2_door_handler : MonoBehaviour, Ipullable
{
    public Transform key;
    public float pull_speed, retract_speed, retract_pos, open_pos;
    bool reject = true, open_key = false, retract = false;
    Vector2 to_pos;
    grapple_handle_handler gh_handler = null;
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
            gh_handler.forced_detach();
            gh_handler.become_ungrapplelable();
            level_2_handler.instance.start_next_sequence(2);
            reject = false;
            to_pos = transform.TransformPoint(new Vector2(0, retract_pos));
            retract = true;
        }else{
            key.position = Vector2.MoveTowards(key.position, (Vector2)key.position + Vector2.down, pull_speed * Time.fixedDeltaTime);
            if(transform.InverseTransformPoint(key.position).y <= open_pos){
                level_2_handler.instance.start_next_sequence(7);
                gh_handler.forced_detach();
                gh_handler.become_ungrapplelable();
            }
        }
    }
    public void set_grapple_handler(grapple_handle_handler handler)
    {
        gh_handler = handler;
    }
    public void give_passage(){
        to_pos = transform.TransformPoint(new Vector2(0, -retract_pos));
        open_key = true;
    }
    void Ipullable.grappled(){}
    void Ipullable.ungrappled(){}
}