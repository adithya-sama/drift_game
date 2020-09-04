using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class blast_minion_master : MonoBehaviour, Ienemy_with_damage
{
    public int enemy_id;
    public float max_scale, min_scale, radius, rotation_step, acceleration, tolerance_dist, circle_cast_radius;
    public int object_avoidance_dist, health;
    public UnityEvent on_killed;
    Vector2 current_target_position;
    LayerMask raycast_layer_mask;
    RaycastHit2D[] ray_results = new RaycastHit2D[1];
    Rigidbody2D rigid_body;
    float current_health;
    Vector2 vector2_tmp;
    void Awake(){
        rigid_body = gameObject.GetComponent<Rigidbody2D>();
        current_health = health;
        current_target_position = Vector2.up * radius;
        vector2_tmp.x = vector2_tmp.y = max_scale;
        transform.localScale = vector2_tmp;
    }
    void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, current_target_position) < tolerance_dist)
        {
            current_target_position = Quaternion.Euler(0, 0, rotation_step) * current_target_position;
        }
        move_to(current_target_position, acceleration);
    }
    public void Damage(int damage)
    {
        current_health -= damage;
        if(current_health <= 0){
            gameObject.SetActive(false);
            on_killed.Invoke();
            return;
        }
        vector2_tmp.x = vector2_tmp.y = min_scale + ((max_scale - min_scale) * (current_health / health));
        transform.localScale = vector2_tmp;
    }
    void move_to(Vector2 target_position, float accleration)
    {
        Utils.move_towards(transform, ref target_position, ref circle_cast_radius, ref object_avoidance_dist, ref ray_results, ref raycast_layer_mask);
        //vector2_tmp = target_position;
        target_position = transform.InverseTransformPoint(target_position).normalized;
        rigid_body.AddRelativeForce(target_position * accleration, ForceMode2D.Force);
    }
}