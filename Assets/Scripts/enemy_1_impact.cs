using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_1_impact : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.15f);        
    }
}
