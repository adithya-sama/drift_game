using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ink_plate_ink_handler : MonoBehaviour
{
    public Collider2D player_collider;
    [SerializeField]
    public UltEvents.UltEvent on_player_enter;

    void onEnable(){
        if(gameObject.GetComponent<Collider2D>().IsTouching(player_collider)){
            on_player_enter.Invoke();
        }
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.CompareTag("Player")){
            on_player_enter.Invoke();
        }
    }
}
