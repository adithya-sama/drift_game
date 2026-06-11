using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deferred_rotation : MonoBehaviour
{

    public Transform reference_transform;
    public float defer_factor, offset;

    Vector3 final_rotation = new Vector3(0,0,0);

    void Start(){

        offset = reference_transform.eulerAngles.z;

    }

    void Update()
    {
        float reference_rotation = reference_transform.eulerAngles.z;
        final_rotation.z = offset + ((reference_rotation - offset) * defer_factor);

        transform.eulerAngles = final_rotation;
    }
}
