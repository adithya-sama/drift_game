using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pillar_grid_maker : MonoBehaviour
{
    public int pillar_gap, interleave_gap, row_count, column_count, interleave_type;
    public GameObject pillar;
    void Start()
    {
        for (int row = 0; row < row_count; row++)
        {
            for (int col = 0; col < column_count; col++)
            {
                Instantiate(
                    pillar,
                    transform.position + new Vector3(((row % 2) == interleave_type ? 0 : interleave_gap) + (col * pillar_gap), row * pillar_gap, 0),
                    Quaternion.identity,
                    transform
                );
            }
        }
    }
    void OnDrawGizmos()
    {
        float pillar_size = 5;
        pillar_size = (pillar.GetComponent<CircleCollider2D>().radius * pillar.transform.localScale.x) * 2;
        for (int row = 0; row < row_count; row++)
        {
            for (int col = 0; col < column_count; col++)
            {
                Vector2 pos = new Vector3(((row % 2) == interleave_type ? 0 : interleave_gap) + (col * pillar_gap), row * pillar_gap, 0);
                pos = pos + (Vector2)transform.position;
                Gizmos.DrawLine(pos + (Vector2.down * (pillar_size / 2)), pos + (Vector2.up * (pillar_size / 2)));
                Gizmos.DrawLine(pos + (Vector2.left * (pillar_size / 2)), pos + (Vector2.right * (pillar_size / 2)));
            }
        }
    }
}