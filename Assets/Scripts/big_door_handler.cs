using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class big_door_handler : MonoBehaviour
{
    public Vector2 open_pos;
    public float open_rotation, move_speed, rot_speed, direction;
    public UnityEvent on_open;
    bool open = false;
    float float_tmp;
    public void open_door(){
        open = true;
    }
    void FixedUpdate(){
        if(open){
            if(((Vector2)transform.position != open_pos) || (transform.localEulerAngles.z != open_rotation)){
                float_tmp = open_rotation - transform.localEulerAngles.z;
                transform.position = Vector2.MoveTowards(transform.position, open_pos, move_speed * Time.fixedDeltaTime);
                transform.Rotate(Vector3.forward, direction * Mathf.Min(rot_speed * Time.fixedDeltaTime, Mathf.Abs(float_tmp)));
            }else{
                open = false;
                on_open.Invoke();
            }
        }
    }
}