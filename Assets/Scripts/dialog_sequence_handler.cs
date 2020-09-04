using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class dialog_sequence_handler : MonoBehaviour
{
    public GameObject[] dialogs = new GameObject[1];
    public UnityEvent[] on_dialog_end = new UnityEvent[1];
    int active_dialog_index = 0, current_dialog_index = -1;
    void OnEnable(){
        next_dialog();
    }
    public void next_dialog(){
        dialogs[++current_dialog_index].SetActive(true);
    }
    void clear(){
        for(;active_dialog_index <= current_dialog_index; active_dialog_index++){
            dialogs[active_dialog_index].SetActive(false);
        }
    }
    public void clear_and_next_dialog(){
        clear();
        next_dialog();
    }
    public void end_sequence(){
        clear();
        gameObject.SetActive(false);
    }
    public void on_click(){
        if(current_dialog_index == (dialogs.Length - 1)){
            end_sequence();
        }
        on_dialog_end[current_dialog_index].Invoke();
    }
}