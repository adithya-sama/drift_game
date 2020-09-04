using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minion_ai_passive : MonoBehaviour, Ienemy_with_damage
{
    public Vector2 level_bounds, level_offset;
    public int health, acceleration, target_dist_tolerance, object_avoidance_dist;
    public float circle_cast_radius, min_scale, max_scale;

    int current_health, target_dist_tolerance_sqrd;
    LayerMask raycast_layer_mask;
    Vector2 vector2_tmp, current_target;
    RaycastHit2D[] ray_results = new RaycastHit2D[1];
    Rigidbody2D rigid_body;
    void Start(){
        target_dist_tolerance_sqrd = target_dist_tolerance * target_dist_tolerance;
        rigid_body = gameObject.GetComponent<Rigidbody2D>();
        raycast_layer_mask = LayerMask.GetMask("Default");
    }
    void OnEnable(){
        current_health = health;
        get_random_point();
    }
    public void Damage(int damage)
    {
        current_health -= damage;
        vector2_tmp.x = vector2_tmp.y = min_scale + ((max_scale - min_scale) * ((float)current_health / health));
        transform.localScale = vector2_tmp;
        if (current_health <= 0)
        {
            level_2_handler.instance.minion_killed();
            gameObject.SetActive(false);
        }
    }
    void FixedUpdate()
    {
        if (((Vector2)transform.position - current_target).sqrMagnitude <= target_dist_tolerance_sqrd)
        {
            get_random_point();
        }
        move_to(current_target, acceleration);
    }
    void get_random_point(){
        current_target.x = Random.Range(-level_bounds.x, level_bounds.x);
        current_target.y = Random.Range(-level_bounds.y, level_bounds.y);
        current_target += level_offset;
    }
    void move_to(Vector2 target_position, float accleration)
    {
        Utils.move_towards(transform, ref target_position, ref circle_cast_radius, ref object_avoidance_dist, ref ray_results, ref raycast_layer_mask);
        //vector2_tmp = target_position;
        target_position = transform.InverseTransformPoint(target_position).normalized;
        rigid_body.AddRelativeForce(target_position * accleration, ForceMode2D.Force);
    }
    void OnDrawGizmos(){
        Gizmos.DrawLine(transform.position, current_target);
    }
}
