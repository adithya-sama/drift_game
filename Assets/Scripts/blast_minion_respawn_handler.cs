using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blast_minion_respawn_handler : MonoBehaviour
{
    public GameObject protectee;
    public Rigidbody2D player;
    public float frequency, radius;
    public int min_minions, max_minions;
    float time_watcher;
    int tmp_int;
    GameObject go_tmp;
    blast_minion_handler bmh_tmp;
    void FixedUpdate(){
        if(protectee.activeSelf){
            if(time_watcher > frequency){
                deploy_minions();
                time_watcher = 0;
            }
        }else{
            gameObject.SetActive(false);
        }
        time_watcher += Time.fixedDeltaTime;
    }
    void deploy_minions(){
        tmp_int = Random.Range(min_minions, max_minions);
        for(int i=1; i <= tmp_int; i++){
            bmh_tmp = blast_minion_pool.instance.get();
            bmh_tmp.player = player;
            go_tmp = bmh_tmp.gameObject;
            go_tmp.transform.position = Quaternion.Euler(0, 0, i * (360 / tmp_int)) * Vector2.up * radius;
            go_tmp.transform.position += protectee.transform.position;
            go_tmp.SetActive(true);
        }
    }
}