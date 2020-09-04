using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheel_cover : MonoBehaviour
{
    private steering_handler steering_handler_i;
    private Rigidbody2D rigid_body;
    public bool rotate;
    public int rotation_angle;
    // Start is called before the first frame update
    void Start()
    {
        steering_handler_i = GameObject.Find("steering").GetComponent<steering_handler>();
        rigid_body = gameObject.GetComponent<Rigidbody2D>();
    }
    void set_angle(float angle){
        angle = Utils.curve_map_linear(-45, 45, -rotation_angle, rotation_angle, angle, 1);
        Vector3 tmp = transform.localEulerAngles;
        tmp.z = angle;
        transform.localEulerAngles = tmp;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(rotate){
            set_angle(steering_handler_i.getSteeringAngle());
        }
    }
}
