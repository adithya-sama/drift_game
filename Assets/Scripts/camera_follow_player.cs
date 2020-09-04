using UnityEngine;
using UnityEngine.Events;
public class camera_follow_player : MonoBehaviour
{
    public mouse_event_watcher mouse_event_watcher_instance;
    public float ahead_distance_percentage, grappled_ahead_dist, follow_speed, max_follow_speed, grappled_follow_speed, padding, min_focus_time, ahead_distance;
    public Vector2 level_bounds;
    public GameObject player;
    public UnityEventInt focus_done;
    Transform player_transform;
    grapple_chain_handler player_chain_handler;
    Rigidbody2D player_rigid_body;
    Vector2 prev_player_up, vector2_tmp, focus_pos;
    float float_tmp, max_distance, distance_to_player, current_ahead_dist, current_follow_speed, current_focus_dist, focus_speed;
    int focus_id;
    bool focus, idle = false;
    void Awake(){
        player_transform = player.transform;
        player_chain_handler = player.GetComponentInChildren<grapple_chain_handler>(true);
        player_rigid_body = player_transform.GetComponent<Rigidbody2D>();
        vector2_tmp.y = Camera.main.orthographicSize;
        vector2_tmp.x = vector2_tmp.y * Camera.main.aspect;
        max_distance = Mathf.Min(vector2_tmp.x, vector2_tmp.y) * padding;
        ahead_distance = max_distance * ahead_distance_percentage;
        max_distance *= max_distance;
        level_bounds -= vector2_tmp;
    }
    void FixedUpdate()
    {
        if (idle) { return; }
        if (focus)
        {
            if ((Vector2)transform.position != focus_pos)
            {
                float_tmp = Mathf.Max((current_focus_dist / focus_speed) * Time.fixedDeltaTime, 0.1f);
                transform.position = Vector3.MoveTowards(transform.position, focus_pos, float_tmp);
                current_focus_dist -= float_tmp;
            }
            else
            {
                idle = true;
                focus_done.Invoke(focus_id);
            }
        }
        else
        {
            if (player_chain_handler.engaged)
            {
                current_ahead_dist = grappled_ahead_dist;
                current_follow_speed = grappled_follow_speed;
            }
            else
            {
                current_ahead_dist = ahead_distance;
                distance_to_player = (transform.position - player_transform.position).sqrMagnitude;
                if (distance_to_player >= max_distance)
                {
                    current_follow_speed = max_follow_speed;
                }
                else
                {
                    current_follow_speed = follow_speed;
                }
            }
            vector2_tmp = player_transform.position + (player_transform.up * Mathf.Lerp(0, current_ahead_dist, 1 - Mathf.Min(1, (Vector2.Angle(prev_player_up, player_transform.up)) / 10)));
            vector2_tmp = Vector2.Lerp(
                transform.position,
                vector2_tmp,
                current_follow_speed * Time.fixedDeltaTime
            );
            Utils.apply_bounds(ref vector2_tmp, level_bounds);
            transform.position = vector2_tmp;
            prev_player_up = player_transform.up;
        }
    }
    public void focus_on(Vector2 position, int f_id, float speed = 2)
    {
        focus_id = f_id;
        idle = false;
        focus = true;
        focus_speed = speed;
        focus_pos = position;
        Utils.apply_bounds(ref focus_pos, level_bounds);
        current_focus_dist = Vector2.Distance(transform.position, focus_pos);
        mouse_event_watcher_instance.stop_events();
    }
    public void reset_focus()
    {
        focus = false;
        idle = false;
        mouse_event_watcher_instance.start_events();
    }
    // void OnDrawGizmos(){
    //     Gizmos.DrawLine(transform.position, vector2_tmp);
    // }
}