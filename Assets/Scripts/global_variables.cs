using UnityEngine;

public class global_variables : MonoBehaviour
{
    public bool disable_dialog = false, disable_steering_animation = false, disable_vehicle_animation = false;
    public static bool _disable_dialog = false, _disable_steering_animation = false, _disable_vehicle_animation = false;

    void Awake(){
        global_variables._disable_dialog = disable_dialog;
        global_variables._disable_steering_animation = disable_steering_animation;
        global_variables._disable_vehicle_animation = disable_vehicle_animation;
        }

}
