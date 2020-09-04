using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bumper : MonoBehaviour
{
    public float impact_force, collision_effect_time;
    private Rigidbody2D chasis_rb;
    private float collision_effect = 0;
    void Start(){
        chasis_rb = gameObject.GetComponentsInParent<Rigidbody2D>()[0];
    }
    void FixedUpdate(){
        if(collision_effect > 0){
            collision_effect -= Time.fixedDeltaTime;
        }
    }
    void OnCollisionEnter2D(Collision2D collision){
        if(collision.collider.tag != "bullets" && collision.collider.tag != "Enemy" && collision_effect <= 0){
            //chasis_rb.velocity = transform.parent.InverseTransformVector(new Vector3(transform.parent.TransformVector(chasis_rb.velocity).x, 0, 0));
            chasis_rb.AddRelativeForce(-impact_force * Vector2.up, ForceMode2D.Impulse);
            collision_effect = collision_effect_time;
        }
    }
    void OnCollisionStay2D(Collision2D collision){
        if(collision.collider.tag != "bullets" && collision.collider.tag != "Enemy" && collision_effect <= 0){
            //chasis_rb.velocity = transform.parent.InverseTransformVector(new Vector3(transform.parent.TransformVector(chasis_rb.velocity).x, 0, 0));
            chasis_rb.AddRelativeForce(-impact_force * Vector2.up, ForceMode2D.Impulse);
            collision_effect = collision_effect_time;
        }
    }
}