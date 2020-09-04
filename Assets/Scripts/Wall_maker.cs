using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Wall_maker : MonoBehaviour
{
    List<Vector3> vertices;
    List<Vector2> perimeter_vertices;
    List<int> triangles;
    public int mesh_size_x, mesh_size_y;
    public float mesh_detail, rough_height;
    public bool rough_top, rough_right, rough_bottom, rough_left;
    RectangleMesh mesh_gen;
    void Start()
    {
        mesh_gen = new RectangleMesh(mesh_size_x, mesh_size_y, mesh_detail, rough_height);
        mesh_gen.set_rough_map(rough_top, rough_right, rough_bottom, rough_left);
        mesh_gen.make_mesh();
        GetComponent<MeshFilter>().mesh = mesh_gen.get_mesh();
        mesh_gen.extract_perimeter_vertices();
        GetComponent<PolygonCollider2D>().points = mesh_gen.get_perimeter();
        GetComponent<MeshRenderer>().sortingLayerName = "Default";

    }
}