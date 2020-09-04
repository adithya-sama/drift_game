using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_launcher_radar : MonoBehaviour
{
    public string enemy_tag;
    bullet_launcher launcher;
    void Awake(){
        if (enemy_tag.Length == 0)
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        else
            launcher = gameObject.GetComponentInChildren<bullet_launcher>();
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(enemy_tag))
            launcher.enemies_in_range[collider.gameObject.GetInstanceID()] = collider.transform;
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag(enemy_tag))
            launcher.enemies_in_range.Remove(collider.gameObject.GetInstanceID());
    }
}
