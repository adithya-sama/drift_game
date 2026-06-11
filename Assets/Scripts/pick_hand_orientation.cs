using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class pick_hand_orientation : MonoBehaviour
{

    public UnityEvent on_orientation_pick;

    void Awake()
    {
        if (PlayerPrefs.HasKey("hand_orientation")){
            gameObject.SetActive(false);
        }

    }

    public void orientation_picked(bool right_handed){
        PlayerPrefs.SetInt("hand_orientation", right_handed ? 1 : 0);
        PlayerPrefs.Save();
        on_orientation_pick.Invoke();
    }

}
