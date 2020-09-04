using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemy_shield_handler : MonoBehaviour, Ienemy_with_damage
{
    public float initial_health = 100, max_scale, min_scale, min_angle;
    public float shield_radius, shield_segments;
    public Image shield_sprite;
    public GameObject impact_animation;

    [SerializeField]
    float current_health;
    float shield_damage_arc_degree, shield_health_per_segment, shield_hole_degree;
    EdgeCollider2D _collider;
    Vector2[] collider_points;
    enemy_handler enemy_core;
    int int_tmp;
    float float_tmp;
    // Start is called before the first frame update
    void Start()
    {
        _collider = gameObject.GetComponent<EdgeCollider2D>();
        enemy_core = gameObject.GetComponentInParent<enemy_handler>();
        collider_points = new Vector2[(int)shield_segments + 3];
        init_collider();
        current_health = initial_health;
        shield_damage_arc_degree = 360 / initial_health;
        shield_health_per_segment = initial_health / shield_segments;
    }
    void init_collider()
    {
        collider_points[0] = Vector2.zero;
        for (int i = 1; i < shield_segments + 2; i++)
        {
            collider_points[i] = Quaternion.Euler(0, 0, (i - 1) * 360 / shield_segments) * (Vector2.up * shield_radius);
        }
        collider_points[(int)shield_segments + 2] = Vector2.zero;
        _collider.points = collider_points;
    }
    void damage_shield()
    {
        float_tmp = current_health * shield_damage_arc_degree;
        if (float_tmp > min_angle)
        {
            int_tmp = Mathf.CeilToInt(current_health / shield_health_per_segment) + 1;
            for (int i = int_tmp + 1; i < collider_points.Length; i++)
            {
                collider_points[i] = Vector2.zero;
            }
            collider_points[int_tmp] = Quaternion.Euler(0, 0, float_tmp) * (Vector2.up * shield_radius);
            _collider.points = collider_points;
            shield_sprite.fillAmount = current_health / initial_health;
            transform.localEulerAngles = new Vector3(0, 0, -float_tmp / 2);
        }
    }
    public void Damage(int damage)
    {
        current_health -= damage;
        if (current_health <= 0)
        {
            enemy_core.shield_destroyed();
            gameObject.SetActive(false);
        }

        Vector3 tmp = transform.localScale;
        tmp.x = tmp.y = min_scale + ((current_health / initial_health) * (max_scale - min_scale));
        transform.localScale = tmp;

        damage_shield();

        Instantiate(impact_animation, transform.position, Quaternion.identity);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("bullets"))
        {
            enemy_core.Onshieldhit(collision);
        }
    }
    // void OnDrawGizmos(){
    //     Gizmos.color = Color.white;
    //     Gizmos.DrawLine(Vector2.zero, transform.InverseTransformDirection(enemy_core.player_transform.position));
    // }
}
