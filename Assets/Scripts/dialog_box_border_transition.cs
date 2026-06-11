using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dialog_box_border_transition : MonoBehaviour
{

    public RectTransform mask, border;

    // Update is called once per frame
    void Update()
    {
        border.localScale = new Vector3(Mathf.Min(1 / mask.localScale.x, 200), 1, 1);
    }
}
