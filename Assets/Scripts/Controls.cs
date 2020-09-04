using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Controls : MonoBehaviour{
    public float max_steering_scale, min_steering_scale;
    public float scale_sensitivity, rotation_sensitivity;
    public bool right_handed = true;
    public float idle_steering_reset_speed, default_angle = 45;
    float min_rotation, max_rotation;
    Vector3 mouse_down_position_scale, mouse_down_position_rotation;
    Vector3 steering_scale_from;
    float steering_angle_from;
    bool first_touch = true;
    Vector3 steering_position;
    public Transform steering;
    public float getSteeringAngle(){
        float angle = steering.eulerAngles.z;
        angle = (angle > 180) ? (angle - 360) : angle;
        if(right_handed){
            return angle - 45;
        }else{
            return angle + 45;
        }
    }
    public float getSteeringScale(){
        return steering.localScale.x;
    }
    public float ToSignedRotation(float angle){
        return (angle > 180) ? (angle - 360) : angle;
    }
    private void Awake(){
        // moving the controller to corners of the camera.
        if (right_handed)
        {
            steering.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
            min_rotation = 90;
            max_rotation = 0;
            steering.eulerAngles = new Vector3(0,0,default_angle);
        }else{
            steering.position = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
            min_rotation = 0;
            max_rotation = -90;
            steering.eulerAngles = new Vector3(0, 0, -default_angle);
        }
        steering_position = Camera.main.WorldToScreenPoint(steering.position);
    }
    void Update () {
        if(Input.GetMouseButton(0)){
            if (first_touch)
            {
                mouse_down_position_scale = Input.mousePosition;
                mouse_down_position_rotation = Input.mousePosition;
                steering_scale_from = steering.localScale;
                steering_angle_from = ToSignedRotation(steering.eulerAngles.z);
                first_touch = false;
            }
            else
            {
                Vector3 current_mouse_position = Input.mousePosition;
                Debug.Log("controls " + current_mouse_position);

                //Scaling the steering
                Vector3 current_steering_scale = steering.localScale;
                Vector3 steering_scale_to = Vector3.zero;
                float delta_distance = Vector3.Distance(steering_position, current_mouse_position) - Vector3.Distance(steering_position, mouse_down_position_scale);

                if (((current_steering_scale.x >= max_steering_scale) && (delta_distance > 0)) ||
                    ((current_steering_scale.x <= min_steering_scale) && (delta_distance < 0)))
                {
                    steering_scale_from = steering.localScale;
                    mouse_down_position_scale = current_mouse_position;
                }
                else
                {
                    float tmp_scale_to = Mathf.Max(min_steering_scale, Mathf.Min(max_steering_scale, steering_scale_from.x + (scale_sensitivity * delta_distance)));
                    //tmp_scale_to = tmp_scale_to - (tmp_scale_to % scale_step_size);
                    steering_scale_to.x = tmp_scale_to; 
                    steering_scale_to.y = tmp_scale_to;
                    steering.localScale = steering_scale_to;
                }

                //Rotating the steering

                float steering_angle_to = Vector2.SignedAngle(mouse_down_position_rotation - steering_position, current_mouse_position - steering_position) * rotation_sensitivity;

                Vector3 tmp = steering.eulerAngles;
                tmp.z = steering_angle_from + steering_angle_to;

                if(tmp.z > min_rotation){
                    tmp.z = min_rotation;
                }else if(tmp.z < max_rotation){
                    tmp.z = max_rotation;
                }

                steering.eulerAngles = tmp;

                float current_steering_rotation = ToSignedRotation(steering.eulerAngles.z);

                if (((current_steering_rotation >= min_rotation) && (steering_angle_to > 0)) ||
                    ((current_steering_rotation <= (max_rotation)) && (steering_angle_to < 0))
                    )
                {
                    steering_angle_from = ToSignedRotation(steering.eulerAngles.z);
                    mouse_down_position_rotation = current_mouse_position;
                }
            }
        }
        else{
            if (!first_touch)
            {
                first_touch = true;
            }

            // Slowly reseting the steering to center 
            Vector3 tmp = steering.eulerAngles;
            tmp.z = ToSignedRotation(tmp.z);
            tmp.z = tmp.z + (((max_rotation + ((min_rotation - max_rotation)/2)) - tmp.z) * idle_steering_reset_speed);
            steering.eulerAngles = tmp;
        }
	}
}
