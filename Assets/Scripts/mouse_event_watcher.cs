using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class mouse_event_watcher : MonoBehaviour
{
    public float max_tap_duration, consequtive_tap_delay;
    public bool touch_controls;
    public static event Action<Vector2, Vector2> on_mouse_drag;
    public static event Action on_mouse_double_click;
    public static event Action on_mouse_down;
    public static event Action on_mouse_up;
    Vector2 prev_mouse_position, current_mouse_position;
    float touch_duration = 0, touch_gap_duration = 0, consequtive_tap_count = 0;
    bool dragged = false, is_camera_on_focus = false;
    Touch current_touch;
    Vector2 get_mouse_pos()
    {
        return touch_controls ? current_touch.position : (Vector2)Input.mousePosition;
    }
    void Update()
    {
        if(is_camera_on_focus){
            return;
        }
        if (touch_controls)
        {
            if (Input.touchCount > 0)
            {
                current_touch = Input.GetTouch(0);
                switch (current_touch.phase)
                {
                    case TouchPhase.Began:
                        dragged = false;
                        began();
                        break;
                    case TouchPhase.Moved:
                        moved();
                        dragged = true;
                        break;
                    case TouchPhase.Stationary:
                        stationary();
                        break;
                    case TouchPhase.Ended:
                        ended();
                        break;
                }
            }
            else
            {
                mouse_inactive();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                began();
            }
            else if (Input.GetMouseButton(0))
            {
                moved();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                ended();
            }
            else
            {
                mouse_inactive();
            }
        }
    }
    void began()
    {
        if(on_mouse_down != null)
            on_mouse_down();
        prev_mouse_position = get_mouse_pos();
        touch_duration = 0;
    }
    void moved()
    {
        current_mouse_position = get_mouse_pos();
        if(on_mouse_drag != null)
            on_mouse_drag(prev_mouse_position, current_mouse_position);
        prev_mouse_position = current_mouse_position;
        touch_duration += Time.deltaTime;
    }
    void stationary()
    {
        touch_duration += Time.deltaTime;
    }
    void ended()
    {
        if(on_mouse_up != null)
            on_mouse_up();
        // checking if it was a tap.
        if (touch_duration <= max_tap_duration && !dragged)
        {
            if (consequtive_tap_count == 1)
            {
                if(on_mouse_double_click != null)
                    on_mouse_double_click();
                consequtive_tap_count = 0;
            }
            else if (consequtive_tap_count == 0)
            {
                consequtive_tap_count = 1;
            }
        }
        touch_gap_duration = 0;
    }
    void mouse_inactive()
    {
        if (consequtive_tap_count != 0)
        {
            if (touch_gap_duration >= consequtive_tap_delay)
            {
                consequtive_tap_count = 0;
            }
            else
            {
                touch_gap_duration += Time.deltaTime;
            }
        }
    }
    public void stop_events(){
        ended();
        is_camera_on_focus = true;
    }
    public void start_events(){
        is_camera_on_focus = false;
    }
}