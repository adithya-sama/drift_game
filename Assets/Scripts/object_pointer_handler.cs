using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class object_pointer_handler : MonoBehaviour
{
    public Transform point_to;
    public Vector3 absolute_point;
    float angle;
    Vector2 target_direction;

    void FixedUpdate(){
        if(point_to != null){
            target_direction = point_to.position - transform.position;
        }else{
            target_direction = absolute_point - transform.position;
        }
        angle = Vector2.SignedAngle(Vector2.up, target_direction);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    public void set_location(Vector3 location){
        point_to = null;
        absolute_point = location;
    }

}