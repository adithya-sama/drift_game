using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grapple_handle_handler : Grapplelable
{
    public float angle, distance;
    public GameObject pullable_obj;
    public bool tut = false;
    public GameObject pointer;
    object_pointer_handler pointer_handler;
    Animator pointer_animator;
    Ipullable pullable_handler;
    Transform grappler_transform;
    float current_angle, distance_sqrd;
    Vector2 vector2_tmp;
    bool open = false;
    void Awake()
    {
        pullable_handler = pullable_obj.GetComponent<Ipullable>();
        pullable_handler.set_grapple_handler(this);
        distance_sqrd = distance * distance;
        if(pointer){
            pointer_handler = pointer.GetComponent<object_pointer_handler>();
            pointer_animator = pointer.GetComponent<Animator>();
        }
    }
    void FixedUpdate()
    {
        if (gc_handler != null && gc_handler.engaged)
        {
            if (is_pulled())
            {
                pullable_handler.pull();
            }
        }
    }
    bool is_pulled()
    {
        vector2_tmp = grappler_transform.position - transform.position;
        if ((vector2_tmp.sqrMagnitude >= distance_sqrd) && Vector2.Angle(transform.up, vector2_tmp) <= angle)
            return true;
        return false;
    }
    public override void attach(grapple_chain_handler handler)
    {
        base.attach(handler);
        open = false;
        grappler_transform = handler.transform;
        pullable_handler.grappled();
        if(pointer){
            if(tut){
                pointer_handler.set_location((gameObject.transform.up * distance_sqrd) + gameObject.transform.position);
            }else{
                pointer_animator.Play("transition_out");
            }
        }
    }
    public override void reset()
    {
        base.reset();
        grappler_transform = null;
        pullable_handler.ungrappled();
        if(open){
            if(pointer.activeSelf){
                pointer_animator.Play("transition_out");
            }
        }else{
            if(pointer){
                if(tut){
                    pointer_handler.set_location(gameObject.transform.position);
                }else{
                    pointer.SetActive(true);
                    pointer_animator.Play("transition_in");
                }
            }
        }
    }
    public void is_open(){
        open = true;
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + (Quaternion.AngleAxis(angle, Vector3.forward) * transform.up * distance));
        Gizmos.DrawLine(transform.position, transform.position + (Quaternion.AngleAxis(-angle, Vector3.forward) * transform.up * distance));
    }
}