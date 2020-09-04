using UnityEngine;

public class teleport_force : MonoBehaviour
{
    Vector2 velocity;
    Rigidbody2D rigid_body;
    bool disabled = false;
    void Start(){
        rigid_body = gameObject.GetComponent<Rigidbody2D>();
    }
    public void store_velocity(){
        velocity = rigid_body.velocity; 
    }
    void OnDisable(){
        disabled = true;
    }
    void OnEnable(){
        if(disabled){
            rigid_body.velocity = velocity;
        }
        disabled = false;
    }
}
