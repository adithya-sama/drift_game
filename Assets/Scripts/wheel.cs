using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheel : MonoBehaviour{
    public bool debug;
    public bool rotate, power;
    public int rotation_angle, acceleration, max_speed;
    public float min_drag, max_drag;
    float current_drag, friction_force, x_vel_mag, x_vel_sign, max_speed_sqrd;
    steering_handler steering_handler_i;
    Rigidbody2D rigid_body;
    Vector2 current_vel;
    bool mouse_down = false;
    void Start(){
        steering_handler_i = GameObject.Find("steering").GetComponent<steering_handler>();
        rigid_body = gameObject.GetComponent<Rigidbody2D>();
        max_speed_sqrd = max_speed * max_speed;
    }
    void OnEnable()
    {
        mouse_event_watcher.on_mouse_down += on_mouse_down;
        mouse_event_watcher.on_mouse_up += on_mouse_up;
    }
    void OnDisable()
    {
        mouse_event_watcher.on_mouse_down -= on_mouse_down;
        mouse_event_watcher.on_mouse_up -= on_mouse_up;
    }
    void set_angle(float angle){
        //angle = Utils.curve_map_linear(-45, 45, -rotation_angle, rotation_angle, angle, 1);
        angle = Mathf.LerpAngle(-rotation_angle, rotation_angle, (angle + 45) / 90);
        Vector3 tmp = transform.localEulerAngles;
        tmp.z = angle;
        transform.localEulerAngles = tmp;
    }
    void accelerate(float acceleration){
        if(current_vel.sqrMagnitude < max_speed_sqrd && mouse_down){
            rigid_body.AddRelativeForce(Vector2.up * acceleration, ForceMode2D.Force);
        }
    }
    void apply_sideways_friction(){
        x_vel_mag = Mathf.Abs(current_vel.x);
        x_vel_sign = Mathf.Sign(current_vel.x);
        current_drag = max_drag;
        friction_force = (x_vel_mag <= current_drag) ? x_vel_mag : current_drag;
        friction_force = -x_vel_sign * friction_force; 
        rigid_body.AddRelativeForce(Vector3.right * friction_force, ForceMode2D.Impulse);
    }
    public void reset(){}
    // Update is called once per frame
    void FixedUpdate(){
        current_vel = transform.InverseTransformVector(rigid_body.velocity);
        if(rotate){
            set_angle(steering_handler_i.getSteeringAngle());
        }
        if(power){
            accelerate(acceleration);
        }
        apply_sideways_friction();
    }
    void on_mouse_down(){
        mouse_down = true;
    } 
    void on_mouse_up(){
        mouse_down = false;
    } 
    void print_with_label(string msg){
        if(!debug) return;
        if(gameObject.name == "front_wheel"){
            Debug.Log("^^^^^^^^^^" + msg);
        }else if(gameObject.name == "rear_wheel"){
            Debug.Log("************" + msg);
        }
    }
}