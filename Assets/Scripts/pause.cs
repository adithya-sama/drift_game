using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class pause : MonoBehaviour
{
    public float pause_time;
    public bool disable_on_end = true;
    public UnityEvent on_end;
    float time_watcher = 0;
    bool done = false;
    void FixedUpdate(){
        if(done){return;}
        time_watcher += Time.fixedDeltaTime;
        if(time_watcher > pause_time){
            on_end.Invoke();
            if(disable_on_end){
                gameObject.SetActive(false);
            }
            done = true;
        }
    }
}
