using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class animation_handler: MonoBehaviour
{

    public UnityEvent[] animations;

    public void animation_end(int animation_index){
        Debug.Log(gameObject.name + ": animation end");
        animations[animation_index].Invoke();
    }
}
