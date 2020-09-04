using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minion_ai : MonoBehaviour, Ienemy_with_damage
{
    public Transform protectee, player;
    public int health, acceleration, dash_acceleration, target_dist_tolerance, dist_from_protectee, dist_from_player, object_avoidance_dist, delay_after_dash;
    public float min_scale, max_scale, circle_cast_radius;
    public Vector2 level_bounds;
    Rigidbody2D rigid_body;
    Vector2 vector2_tmp, current_direction;
    RaycastHit2D[] ray_results = new RaycastHit2D[1];
    int object_avoidance_dist_sqrd, current_health;
    LayerMask raycast_layer_mask;
    bool bool_tmp;
    float dash_cooldown_time_watcher, float_tmp;
    minion_respawn_handler respawn_handler;
    void Awake()
    {
        rigid_body = gameObject.GetComponent<Rigidbody2D>();
        target_dist_tolerance = target_dist_tolerance * target_dist_tolerance;
        object_avoidance_dist_sqrd = object_avoidance_dist * object_avoidance_dist;
        raycast_layer_mask = LayerMask.GetMask("Default", "key_minion");
        dist_from_player = dist_from_player * dist_from_player;
    }
    public void Damage(int damage)
    {
        current_health -= damage;
        vector2_tmp.x = vector2_tmp.y = min_scale + ((max_scale - min_scale) * ((float)current_health / health));
        transform.localScale = vector2_tmp;
        if (current_health <= 0)
        {
            gameObject.SetActive(false);
            respawn_handler.minion_died(gameObject);
        }
    }
    public void set_respawn_handler(minion_respawn_handler handler)
    {
        respawn_handler = handler;
    }
    void OnEnable()
    {
        current_health = health;
        vector2_tmp.x = vector2_tmp.y = max_scale;
        transform.localScale = vector2_tmp;
        get_random_point_around_protectee();
    }
    void FixedUpdate()
    {
        if(!protectee.gameObject.activeSelf){
            gameObject.SetActive(false);
        }
        float_tmp = (protectee.position - player.position).sqrMagnitude;
        vector2_tmp = (Vector2)protectee.position + current_direction;
        bool_tmp = (((Vector2)transform.position - vector2_tmp).sqrMagnitude <= target_dist_tolerance);

        if(bool_tmp){
            get_random_point_around_protectee();
        }
        if(
            (float_tmp <= dist_from_player) &&
            (
                (dash_cooldown_time_watcher < 0) ||
                bool_tmp
            )
        ){
            dash(player.position, dash_acceleration);
            dash_cooldown_time_watcher = delay_after_dash;
        }else{
            move_to(vector2_tmp, acceleration);
        }
        dash_cooldown_time_watcher -= Time.fixedDeltaTime;

    }
    void move_to(Vector2 target_position, float accleration)
    {
        Utils.move_towards(transform, ref target_position, ref circle_cast_radius, ref object_avoidance_dist, ref ray_results, ref raycast_layer_mask);
        //vector2_tmp = target_position;
        target_position = transform.InverseTransformPoint(target_position).normalized;
        rigid_body.AddRelativeForce(target_position * accleration, ForceMode2D.Force);
    }
    void dash(Vector3 position, float accleration)
    {
        position = transform.InverseTransformPoint(position).normalized;
        rigid_body.AddRelativeForce(position * accleration, ForceMode2D.Impulse);
    }
    void get_random_point_around_protectee()
    {
        current_direction = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector2.up * dist_from_protectee;
    }
    // void OnDrawGizmos()
    // {
    //     Gizmos.DrawLine(transform.position, vector2_tmp);
    // }
}