using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_1_explosion : MonoBehaviour
{
    public float x_offset, y_offset;
    public Animator animator;
    float time_watcher;
    void Start(){
        animator = gameObject.GetComponent<Animator>();
    }
    void on_animation_end(){
        bullet_1_explosion_pool.instance.return_to_pool(this);
    }
    void OnEnable(){
        transform.position += transform.TransformVector(new Vector3(x_offset, y_offset, 0));
        animator.Play("bullet_1_explosion_1", -1, 0);
    }
}