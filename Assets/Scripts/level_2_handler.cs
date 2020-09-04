using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class level_2_handler: MonoBehaviour
{
    public camera_follow_player camera_follow_player_instance;
    public mouse_event_watcher mouse_event_watcher_instance;
    public GameObject player, deploy_minions_animation_obj, minions_obj, key_obj, level_name_obj;
    public big_door_handler left_door, right_door;
    public static level_2_handler instance {get; private set;}
    public GameObject[] key_minion_pointer = new GameObject[4];
    public GameObject[] dialogs;
    int num_of_minions = 4, num_of_open_doors = 0, current_sequence = 0, seq_id;
    Vector2 init_focus;
    bool in_pause = false;
    float time_watcher, pause_time;
    void Awake(){
        if(instance == null){
            instance = this;
        }
        level_name_obj.transform.position = new Vector2(0, player.transform.position.y + Camera.main.orthographicSize + 20);
        Camera.main.transform.position = level_name_obj.transform.position;
        init_focus.x = 0;
        init_focus.y = Camera.main.transform.position.y - 40;
    }
    void Start(){
        start_next_sequence(-1);
    }
    public void FixedUpdate(){
        if(in_pause){
            time_watcher += Time.fixedDeltaTime;
            if(time_watcher >= pause_time){
                pause_done();
            }
        }
    }
    public void deploy_minions(){
        for(int i=0; i<4; i++){
            key_minion_pointer[i].SetActive(true);
        }
        minions_obj.SetActive(true);
    }
    public void minion_killed(){
        num_of_minions -= 1;
        if(num_of_minions == 0){
            start_next_sequence(6);
        }
    }
    public void door_opened(){
        num_of_open_doors++;
        if(num_of_open_doors == 2){
            start_next_sequence(8);
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
    public void start_dialog(int index){
        dialogs[index].SetActive(true);
    }
    public void start_next_sequence(int prev_sequence){
        if(in_pause){return;}
        if(current_sequence == 0 && prev_sequence == -1){
            camera_follow_player_instance.focus_on(init_focus, current_sequence); 
            level_name_obj.SetActive(true);
            current_sequence = 1;
        }else if(current_sequence == 1 && prev_sequence == 0){
            seq_id = current_sequence;
            pause(1);
            current_sequence = 2;
        }else if(current_sequence == 2 && prev_sequence == 1){
            camera_follow_player_instance.reset_focus();
            current_sequence = 3;
        // on door handler detach
        }else if(current_sequence == 3 && prev_sequence == 2){
            camera_follow_player_instance.focus_on(key_obj.transform.position, current_sequence); 
            current_sequence = 4;
        // minion deploy animation end
        }else if(current_sequence == 4 && prev_sequence == 3){
            deploy_minions_animation_obj.SetActive(true);
            current_sequence = 5;
        }else if(current_sequence == 5 && prev_sequence == 4){
            start_dialog(0);
            deploy_minions();
            current_sequence = 6;
        }else if(current_sequence == 6 && prev_sequence == 5){
            camera_follow_player_instance.reset_focus();
            current_sequence = 7;
        }else if(current_sequence == 7 && prev_sequence == 6){
            key_obj.GetComponent<level_2_door_handler>().give_passage();
            current_sequence = 8;
        }else if(current_sequence == 8 && prev_sequence == 7){
            camera_follow_player_instance.focus_on(key_obj.transform.position, -1);
            left_door.open_door();
            right_door.open_door();
            current_sequence = 9;
        }else if(current_sequence == 9 && prev_sequence == 8){
            seq_id = current_sequence;
            pause(2);
            current_sequence = 10;
        }else if(current_sequence == 10 && prev_sequence == 9){
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }
    }
}