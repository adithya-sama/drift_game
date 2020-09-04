using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vehicle_shadow_handler : MonoBehaviour
{
    public Transform light_source_transform;
    public Vector3 light_source_pos;
    public float shadow_distance;
    public bool position;
    SpriteRenderer sprite_renderer;
    Vector2 shadow_direction;
    void Start(){
        sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
    }
    void FixedUpdate()
    {
        if(position){
            shadow_direction = (transform.parent.position - light_source_pos).normalized;
        }else{
            if(!light_source_transform){
                sprite_renderer.enabled = false;
            }else{
                sprite_renderer.enabled = true;
                shadow_direction = (transform.parent.position - light_source_transform.position).normalized;
            }
        }
       if(sprite_renderer.enabled){ 
            shadow_direction *= shadow_distance;
            transform.position = shadow_direction + (Vector2)transform.parent.position;
       }
    }
}
