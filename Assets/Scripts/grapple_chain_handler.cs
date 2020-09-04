using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grapple_chain_handler : MonoBehaviour
{
    public hook_handler hook;
    public GameObject grapple_hook_wrapper, grappled_obj, debug_obj;
    public float shoot_dist, raycast_circle_radius;
    public Vector2 raycast_origin;
    public bool engaged = false;
    Transform chain_tf;
    RaycastHit2D raycast_hit;
    Grapplelable grapplelable_obj;
    LayerMask layer_mask;
    bool active;
    void Start()
    {
        active = grapple_hook_wrapper.activeSelf;
        layer_mask = LayerMask.GetMask("Default");
    }
    void OnEnable()
    {
        mouse_event_watcher.on_mouse_double_click += shoot;
    }
    void OnDisable()
    {
        mouse_event_watcher.on_mouse_double_click -= shoot;
        release();
    }
    public void release()
    {
        grapple_hook_wrapper.SetActive(false);
        active = false;
        engaged = false;
        if (grapplelable_obj != null)
        {
            grapplelable_obj.reset();
        }
    }
    void shoot()
    {
        if (active)
        {
            release();
        }
        else
        {
            if (debug_obj != null)
                debug_obj.transform.position = transform.position + (transform.up * shoot_dist);
            raycast_hit = Physics2D.CircleCast(transform.TransformPoint(raycast_origin), raycast_circle_radius, transform.up, shoot_dist, layer_mask);
            if (raycast_hit.collider != null)
            {
                grappled_obj = raycast_hit.collider.gameObject;
                grapplelable_obj = raycast_hit.collider.gameObject.GetComponent<Grapplelable>();
                if (grapplelable_obj != null)
                {
                    grapplelable_obj.attach(this);
                }
                hook.set_hook_position(raycast_hit);
                grapple_hook_wrapper.SetActive(true);
                active = true;
                engaged = true;
            }
        }
    }
}