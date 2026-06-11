using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tomb_stone_handler : Grapplelable{

    public GameObject tomb_stone_pointer, tomb_pointer;

    Rigidbody2D rb;

    void Awake(){
        tomb_stone_pointer.SetActive(true);
        tomb_stone_pointer.GetComponent<Animator>().Play("transition_in");
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public override void attach(grapple_chain_handler handler){

        Debug.Log("tomb_stone_attach");

        tomb_stone_pointer.GetComponent<Animator>().Play("transition_out");

        tomb_pointer.SetActive(true);
        tomb_pointer.GetComponent<Animator>().Play("transition_in");

        base.attach(handler);
    }
    public override void reset(){

        Debug.Log("tomb_stone_detach");

        tomb_pointer.GetComponent<Animator>().Play("transition_out");

        tomb_stone_pointer.SetActive(true);
        tomb_stone_pointer.GetComponent<Animator>().Play("transition_in");

    }

    public void set_rb_type(string type){
        if(type == "static"){
            rb.bodyType = RigidbodyType2D.Static;
        }else if(type == "kinematic"){
            rb.bodyType = RigidbodyType2D.Kinematic;
        }else if(type == "dynamic"){
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

}
