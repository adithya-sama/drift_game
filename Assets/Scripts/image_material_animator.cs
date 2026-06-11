using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class image_material_animator : MonoBehaviour
{

    private Material img_mat;
    public string property;
    public float property_val;

    // void Awake()
    // {
    //     if(property_val != 0) {
    //         img_mat.SetFloat(property, property_val);
    //     }
    // }

   void Start()
    {
        Image img_component = GetComponent<Image>();
        //Fetch the Material from the Renderer of the GameObject
        img_mat = Instantiate(img_component.material);
        img_component.material = img_mat;
    }

    // Update is called once per frame
    void Update()
    {
        if(property_val != 0) {
            img_mat.SetFloat(property, property_val);
        }
    }
}
