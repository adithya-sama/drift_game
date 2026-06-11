using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class player_proximity_detector : MonoBehaviour
{
    [SerializeField]
    public UltEvents.UltEvent on_enter;
    public bool deactive_after_detection;

    bool active = true;
    void OnTriggerEnter2D(Collider2D collider){
        if(!active) return;
        if(collider.CompareTag("Player")){
            on_enter.Invoke();
            if(deactive_after_detection){
                active = false;
            }
        }
    }
}