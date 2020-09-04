using System.Collections.Generic;
using UnityEngine;
public class bullet_launcher : MonoBehaviour
{
    public string enemy_tag;
    public float bullet_frequency, max_rotation, rotation_speed;
    GameObject bullet_i;
    float time_watcher = 0, distance_from_enemy, angle_from_enemy, tmp_float;
    bool shoot = false;
    Vector2 vector2_tmp;
    public Dictionary<int, Transform> enemies_in_range = new Dictionary<int, Transform>();
    Transform enemy_transform, transform_tmp, parent_transform;
    int shoot_range, int_tmp, int_tmp_1;
    void Awake(){
        parent_transform = transform.parent.transform;
    }
    void OnEnable()
    {
        time_watcher = 0;
    }
    void FixedUpdate()
    {
        shoot = false;
        if (enemies_in_range.Count > 0)
        {
            find_closest_enemy();
            if (enemy_transform != null)
            {
                vector2_tmp = enemy_transform.position - transform.position;
                if (parent_transform != null)
                {
                    angle_from_enemy = Vector2.SignedAngle(parent_transform.up, vector2_tmp);
                }
                else
                {
                    angle_from_enemy = Vector2.SignedAngle(Vector2.up, vector2_tmp);
                }
                if (Mathf.Abs(angle_from_enemy) < max_rotation)
                {
                    shoot = true;
                }
                tmp_float = (Mathf.Clamp(angle_from_enemy, -max_rotation, max_rotation) - Utils.ToSignedRotation(transform.localEulerAngles.z));
                transform.Rotate(0, 0, Mathf.Sign(tmp_float) * Mathf.Min(rotation_speed, Mathf.Abs(tmp_float)));
            }
        }
        time_watcher += Time.fixedDeltaTime;
        if (shoot && (time_watcher >= bullet_frequency))
        {
            bullet_i = bullet_1_pool.instance.get().gameObject;
            bullet_i.transform.position = transform.position;
            bullet_i.transform.rotation = transform.rotation;
            bullet_i.SetActive(true);
            time_watcher = 0;
        }
    }
    void find_closest_enemy()
    {
        int_tmp = int.MaxValue;
        enemy_transform = null;
        foreach (Transform transform_tmp in enemies_in_range.Values)
        {
            int_tmp_1 = (int)(transform.position - transform_tmp.position).sqrMagnitude;
            if (int_tmp_1 < int_tmp)
            {
                enemy_transform = transform_tmp;
                int_tmp = int_tmp_1;
            }
        }
    }
}