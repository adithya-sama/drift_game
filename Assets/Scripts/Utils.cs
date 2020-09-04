using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
public static class Utils{
    public static void apply_bounds(ref Vector2 target, Vector2 bounds){
        if(Mathf.Abs(target.x) > bounds.x)
            target.x = Mathf.Sign(target.x) * bounds.x;
        if(Mathf.Abs(target.y) > bounds.y)
            target.y = Mathf.Sign(target.y) * bounds.y;
    }
    public static bool check_bounds(Vector2 target,ref Vector2 bounds){
        if((Mathf.Abs(target.x) > bounds.x) ||(Mathf.Abs(target.y) > bounds.y))
            return true;
        return false;
    }
    public static void move_towards(Transform origin, ref Vector2 target_position, ref float circle_cast_radius, ref int object_avoidance_dist, ref RaycastHit2D[] ray_results, ref LayerMask raycast_layer_mask)
    {
        // The 1 here is a layer mask. which is a bitmap. so 1 means 000001 in binary and 1 is in 0th position. we are only selecting layer 0.
        // if we want only layer 9 then we would use ...01000000000 which in decimal is 512.
        if (Physics2D.CircleCastNonAlloc(origin.position, circle_cast_radius, target_position - (Vector2)origin.position, ray_results, 30, raycast_layer_mask) > 0)
        {
            // getting the direction from current object to object which is hit.
            target_position = (ray_results[0].transform.position - origin.position).normalized;
            // getting the normal to the direction.
            target_position = new Vector2(target_position.y, -target_position.x);
            // adding the normal to the point at which raycast is hit.
            target_position = ray_results[0].point + (target_position * object_avoidance_dist);
        }
    }
    public static float curve_map_1(float from_min, float from_max, float to_min, float to_max, float value, float lateral_param, float tension, float x_offset, int direction)
    {
        //validating inputs;
        if(
            (
                (from_min == 0) &&
                (from_max == 0)
            ) ||
            (
                (direction != -1) &&
                (direction != 1)
            )
        ){
            return 0;
        }
        if(value < from_min){
            if(direction == 1){
                return to_min;
            }else{
                return to_max;
            }
        }
        float res = 0;
        // getting the 0 - 1 range
        float from_range = (1 / (from_max - from_min));

        // converting to 0 - 1 range
        float from_val = (value - from_min) * from_range;

        float x = from_val, y;

        if(direction == -1){
            x = Mathf.Abs(from_val - 1);
        }

        y = 1/(1 + (tension * Mathf.Pow(x + x_offset, lateral_param)));

        res = to_min + (y * (to_max - to_min));

        return res;
    }
    public static float curve_map_linear(float from_min, float from_max, float to_min, float to_max, float value, int direction)
    {

        //validating inputs;

        if(
            (
                (from_min == 0) &&
                (from_max == 0)
            ) ||
            (Mathf.Abs(direction) != 1)
        ){
            return 0;
        }

        if(value < from_min){
            if(direction == 1){
                return to_min;
            }else{
                return to_max;
            }
        }else if(value > from_max){
            if(direction == 1){
                return to_max;
            }else{
                return to_min;
            }
        }

        float res = 0;

        // getting the 0 - 1 range
        float from_range = (1 / (from_max - from_min));

        // converting to 0 - 1 range
        float from_val = (value - from_min) * from_range;

        float x = from_val, y;

        if(direction == -1){
            x = Mathf.Abs(from_val - 1);
        }

        y = x;

        res = to_min + (y * (to_max - to_min));

        return res;
    }
    public static float ToSignedRotation(float angle){
        return (angle > 180) ? (angle - 360) : angle;
    }
    public static Transform findClosest(Transform obj, Transform[] transforms){
        float min_dist = float.MaxValue;
        int min_index = -1;
        for(int i=0; i < transforms.Length; i++){
            if(transforms[i] == null){ continue; }
            float dist = Vector2.Distance(obj.position, transforms[i].position);
            if(dist < min_dist){
                min_dist = dist;
                min_index = i;
            } 
        }
        if(min_index == -1)
            return null;
        return transforms[min_index];
    }
    public static void FitColliderToChildren(GameObject parentObject)
    {
        BoxCollider2D bc = parentObject.GetComponent<BoxCollider2D>();
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        bool hasBounds = false;
        Renderer[] renderers = parentObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (Renderer render in renderers)
        {
            if (hasBounds)
            {
                bounds.Encapsulate(render.bounds);
            }
            else
            {
                bounds = render.bounds;
                hasBounds = true;
            }
        }
        if (hasBounds)
        {
            bc.offset = bounds.center - parentObject.transform.position;
            bc.size = bounds.size;
            //front_bumber_bounds.y = bounds.size.y/2;
        }
        else
        {
            bc.size = bc.offset = Vector3.zero;
            bc.size = Vector3.zero;
        }
    }

}
public class RectangleMesh{
    Mesh var_mesh;
    List<Vector3> vertices;
    List<Vector2> perimeter_vertices;
    List<int> triangles;
    int mesh_size_x, mesh_size_y;
    float mesh_detail, rough_height = 0.3f;
    // indicates which side of the rectangle to make rough.
    // top, right, bottom, left.
    bool[] rough_side_map = {false, false, false, false};
    public Vector3[] get_vertices(){
        return vertices.ToArray();
    }
    public Vector2[] get_perimeter(){
        return perimeter_vertices.ToArray();
    }
    public int[] get_triangles(){
        return triangles.ToArray();
    }
    public Mesh get_mesh(){
        return var_mesh;
    }
    public RectangleMesh(int in_mesh_x, int in_mesh_y, float in_mesh_detail, float in_rough_height){
        mesh_size_x = in_mesh_x;
        mesh_size_y = in_mesh_y;
        mesh_detail = in_mesh_detail;
        rough_height = in_rough_height;
        var_mesh = new Mesh();
        vertices = new List<Vector3>();
        triangles = new List<int>();
        perimeter_vertices = new List<Vector2>();
    }

    public void make_mesh(){
        create_vertices();
        form_triangles();
        var_mesh.Clear();
        var_mesh.vertices = get_vertices();
        var_mesh.triangles = get_triangles();
    }
    public void set_rough_map(bool in_top, bool in_right, bool in_bottom, bool in_left){
        rough_side_map[0] = in_top;
        rough_side_map[1] = in_right;
        rough_side_map[2] = in_bottom;
        rough_side_map[3] = in_left;
    }
    void create_vertices(){
        for(int y=0; y <= mesh_size_y; y++){
            for(int x=0; x <= mesh_size_x; x++){
                float x_noise = 0, y_noise = 0;
                if((rough_side_map[0]) && (y == mesh_size_y)){
                    y_noise = Random.Range(0.0f, rough_height);
                }
                if((rough_side_map[1]) && (x == mesh_size_x)){
                    x_noise = Random.Range(0.0f, rough_height);
                }
                if((rough_side_map[2]) && (y == 0)){
                    y_noise = -Random.Range(0.0f, rough_height);
                }
                if((rough_side_map[3]) && (x == 0)){
                    x_noise = -Random.Range(0.0f, rough_height);
                }
                vertices.Add(new Vector2((x * mesh_detail) + x_noise, (y * mesh_detail) + y_noise));
            }
        }
    }
    public void extract_perimeter_vertices(){
        // order of these is important.
        // perimeter is formed in anti-clockwise.
        if(rough_side_map[2]){
            for(int x=0; x < mesh_size_x; x++){
                perimeter_vertices.Add(vertices[xy_to_index(x, 0)]);
            }
        }else{
            perimeter_vertices.Add(Vector2.zero);
        }
        if(rough_side_map[1]){
            for(int y=0; y < mesh_size_y; y++){
                perimeter_vertices.Add(vertices[xy_to_index(mesh_size_x, y)]);
            }
        }else{
            perimeter_vertices.Add(vertices[xy_to_index(mesh_size_x, 0)]);
        }
        if(rough_side_map[0]){
            for(int x=mesh_size_x; x > 0; x--){
                perimeter_vertices.Add(vertices[xy_to_index(x, mesh_size_y)]);
            }
        }else{
            perimeter_vertices.Add(vertices[xy_to_index(mesh_size_x, mesh_size_y)]);
        }
        if(rough_side_map[3]){
            for(int y=mesh_size_y; y > 0; y--){
                perimeter_vertices.Add(vertices[xy_to_index(0, y)]);
            }
        }else{
            perimeter_vertices.Add(vertices[xy_to_index(0, mesh_size_y)]);
        }
    }
    void form_triangles(){
        for(int y=0; y < mesh_size_y; y++){
            for(int x=0; x < mesh_size_x; x++){
                triangles.Add(xy_to_index(x, y));
                triangles.Add(xy_to_index(x, y + 1));
                triangles.Add(xy_to_index(x + 1, y));
                triangles.Add(xy_to_index(x, y + 1));
                triangles.Add(xy_to_index(x + 1, y + 1));
                triangles.Add(xy_to_index(x + 1, y));
            }
        }
    }
    public int xy_to_index(int x, int y){
        return (y * (mesh_size_x + 1)) + x;
    }
}
public class Instruction{
    public Vector2 target;
    public float angle, speed;
    // angle if positive object turns anti-clockwise. if negative turn clockwise.
    // speed is per second.
    // speed for type 0 is different from type 1, same value will not give same speed.
    public int type;
    // 0 : linear movement (object is moved in a line towards target);
    // 1 : Circular movement (object is rotated aroung the target);
    public Instruction(int in_type, Vector2 in_target, float in_angle, float in_speed){
        type = in_type;
        target = in_target;
        angle = in_angle;
        speed = in_speed;
    }
}
public class InstructionExecutor{
    Instruction current_inst;
    // 0 : ready for next instruction;
    // 1 : instruction still running;
    // 2 : First instruction yet to be executed;
    Transform subject;
    int inst_index = -1, inst_status = 0;
    List<Instruction> inst_list;
    Vector2 current_target;
    float step, remaining_angle = 0;
    public InstructionExecutor(Transform in_transform, List<Instruction> in_inst_list){
        subject = in_transform;
        inst_list = in_inst_list;
    }
    public void set_inst_index(int in_index){
        inst_index = in_index;
    }
    public void set_next_inst(Instruction in_inst){
        current_inst = in_inst;
    }
    public void prepare_next_inst(){
        if(inst_status == 0){
            inst_index = (inst_index + 1) % inst_list.Count;
            current_inst = inst_list[inst_index];
            current_target = current_inst.target;
            step = current_inst.speed * Time.fixedDeltaTime;

            remaining_angle = 0;
            if(current_inst.type == 1){
                remaining_angle = Mathf.Abs(current_inst.angle);
            }
        }
    }
    public void execute_current_inst(){
        inst_status = 1;
        if(current_inst.type == 0){
            subject.position = Vector2.MoveTowards(subject.position, current_target, step);
            if((Vector2)subject.localPosition == current_target){
                inst_status = 0;
            }
        }else if(current_inst.type == 1){
            step *= Mathf.Sign(current_inst.angle);
            subject.RotateAround(current_target, Vector3.forward, step);
            remaining_angle -= Mathf.Abs(step);
            if(remaining_angle <= 0){
                inst_status = 0;
                remaining_angle = 0;
            }
        }
    }
}
[System.Serializable]
public class UnityEventInt: UnityEvent<int>{}