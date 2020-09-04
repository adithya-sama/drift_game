using UnityEngine;
using UnityEngine.SceneManagement;
public class level_1_handler : MonoBehaviour
{
    public int skip_intro;
    public static level_1_handler instance { get; private set;}
    public camera_follow_player camera_follow_player_instance;
    public mouse_event_watcher mouse_event_watcher_instance;
    public steering_handler steering_handler_instance;
    public GameObject player, tomb, tomb_stone, tomb_done_animation, tomb_pointer, tomb_stone_pointer, tut_acc;
    public GameObject[] idh, ipi, idh_pointers, ip_pointers, dialogs; 
    public Vector2 init_focus, level_name_pos;
    public GameObject level_name_obj;
    bool[] inks_opened = new bool[3]{false, false, false}, inks_done = new bool[3]{false, false, false};
    int current_sequence = 0, seq_id;
    float time_watcher, pause_time;
    bool in_pause = false, first_ink = true;
    public enum tomb_color
    {
        red, green, blue, noop
    }
    public tomb_color current_ink;
    void Awake(){
        if(instance == null){
            instance = this;
            current_ink = tomb_color.noop;
        }
        if(skip_intro == 1){
            tomb_stone_ungrappled();
            current_sequence = 9;
        }
        init_focus.x = 0;
        init_focus.y = player.transform.position.y + Camera.main.orthographicSize + 20;
        level_name_obj.transform.position = new Vector3(0, init_focus.y - 20, 0);
        level_name_pos = player.transform.position;
        level_name_pos.y += 20;
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
    public void set_current_ink(tomb_color col)
    {
        current_ink = col;
        for (int i = 0; i < 3; i++)
        {
            if ((int)col == i)
            {
                if(ip_pointers[i].activeSelf){
                    ip_pointers[i].SetActive(false);
                }
            }
            else if (inks_opened[i] && !inks_done[i])
            {
                if (!ip_pointers[i].activeSelf)
                {
                    ip_pointers[i].SetActive(true);
                }
            }
        }
        if (!tomb_pointer.activeSelf)
        {
            tomb_pointer.SetActive(true);
        }
    }
    public void ink_done(int id){
        inks_done[id] = true;
        ip_pointers[id].SetActive(false);
        tomb_pointer.SetActive(false);
    }
    public void tomb_stone_grappled(){
        tomb_pointer.SetActive(true);
        if(tomb_stone_pointer.activeSelf){
            tomb_stone_pointer.SetActive(false);
        }
    }
    public void tomb_stone_ungrappled(){
        tomb_stone_pointer.SetActive(true);
        if(tomb_pointer.activeSelf){
            tomb_pointer.SetActive(false);
        }
    }
    public void tomb_stone_animation_started(){
        camera_follow_player_instance.focus_on(tomb.transform.position, -1);
    }
    public void tomb_stone_in_place(){
        if(tomb_stone_pointer.activeSelf){
            tomb_stone_pointer.SetActive(false);
        }
        for(int i=0; i<3; i++){
            idh[i].GetComponent<grapple_handle_handler>().become_grapplelable();
            idh_pointers[i].SetActive(true);
        }
        camera_follow_player_instance.reset_focus();
    }
    public void ink_dispencer_opened(int id){
        if(inks_opened[id]){return;}
        idh_pointers[id].SetActive(false);
        ip_pointers[id].SetActive(true);
        ipi[id].SetActive(true);
        inks_opened[id] = true;
        camera_follow_player_instance.focus_on(ipi[id].transform.position, -1);
    }
    public void ink_plate_animation_end(){
        camera_follow_player_instance.reset_focus();
        if(first_ink){
            first_ink = false;
            start_next_sequence(12);
        }
    }
    public void scripture_animation_start(){
        camera_follow_player_instance.focus_on(tomb.transform.position, -1);
    }
    public void scripture_animation_end(){
        camera_follow_player_instance.reset_focus();
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
            camera_follow_player_instance.focus_on(init_focus, current_sequence, Time.fixedDeltaTime);
            current_sequence = 1;
        }else if(current_sequence == 1 && prev_sequence == 0){
            seq_id = current_sequence;
            pause(1);
            current_sequence = 2;
        }else if(current_sequence == 2 && prev_sequence == 1){
            start_dialog(0);
            current_sequence = 3;
        }else if(current_sequence == 3 && prev_sequence == 2){
            level_name_obj.SetActive(true);
            camera_follow_player_instance.focus_on(level_name_pos, current_sequence);
            current_sequence = 4;
        }else if(current_sequence == 4 && prev_sequence == 3){
            seq_id = current_sequence;
            pause(1.5f);
            current_sequence = 18;
        }else if(current_sequence == 18 && prev_sequence == 4){
            start_dialog(7);
            current_sequence = 19;
        }else if(current_sequence == 19 && prev_sequence == 18){
            camera_follow_player_instance.reset_focus();
            tut_acc.SetActive(true);
            steering_handler_instance.disable_steering();
            current_sequence = 20;
        }else if(current_sequence == 20 && prev_sequence == 19){
            steering_handler_instance.enable_steering();
            mouse_event_watcher_instance.stop_events();
            start_dialog(8);
            current_sequence = 21;
        }else if(current_sequence == 21 && prev_sequence == 20){
            mouse_event_watcher_instance.start_events();
            seq_id = 4;
            pause(5);
            current_sequence = 5;
        }else if(current_sequence == 5 && prev_sequence == 4){
            mouse_event_watcher_instance.stop_events();
            start_dialog(1);
            current_sequence = 6;
        }else if(current_sequence == 6 && prev_sequence == 5){
            tomb_stone_ungrappled();
            current_sequence = 7;
        }else if(current_sequence == 7 && prev_sequence == 6){
            start_dialog(2);
            current_sequence = 8;
        }else if(current_sequence == 8 && prev_sequence == 7){
            mouse_event_watcher_instance.start_events();
            current_sequence = 9;
        }else if(current_sequence == 9 && prev_sequence == 8){
            mouse_event_watcher_instance.stop_events();
            start_dialog(3);
            current_sequence = 10;
        }else if(current_sequence == 10 && prev_sequence == 9){
            mouse_event_watcher_instance.start_events();
            current_sequence = 11;
        }else if(current_sequence == 11 && prev_sequence == 10){
            mouse_event_watcher_instance.stop_events();
            start_dialog(4);
            current_sequence = 12;
        }else if(current_sequence == 12 && prev_sequence == 11){
            mouse_event_watcher_instance.start_events();
            current_sequence = 13;
        }else if(current_sequence == 13 && prev_sequence == 12){
            mouse_event_watcher_instance.stop_events();
            start_dialog(5);
            current_sequence = 14;
        }else if(current_sequence == 14 && prev_sequence == 13){
            mouse_event_watcher_instance.start_events();
            current_sequence = 15;
        }else if(current_sequence == 15 && prev_sequence == 14){
            start_dialog(6);
            current_sequence = 16;
        }else if(current_sequence == 16 && prev_sequence == 15){
            SceneManager.LoadScene(1, LoadSceneMode.Single);
            current_sequence = 17;
        }
    }
}