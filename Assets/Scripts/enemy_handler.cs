using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class enemy_handler : MonoBehaviour, Ienemy_with_damage
{
    public bool testing;
    // general:
    public int object_avoidance_dist, health;

    // idle:
    public float idle_max_angle;
    public float idle_circle_radius;
    public float idle_acceleration;
    public float idle_min_distance;
    public float idle_rotation_torque;
    Vector2 current_idle_position;
    // player in range:
    public Transform player_transform;
    public float player_in_range_detect_dist, player_in_range_min_dist;
    public float player_in_range_attack_acceleration, player_in_range_dash_acceleration;
    public float player_in_range_dash_cool_down, player_in_range_rotation_step, player_in_range_revolution_speed;
    // bullet collision:
    public float bullet_collision_dash_acceleration;
    public float circle_cast_radius;
    Rigidbody2D rigid_body;
    Vector2 enemy_pos_in_player_local, bullet_collision_ortho_normal;
    //Vector2 vector2_tmp;
    float float_tmp;
    float angle_from_player_2_enemy, dash_cool_down_watcher;
    RaycastHit2D[] ray_results = new RaycastHit2D[1];
    LayerMask raycast_layer_mask;
    bool is_shield_destroyed = false;
    public int current_health;
    public enum states
    {
        idle,
        player_in_range,
        bullet_collision
    };
    public states state;
    void Start()
    {
        current_health = health;
        Random.InitState(System.DateTime.Now.Second);
        state = states.idle;
        rigid_body = gameObject.GetComponent<Rigidbody2D>();
        current_idle_position = Vector2.up * idle_circle_radius;
        raycast_layer_mask = LayerMask.GetMask("Default", "no_grapple");
    }
    void FixedUpdate()
    {
        if (testing)
        {
            return;
        }
        switch (state)
        {
            case states.idle:
                if (Vector2.Distance(transform.position, current_idle_position) < idle_min_distance)
                {
                    current_idle_position = Quaternion.Euler(0, 0, idle_max_angle) * current_idle_position;
                }
                move_to(current_idle_position, idle_acceleration);
                rigid_body.AddTorque(idle_rotation_torque);
                break;
            case states.player_in_range:

                if (
                    (dash_cool_down_watcher == 0) &&
                    (Vector2.Distance(transform.position, player_transform.position) < player_in_range_min_dist)
                )
                {
                    dash(player_transform.position, player_in_range_dash_acceleration);
                    dash_cool_down_watcher = player_in_range_dash_cool_down;
                }
                else
                {

                    enemy_pos_in_player_local = player_transform.InverseTransformPoint(transform.position);
                    angle_from_player_2_enemy = Vector2.SignedAngle(Vector2.up, enemy_pos_in_player_local);
                    if (Mathf.Abs(angle_from_player_2_enemy) < 180)
                    {

                        enemy_pos_in_player_local = Quaternion.AngleAxis(
                                                        Mathf.Sign(angle_from_player_2_enemy) * player_in_range_rotation_step,
                                                        Vector3.forward
                                                        ) * enemy_pos_in_player_local.normalized * player_in_range_min_dist;

                        move_to(player_transform.TransformPoint(enemy_pos_in_player_local), player_in_range_attack_acceleration);
                    }
                }
                rotate_toward(player_transform.position, player_in_range_revolution_speed);
                break;
            case states.bullet_collision:
                dash(bullet_collision_ortho_normal, bullet_collision_dash_acceleration);
                rotate_toward(player_transform.position, player_in_range_revolution_speed);
                break;
        }
        if (dash_cool_down_watcher > 0)
            dash_cool_down_watcher = Mathf.Max(0, dash_cool_down_watcher - Time.fixedDeltaTime);
        check_player_distance();
    }
    void move_to(Vector2 target_position, float accleration)
    {
        Utils.move_towards(transform, ref target_position, ref circle_cast_radius, ref object_avoidance_dist, ref ray_results, ref raycast_layer_mask);
        target_position = transform.InverseTransformPoint(target_position).normalized;
        rigid_body.AddRelativeForce(target_position * accleration, ForceMode2D.Force);
    }
    void dash(Vector3 position, float accleration)
    {
        position = transform.InverseTransformPoint(position).normalized;
        rigid_body.AddRelativeForce(position * accleration, ForceMode2D.Impulse);
    }
    void rotate_toward(Vector3 target, float speed)
    {
        float_tmp = Vector2.SignedAngle(Vector2.up, transform.InverseTransformPoint(target));
        transform.Rotate(0, 0, Mathf.Sign(float_tmp) * Mathf.Min(speed, Mathf.Abs(float_tmp)));
    }
    void check_player_distance()
    {
        if (Vector2.Distance(transform.position, player_transform.position) < player_in_range_detect_dist)
        {
            state = states.player_in_range;
        }
        else
        {
            state = states.idle;
        }
    }
    public void shield_destroyed()
    {
        is_shield_destroyed = true;
    }
    public void Onshieldhit(Collision2D collision)
    {
        bullet_collision_ortho_normal = player_transform.up;
        if (Random.Range(0, 100) > 50)
        {
            bullet_collision_ortho_normal = Quaternion.AngleAxis(90, Vector3.forward) * bullet_collision_ortho_normal;
        }
        else
        {
            bullet_collision_ortho_normal = Quaternion.AngleAxis(-90, Vector3.forward) * bullet_collision_ortho_normal;
        }
        bullet_collision_ortho_normal += (Vector2)transform.position;
        state = states.bullet_collision;
    }
    public void Damage(int damage)
    {
        if (is_shield_destroyed)
        {
            current_health -= damage;
            if (current_health <= 0)
            {
                level_3_handler.instance.start_next_sequence(2);
                gameObject.SetActive(false);
            }
        }
    }
    // void OnDrawGizmos()
    // {
    //     Gizmos.DrawLine(Vector2.zero, vector2_tmp);
    // }
}