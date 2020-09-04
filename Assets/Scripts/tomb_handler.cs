using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class tomb_handler : MonoBehaviour
{
    public GameObject[] scriptures;
    public Color[] ink_colors;
    public int tomb_stone_speed;
    public GameObject tomb_done_animation;
    public UnityEvent on_done;
    int current_scripture = 0;
    bool put_stone_in_place = false, stone_in_place = false;
    Transform tomb_stone;
    tomb_stone_handler ts_handler;
    List<level_1_handler.tomb_color> used_inks = new List<level_1_handler.tomb_color>();
    void FixedUpdate(){
        if(put_stone_in_place){
            tomb_stone.position = Vector2.MoveTowards(tomb_stone.position, transform.position, tomb_stone_speed * Time.fixedDeltaTime);
            if(tomb_stone.position == transform.position){
                level_1_handler.instance.tomb_stone_in_place();
                put_stone_in_place = false;
                stone_in_place = true;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collider){
        if(!put_stone_in_place && !stone_in_place && collider.CompareTag("tomb_stone")){
            level_1_handler.instance.tomb_stone_animation_started();
            tomb_stone = collider.transform;
            Rigidbody2D ts_rb = tomb_stone.GetComponent<Rigidbody2D>();
            ts_rb.isKinematic = true;
            ts_rb.velocity = Vector2.zero;
            ts_handler = tomb_stone.GetComponent<tomb_stone_handler>();
            if(ts_handler.grappled){
                ts_handler.forced_detach();
                ts_handler.become_ungrapplelable();
            }
            put_stone_in_place = true;
        }else if(collider.CompareTag("Player")){
            if((level_1_handler.instance.current_ink != level_1_handler.tomb_color.noop) && !used_inks.Contains(level_1_handler.instance.current_ink)){
                level_1_handler.instance.ink_done((int)level_1_handler.instance.current_ink);
                used_inks.Add(level_1_handler.instance.current_ink);
                activate_ink(level_1_handler.instance.current_ink);
                if(scriptures.Length == used_inks.Count){
                    tomb_done_animation.SetActive(true);
                }
            }
        }
    }
    void activate_ink(level_1_handler.tomb_color ink){
        scriptures[current_scripture].GetComponent<SpriteRenderer>().color = ink_colors[(int)ink];
        scriptures[current_scripture++].SetActive(true);
        level_1_handler.instance.scripture_animation_start();
    }
    public void tomb_done_animation_end(){
        on_done.Invoke();
    }
}