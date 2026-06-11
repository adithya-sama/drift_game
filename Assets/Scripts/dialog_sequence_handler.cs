using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class dialog_sequence_handler : MonoBehaviour
{
    Button dialog_read_btn;
    public UnityEvent on_dialog_sequence_start, on_dialog_sequence_end;
    public GameObject[] dialogs = new GameObject[1];
    public UnityEvent[] on_dialog_end = new UnityEvent[1];
    int active_dialog_index = 0, current_dialog_index = -1;
    bool sequence_ended = false;
    GraphicRaycaster graphic_raycaster;

    void Start(){
        dialog_read_btn = gameObject.GetComponent<Button>();
        graphic_raycaster = gameObject.transform.parent.GetComponent<GraphicRaycaster>();
    }
    public void start_dialog(){
        graphic_raycaster.enabled = true;
        on_dialog_sequence_start.Invoke();
        if(global_variables._disable_dialog){
            current_dialog_index = dialogs.Length - 1;
            on_click();
            return;
        };
        next_dialog();
    }
    public void next_dialog(){
        if(current_dialog_index != -1)
            dialogs[current_dialog_index].GetComponentInChildren<Animator>().Play("finished_reading");
        dialogs[++current_dialog_index].GetComponentInChildren<Animator>().Play("fade_in");
    }
    void clear(){
        for(;active_dialog_index <= current_dialog_index; active_dialog_index++){
            dialogs[active_dialog_index].GetComponentInChildren<Animator>().Play("fade_out");
        }
    }
    public void clear_and_next_dialog(){
        clear();
        next_dialog();
    }
    public void end_sequence(){
        graphic_raycaster.enabled = false;
        dialogs[current_dialog_index].GetComponentInChildren<Animator>().Play("finished_reading");
        clear();
        sequence_ended = true;
        on_dialog_sequence_end.Invoke();
        Debug.Log("dialog sequence ended");
    }
    public void dialog_closed(){
        if(sequence_ended){
            gameObject.SetActive(false);
        }
    }
    public void on_click(){
        dialog_read_btn.interactable = false;
        if(current_dialog_index == (dialogs.Length - 1)){
            end_sequence();
        }
        on_dialog_end[current_dialog_index].Invoke();
    }
    public void dialog_read(){
        dialog_read_btn.interactable = true;
    }
}