using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blast_minion_handler : MonoBehaviour
{
    public float blast_power, ttl, orig_scale, min_scale;
    public Rigidbody2D player;
    minion_respawn_handler respawn_handler;
    float time_watcher;
    Vector2 vector2_tmp;
    void OnEnable(){
        time_watcher = 0;
        vector2_tmp.x = vector2_tmp.y = orig_scale;
        transform.localScale = vector2_tmp;
    }
    void FixedUpdate(){
        if(time_watcher > ttl){
            blast_minion_pool.instance.return_to_pool(this);
            return;
        }else{
            vector2_tmp.x = vector2_tmp.y = min_scale + ((orig_scale - min_scale) * (1 - (time_watcher / ttl)));
            transform.localScale = vector2_tmp;
        }
        time_watcher += Time.fixedDeltaTime;
    }
    void OnCollisionEnter2D(Collision2D collision){
        if(collision.collider.CompareTag("Player")){
            player.AddForceAtPosition(
                (collision.contacts[0].point - (Vector2)transform.position).normalized * blast_power,
                collision.contacts[0].point,
                ForceMode2D.Impulse
            );
            blast_minion_pool.instance.return_to_pool(this);
        }else if(collision.collider.CompareTag("bullets")){
            blast_minion_pool.instance.return_to_pool(this);
        }
    }
}