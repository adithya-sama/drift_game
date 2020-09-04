using System.Collections;
using UnityEngine;
public class vehicle_teleport_handler : MonoBehaviour
{
    public GameObject chasis;
    public float teleport_distance, teleport_time, max_rotation_speed;
    public Vector2 level_bounds;
    public Gradient on_collision_color, default_color;
    LineRenderer teleport_pos_indicator;
    Rigidbody2D chasis_transform;
    teleport_force chasis_teleport_force;
    Vector2 teleport_direction;
    bool out_of_bounds = false;
    int number_of_overlaps = 0;
    void Start(){
        teleport_pos_indicator = transform.GetComponent<LineRenderer>();
        chasis_transform = chasis.GetComponent<Rigidbody2D>();
        chasis_teleport_force = chasis.GetComponent<teleport_force>();
    }
    void OnEnable()
    {
        mouse_event_watcher.on_mouse_double_click += on_mouse_double_click;
    }
    void OnDisable()
    {
        mouse_event_watcher.on_mouse_double_click -= on_mouse_double_click;
    }
    void FixedUpdate(){
        if(chasis.activeSelf){
            update_teleport_indicator();
            teleport_direction = chasis_transform.velocity.normalized;
            if(teleport_direction.sqrMagnitude == 0)
                teleport_direction = Vector2.up;
            transform.position = teleport_direction * teleport_distance + chasis_transform.position;
            transform.localEulerAngles = new Vector3(0,0,chasis_transform.transform.localEulerAngles.z);
            check_bounds();
        }
    }
    void check_bounds(){
        if((Mathf.Abs(transform.position.x) > level_bounds.x) || (Mathf.Abs(transform.position.y) > level_bounds.y)){
            out_of_bounds = true;
        }else{
            out_of_bounds = false;
        }
    }
    void update_teleport_indicator(){
        teleport_pos_indicator.SetPosition(0, chasis_transform.position);
        teleport_pos_indicator.SetPosition(1, transform.position);
        if(!out_of_bounds && number_of_overlaps == 0){
            teleport_pos_indicator.colorGradient = default_color;
        }else{
            teleport_pos_indicator.colorGradient = on_collision_color;
        }
    }
    void on_mouse_double_click(){
        if(!out_of_bounds && number_of_overlaps == 0){
            chasis_teleport_force.store_velocity();
            teleport_pos_indicator.enabled = false;
            chasis.SetActive(false);
            StartCoroutine(re_enable());
        }
    }
    IEnumerator re_enable(){
        yield return new WaitForSeconds(teleport_time);
        chasis.transform.position = transform.position;
        chasis.SetActive(true);
        teleport_pos_indicator.enabled = true;
    }
    void OnTriggerEnter2D(Collider2D obj){
        if(!obj.CompareTag("bullets") && !obj.CompareTag("Player")){
            number_of_overlaps++;
        }
    }
    void OnTriggerExit2D(Collider2D obj){
        if(!obj.CompareTag("bullets") && !obj.CompareTag("Player")){
            number_of_overlaps--;
        }
    }
}