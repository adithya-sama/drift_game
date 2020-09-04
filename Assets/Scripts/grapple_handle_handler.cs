using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grapple_handle_handler : Grapplelable
{
    public float angle, distance;
    public GameObject pullable_obj;
    Ipullable pullable_handler;
    Transform grappler_transform;
    float current_angle, distance_sqrd;
    Vector2 vector2_tmp;
    void Awake()
    {
        pullable_handler = pullable_obj.GetComponent<Ipullable>();
        pullable_handler.set_grapple_handler(this);
        distance_sqrd = distance * distance;
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
        grappler_transform = handler.transform;
        pullable_handler.grappled();
    }
    public override void reset()
    {
        base.reset();
        grappler_transform = null;
        pullable_handler.ungrappled();
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + (Quaternion.AngleAxis(angle, Vector3.forward) * transform.up * distance));
        Gizmos.DrawLine(transform.position, transform.position + (Quaternion.AngleAxis(-angle, Vector3.forward) * transform.up * distance));
    }
}