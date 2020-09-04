using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hook_handler : MonoBehaviour
{
    public Transform chain_link;
    public HingeJoint2D hook_joint;
    public LineRenderer line_renderer;
    void LateUpdate()
    {
        line_renderer.SetPosition(0, transform.InverseTransformPoint(chain_link.TransformPoint(new Vector2(0, 1.5f))));
    }
    public void set_hook_position(RaycastHit2D hit){
        hook_joint.connectedBody = hit.collider.GetComponent<Rigidbody2D>();
        transform.position = hit.point;
        if(hook_joint.connectedBody != null){
            hook_joint.connectedAnchor = hit.collider.transform.InverseTransformPoint(hit.point);
        }else{
            hook_joint.connectedAnchor = hit.point;
        }
    }
}