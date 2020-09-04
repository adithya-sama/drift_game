using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class level_3_handler: MonoBehaviour
{
    public GameObject player;
    public camera_follow_player camera_follow_player_instance;
    public mouse_event_watcher mouse_Event_Watcher_instance;
    public static level_3_handler instance { get; private set;}
    public GameObject[] enemies, locks, enemy_pointers, circular_lock_active_animations, dialogs;
    public GameObject level_name_obj;
    int num_of_open_locks = 3, current_sequence = -1, seq_id;
    bool in_pause = false;
    float time_watcher, pause_time;
    Vector2 init_focus;
    void Awake(){
        if(instance == null){
            instance = this;
        }
    }
    void Start(){
        level_name_obj.transform.position = new Vector2(0, player.transform.position.y + Camera.main.orthographicSize);
        Camera.main.transform.position = new Vector2(0, level_name_obj.transform.position.y + 20);
        init_focus.x = 0;
        init_focus.y = player.transform.position.y + camera_follow_player_instance.ahead_distance;
        start_next_sequence(0);
    }
    public void FixedUpdate(){
        if(in_pause){
            time_watcher += Time.fixedDeltaTime;
            if(time_watcher >= pause_time){
                pause_done();
            }
        }
    }
    public void pause(float time){
        pause_time = time;
        time_watcher = 0;
        in_pause = true;
    }
    void pause_done(){
        in_pause = false;
        start_next_sequence(seq_id);
    }
    public void lock_closed(){
        Debug.Log(num_of_open_locks);
        num_of_open_locks--;
        if(num_of_open_locks == 0){
            start_next_sequence(14);
        }
    }
    void deploy_enemy(int id){
        enemies[id].SetActive(true);
        enemy_pointers[id].SetActive(true);
    }
    public void start_dialog(int index){
        dialogs[index].SetActive(true);
    }
    public void start_next_sequence(int prev_sequence){
        if(in_pause){return;}
        if(current_sequence == -1 && prev_sequence == 0){
            camera_follow_player_instance.focus_on(init_focus, current_sequence);
            level_name_obj.SetActive(true);
            current_sequence = 0;
        }else if(current_sequence == 0 && prev_sequence == -1){
            camera_follow_player_instance.reset_focus();
            current_sequence = 1;
        }else if(current_sequence == 1 && prev_sequence == 0){
            mouse_Event_Watcher_instance.stop_events();
            start_dialog(0);
            current_sequence = 2;
        }else if(current_sequence == 2 && prev_sequence == 1){
            mouse_Event_Watcher_instance.start_events();
            deploy_enemy(0);
            current_sequence = 3;
        }else if(current_sequence == 3 && prev_sequence == 2){
            camera_follow_player_instance.focus_on(circular_lock_active_animations[0].transform.position, current_sequence, 1);
            current_sequence = 4;
        }else if(current_sequence == 4 && prev_sequence == 3){
            circular_lock_active_animations[0].SetActive(true);
            current_sequence = 5;
        }else if(current_sequence == 5 && prev_sequence == 4){
            camera_follow_player_instance.reset_focus();
            deploy_enemy(1);
            current_sequence = 6;
        }else if(current_sequence == 6 && prev_sequence == 5){
            camera_follow_player_instance.focus_on(circular_lock_active_animations[1].transform.position, current_sequence, 1);
            current_sequence = 7;
        }else if(current_sequence == 7 && prev_sequence == 6){
            circular_lock_active_animations[1].SetActive(true);
            current_sequence = 8;
        }else if(current_sequence == 8 && prev_sequence == 7){
            camera_follow_player_instance.reset_focus();
            deploy_enemy(2);
            current_sequence = 9;
        }else if(current_sequence == 9 && prev_sequence == 8){
            camera_follow_player_instance.focus_on(circular_lock_active_animations[2].transform.position, current_sequence, 1);
            current_sequence = 10;
        }else if(current_sequence == 10 && prev_sequence == 9){
            circular_lock_active_animations[2].SetActive(true);
            current_sequence = 11;
        }else if(current_sequence == 11 && prev_sequence == 10){
            camera_follow_player_instance.focus_on(Vector2.zero, current_sequence, 1);
            current_sequence = 12;
        }else if(current_sequence == 12 && prev_sequence == 11){
            for(int i=0; i<3; i++){
                locks[i].GetComponent<circular_lock_handler>().activate();
            }
            seq_id = current_sequence;
            pause(2);
            current_sequence = 13;
        }else if(current_sequence == 13 && prev_sequence == 12){
            start_dialog(1);
            current_sequence = 14;
        }else if(current_sequence == 14 && prev_sequence == 13){
            camera_follow_player_instance.reset_focus();
            current_sequence = 15;
        }else if(current_sequence == 15 && prev_sequence == 14){
            camera_follow_player_instance.focus_on(Vector2.zero, current_sequence, 1);
            Debug.Log("Done");
        }
    }
}