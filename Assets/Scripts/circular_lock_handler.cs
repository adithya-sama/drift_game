using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class circular_lock_handler : MonoBehaviour, Ipullable
{
    public int max_angle, final_angle, max_rot_speed, auto_rot_speed;
    public GameObject handle;
    public UnityEvent on_close;
    grapple_handle_handler gh_handler;
    bool auto_rotate = false, done = false;
    public void activate(){
        handle.SetActive(true);
    }
    void FixedUpdate()
    {
        if(!done && auto_rotate){
            transform.Rotate(Vector3.forward, auto_rot_speed * Time.fixedDeltaTime);
            if(transform.localEulerAngles.z >= final_angle){
                transform.localEulerAngles = new Vector3(0, 0, final_angle);
                done = true;
                on_close.Invoke();
            }
        }
    }
    public void pull()
    {
        transform.Rotate(Vector3.forward, max_rot_speed * Time.fixedDeltaTime);
        if (transform.localEulerAngles.z > max_angle)
        {
            gh_handler.become_ungrapplelable();
            gh_handler.forced_detach();
            auto_rotate = true;
        }
    }
    public void set_grapple_handler(grapple_handle_handler handler)
    {
        gh_handler = handler;
    }
    public void grappled()
    {
    }
    public void ungrappled()
    {
    }
}
