using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minion_respawn_handler : MonoBehaviour
{
    public Transform protectee, player;
    public GameObject minion;
    public Vector2 level_bounds;
    public Vector2[] init_positions;
    public bool batched_respawn = true;
    public int rounds;
    GameObject[] minions;
    int count = 0, i;
    Vector2 vector2_tmp;
    Dictionary<int, int> minion_to_pos = new Dictionary<int, int>();
    void Awake(){
        minions = new GameObject[init_positions.Length];
        minion_ai m_ai;
        for(i=0; i < init_positions.Length; i++){
            minions[i] = Instantiate(minion);
            minions[i].SetActive(false);
            minions[i].transform.parent = transform;
            minion_to_pos.Add(minions[i].GetInstanceID(), i);
            m_ai = minions[i].GetComponent<minion_ai>();
            m_ai.player = player;
            m_ai.protectee = protectee;
            m_ai.set_respawn_handler(this);
            m_ai.level_bounds = level_bounds;
        }
    }
    void OnEnable(){
        count = rounds * init_positions.Length;
        init_round();
    }
    void init_round(){
        for(i=0; i < init_positions.Length; i++){
            minions[i].transform.position = init_positions[i];
            if(protectee){
                minions[i].transform.position += protectee.position;
            }
            minions[i].SetActive(true);
        }
    }
    void init_minion(GameObject minion){
        minion.transform.position = init_positions[minion_to_pos[minion.GetInstanceID()]];
        if (protectee)
        {
            minion.transform.position += protectee.position;
        }
        minion.SetActive(true);
    }
    public void minion_died(GameObject minion){
        count--;
        if(count > 0){
            if(batched_respawn){
                if((count % init_positions.Length) == 0){
                    init_round();
                }
            }else{
                init_minion(minion);
            }
        }
    }
    void OnDrawGizmos(){
        float size = 5;
        size = (minion.GetComponent<CircleCollider2D>().radius * minion.transform.localScale.x) * 2;
        for (i = 0; i < init_positions.Length; i++)
        {
            vector2_tmp = init_positions[i];
            vector2_tmp += (Vector2)protectee.position;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(vector2_tmp + (Vector2.down * (size / 2)), vector2_tmp + (Vector2.up * (size / 2)));
            Gizmos.DrawLine(vector2_tmp + (Vector2.left * (size / 2)), vector2_tmp + (Vector2.right * (size / 2)));
        }
    }
}