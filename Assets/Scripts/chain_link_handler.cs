using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chain_link_handler: MonoBehaviour
{
    public Vector2 init_pos;
    public float init_rotation;
    public Transform prev_chain_link_tf;
    public Vector2 prev_chain_link_anchor;
    LineRenderer line_renderer;
    HingeJoint2D hinge_joint;
    Rigidbody2D prev_chain_link_rb;
    void Start(){
        line_renderer = gameObject.GetComponent<LineRenderer>();
        if(prev_chain_link_tf != null){
            prev_chain_link_rb = prev_chain_link_tf.GetComponent<Rigidbody2D>();
            hinge_joint = gameObject.GetComponent<HingeJoint2D>();
            hinge_joint.connectedBody = prev_chain_link_rb;
            hinge_joint.connectedAnchor = prev_chain_link_anchor;
        }
    }
    void OnEnable(){
        transform.localPosition = init_pos;
        transform.localEulerAngles = new Vector3(0, 0, init_rotation);
    }
    void LateUpdate(){
        update_line_renderer();
    }
    void update_line_renderer(){
        line_renderer.SetPosition(0, transform.InverseTransformPoint(prev_chain_link_tf.TransformPoint(new Vector2(0, 1.5f))));
    }
    // void OnDrawGizmos(){
    //     if(prev_chain_link_tf)
    //         Gizmos.DrawLine(transform.position, transform.TransformPoint(transform.InverseTransformPoint(prev_chain_link_tf.TransformPoint(new Vector2(0, 1.5f)))));
    // }
}