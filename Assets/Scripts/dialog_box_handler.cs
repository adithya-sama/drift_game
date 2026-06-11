using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dialog_box_handler : MonoBehaviour
{

    public Image left_border, right_border;
    public Animator pearl_halo_animator;
    dialog_sequence_handler parent_dialog_sequence_handler;

    public void Start(){
        parent_dialog_sequence_handler = gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<dialog_sequence_handler>();
        if(parent_dialog_sequence_handler == null) {
            parent_dialog_sequence_handler = gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<dialog_sequence_handler>();
        }
    }

    public void close(){
        parent_dialog_sequence_handler.dialog_closed();
        left_border.material = null;
        right_border.material = null;
    }

    public void dialog_read(){
        parent_dialog_sequence_handler.dialog_read();
    }

    public void mark_dialog_read(){
        pearl_halo_animator.Play("transition_out");
    }

}
