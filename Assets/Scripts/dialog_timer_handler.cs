using UnityEngine; 
using System.Collections; 
 
public class dialog_timer_handler : MonoBehaviour { 

    public Animator anim; 
    public float speed = 1f;

    void OnEnable() { 
        anim.speed = 1 / speed; 
    } 

} 