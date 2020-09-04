using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class player_proximity_detector : MonoBehaviour
{
    public float circle_radius;
    public UnityEvent on_enter;
    public bool deactive_after_detection;
    void Start()
    {
        gameObject.GetComponent<CircleCollider2D>().radius = circle_radius;
    }
    void OnTriggerEnter2D(Collider2D collider){
        if(collider.CompareTag("Player")){
            on_enter.Invoke();
            if(deactive_after_detection){
                gameObject.SetActive(false);
            }
        }
    }
}