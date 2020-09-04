using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class steering_handler : MonoBehaviour{
    public float rotation_sensitivity;
    public float idle_steering_reset_speed, default_angle = 45;
    public float teleport_duration, reset_delay;
    public bool right_handed = true, steering_enable = true;
    public GameObject[] accessories;
    float teleport_state, reset_delay_state;
    int min_rotation, max_rotation, current_accessory = 0;
    Vector2 steering_position;
    Vector3 rotate_to;
    bool mouse_down = false;
    private void Awake(){
        // moving the steering to the corner of camera.
        if (right_handed){
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
            max_rotation = 90;
            min_rotation = 0;
            transform.eulerAngles = new Vector3(0,0,default_angle);
        }else{
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
            max_rotation = 0;
            min_rotation = -90;
            transform.eulerAngles = new Vector3(0, 0, -default_angle);
        }
        steering_position = Camera.main.WorldToScreenPoint(transform.position);
        for(int i=0; i < accessories.Length; i++){
            if(accessories[i].activeSelf){
                current_accessory = i;
                break;
            }
        }
    }
    public void enable_steering(){
        steering_enable = true;
    }
    public void disable_steering(){
        steering_enable = false;
    }
    void OnEnable()
    {
        mouse_event_watcher.on_mouse_drag += on_mouse_drag;
        mouse_event_watcher.on_mouse_up += on_mouse_up;
    }
    void OnDisable()
    {
        mouse_event_watcher.on_mouse_drag -= on_mouse_drag;
        mouse_event_watcher.on_mouse_up -= on_mouse_up;
    }
    void FixedUpdate(){
        if(teleport_state > 0){
            teleport_state -= Time.fixedDeltaTime; 
        }else{
            if(reset_delay_state > 0){
                reset_delay_state -= Time.fixedDeltaTime;
            }
        }
        if(
            !mouse_down &&
            teleport_state <= 0 &&
            reset_delay_state <= 0
        ){
            // Slowly reseting the steering to center 
            Vector3 tmp = transform.eulerAngles;
            if(Mathf.Abs(tmp.z) != 45){
                tmp.z = Utils.ToSignedRotation(tmp.z);
                tmp.z += ((max_rotation - 45 - tmp.z) * idle_steering_reset_speed);
                transform.eulerAngles = tmp;
            }
        }
    }
    public float getSteeringAngle(){
        if(!steering_enable){ return 0;}
        float angle = transform.eulerAngles.z;
        angle = (angle > 180) ? (angle - 360) : angle;
        if(right_handed){
            return angle - 45;
        }else{
            return angle + 45;
        }
    }
    void on_mouse_drag(Vector2 prev_mouse_pos, Vector2 current_mouse_pos){
        if (!mouse_down) mouse_down = true;
        rotate_to.z = Vector2.SignedAngle(prev_mouse_pos - steering_position, current_mouse_pos - steering_position) * rotation_sensitivity;
        rotate_to.z = Utils.ToSignedRotation(transform.eulerAngles.z) + rotate_to.z;
        rotate_to.z = Mathf.Max(min_rotation, Mathf.Min(max_rotation, rotate_to.z));
        transform.eulerAngles = rotate_to;
    }
    void on_mouse_up(){
        mouse_down = false;
        reset_delay_state = reset_delay;
    }
    void on_mouse_double_click(){
        teleport_state = teleport_duration;
    }
    public void on_button_click(){
        current_accessory = (++current_accessory) % accessories.Length;
        for(int i=0; i < accessories.Length; i++){
            accessories[i].SetActive(false);
        }
        accessories[current_accessory].SetActive(true);
    }
}
