using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chasis : MonoBehaviour
{
    public bool reset;
    public int graph_scale;
    public Rigidbody2D[] rbs, rbv;
    public TrailRenderer[] tr;
    public wheel[] wheel_i;
    int reset_flow;
    steering_handler steering_handler_i;
    Rigidbody2D rigid_body;
    void Start(){
        //debug_instanstiate();
        steering_handler_i = GameObject.Find("steering").GetComponent<steering_handler>();
        rigid_body = gameObject.GetComponent<Rigidbody2D>();
    }
    // Debug stuff
    void reset_rb(Rigidbody2D rb){
        rb.transform.localEulerAngles = Vector2.zero;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
    }
    void reset_pos(Rigidbody2D rb){
        rb.transform.position = Vector2.zero;
    }
    void debug_instanstiate(){
        Graph.YMin = graph_scale;
		Graph.YMax = -graph_scale;
 
		Graph.channel[ 0 ].isActive = true;
		Graph.channel[ 1 ].isActive = true;
    }
    void debug_update(){
        if(reset){
            reset_flow = 2;
            reset = false;
        }
        if(reset_flow == 2){
            for(int i=0; i < tr.Length; i++){
                tr[i].emitting = false;
            }
            reset_flow = 1;
        }else if(reset_flow == 1){
            for(int i=0; i < rbs.Length; i++){
                reset_pos(rbs[i]);
                reset_rb(rbs[i]);
            }
            for(int i=0; i < rbv.Length; i++){
                reset_rb(rbv[i]);
            }
            reset_flow = 0;
            for(int i=0; i < wheel_i.Length; i++){
                wheel_i[i].reset();
            }
        }else if(reset_flow == 0){
            for(int i=0; i < tr.Length; i++){
                tr[i].emitting = true;
            }
            reset_flow = -1;
        }
    }
}