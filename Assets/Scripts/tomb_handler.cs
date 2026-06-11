using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class tomb_handler : MonoBehaviour
{
    public bool allow_ink = false;
    public player_handler player_handler_i;
    [SerializeField]
    public UltEvents.UltEvent on_red, on_green, on_blue;
    public int tomb_stone_speed;
    [SerializeField]
    public UltEvents.UltEvent on_tomb_stone_enter, on_tomb_stone_in_place;
    bool put_stone_in_place = false, stone_in_place = false;
    Transform tomb_stone;
    void FixedUpdate(){
        if(put_stone_in_place){
            tomb_stone.position = Vector2.MoveTowards(tomb_stone.position, transform.position, tomb_stone_speed * Time.fixedDeltaTime);
            if(tomb_stone.position == transform.position){
                put_stone_in_place = false;
                stone_in_place = true;
                on_tomb_stone_in_place.Invoke();
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collider){
        if(!put_stone_in_place && !stone_in_place && collider.CompareTag("tomb_stone")){
            tomb_stone = collider.transform;
            Rigidbody2D ts_rb = tomb_stone.GetComponent<Rigidbody2D>();
            tomb_stone_handler ts_handler = tomb_stone.GetComponent<tomb_stone_handler>();
            ts_handler.set_rb_type("kinematic");
            ts_rb.linearVelocity = Vector2.zero;
            ts_rb.angularVelocity = 0;
            if(ts_handler.grappled){
                ts_handler.forced_detach();
                ts_handler.become_ungrapplelable();
            }
            put_stone_in_place = true;
            on_tomb_stone_enter.Invoke();
        }else if(collider.CompareTag("Player")){
            if(allow_ink){
                string color = player_handler_i.current_color;
                if(color == "RED"){
                    on_red.Invoke();
                }else if(color == "GREEN"){
                    on_green.Invoke();
                }else if(color == "BLUE"){
                    on_blue.Invoke();
                }
            }
        }
    }

    public void set_allow_ink(bool val){
        allow_ink = val;
    }

}