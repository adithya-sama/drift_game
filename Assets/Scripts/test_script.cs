using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_script : MonoBehaviour, Ipullable
{
    bool test = false;
    grapple_handle_handler gh_handler;
    public void pull(){
        transform.position = new Vector3(transform.position.x, transform.position.y - 1f);
    }
    public void set_grapple_handler(grapple_handle_handler handler){
        gh_handler = handler;
    }
    void FixedUpdate(){
        if(test){
            gh_handler.forced_detach();
        }
    }

    void Ipullable.grappled()
    {
        throw new System.NotImplementedException();
    }

    void Ipullable.ungrappled()
    {
        throw new System.NotImplementedException();
    }
}
