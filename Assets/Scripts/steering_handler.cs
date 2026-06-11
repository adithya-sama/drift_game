using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class steering_handler : MonoBehaviour{

    public Camera player_camera;
    public float rotation_sensitivity;
    public float idle_steering_reset_speed, finger_tracker_reset_speed, default_angle = 45;
    public float teleport_duration, reset_delay;
    public bool steering_enable;
    public Transform finger_tracker;
    public UnityEvent on_button_click, on_button_deactivate, on_grapple_success;

    bool grappled = false;
    float teleport_state, reset_delay_state;
    int min_rotation, max_rotation;
    Vector2 steering_position;
    Vector3 finger_tracker_rest_pos, rotate_to;
    bool right_handed, mouse_down = false;

    void Awake(){

        right_handed = PlayerPrefs.GetInt("hand_orientation") == 1;

        finger_tracker_rest_pos = finger_tracker.localPosition;

    }

    void Start(){

        // moving the steering to the corner of camera.
        if (right_handed){
            transform.position = player_camera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
            max_rotation = 90;
            min_rotation = 0;
            transform.eulerAngles = new Vector3(0,0,default_angle);
        }else{
            transform.position = player_camera.ScreenToWorldPoint(new Vector3(0, 0, 0));
            max_rotation = 0;
            min_rotation = -90;
            transform.eulerAngles = new Vector3(0, 0, -default_angle);
        }
        steering_position = player_camera.WorldToScreenPoint(transform.position);

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
            steering_enable &&
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

            if(finger_tracker.localPosition != finger_tracker_rest_pos){
                finger_tracker.localPosition = Vector3.MoveTowards(finger_tracker.localPosition, finger_tracker_rest_pos, finger_tracker_reset_speed);
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
        if(!steering_enable){ return;}
        if (!mouse_down) mouse_down = true;
        rotate_to.z = Vector2.SignedAngle(prev_mouse_pos - steering_position, current_mouse_pos - steering_position) * rotation_sensitivity;
        rotate_to.z = Utils.ToSignedRotation(transform.eulerAngles.z) + rotate_to.z;
        rotate_to.z = Mathf.Max(min_rotation, Mathf.Min(max_rotation, rotate_to.z));
        transform.eulerAngles = rotate_to;
        finger_tracker.position = player_camera.ScreenToWorldPoint(current_mouse_pos);
    }
    void on_mouse_up(){
        mouse_down = false;
        reset_delay_state = reset_delay;
    }
    void on_mouse_double_click(){
        teleport_state = teleport_duration;
    }

    public void button_click(){

        if(grappled){
            on_button_deactivate.Invoke();
            grappled = false;
        }else{
            on_button_click.Invoke();
        }

    }

    public void grapple_successful(){
        grappled = true;
    }

}
