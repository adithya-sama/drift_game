using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vehicle_lighting_handler : MonoBehaviour
{
    public Transform light_source_transform;
    public Vector3 light_source_pos;
    public bool position;
    SpriteRenderer sprite_renderer;
    Material material;
    void Start(){
        sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        material = sprite_renderer.material;
    }
    void FixedUpdate()
    {
        if(!position && !light_source_transform){
            gameObject.SetActive(false);
            sprite_renderer.enabled = false;
        }else{
            sprite_renderer.enabled = true;
            material.SetVector("_light_normal", transform.InverseTransformPoint(position ? light_source_pos : light_source_transform.position).normalized);
        }
    }
}