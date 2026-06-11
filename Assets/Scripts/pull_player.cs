using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pull_player : MonoBehaviour
{

    public GameObject player;
    public Vector2 to_pos;
    public float speed;

    [SerializeField]
    public UltEvents.UltEvent on_enable, on_done;

    bool is_player_in_pos = false;

    void OnEnable(){
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        on_enable.Invoke();
    }

    void FixedUpdate()
    {
        if(!is_player_in_pos){
            player.transform.position = Vector2.MoveTowards(player.transform.position, to_pos, speed * Time.fixedDeltaTime);
            if((Vector2)player.transform.position == to_pos){
                player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                is_player_in_pos = true;
                on_done.Invoke();
            }
        }
    }

    public void set_pull_location(Vector2 pos){
        to_pos = pos;
        is_player_in_pos = false;
    }

}
