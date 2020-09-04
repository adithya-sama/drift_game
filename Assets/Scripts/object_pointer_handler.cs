using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class object_pointer_handler : MonoBehaviour
{
    public Transform obj_transform;
    public UnityEvent on_animation_end_event;
    float min_dist, l_dist, angle;
    Transform closest_enemy;
    Vector2 target_direction;
    void Start(){
        temp();
    }
    void temp(){
        if(on_animation_end_event != null){
            on_animation_end_event.Invoke();
        }
    }
    void FixedUpdate(){
        if(obj_transform && obj_transform.gameObject.activeSelf){
            target_direction = obj_transform.position - transform.position;
            angle = Vector2.SignedAngle(Vector2.up, target_direction);
            transform.eulerAngles = new Vector3(0, 0, angle);
        }else{
            Destroy(gameObject);
        }
    }
}