using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class level_2_handler: MonoBehaviour
{
    public UnityEvent[] sequence;
    public int sequence_index = 0;
    public bool disable = false;
    [SerializeField]
    public UltEvents.UltEvent on_door_open;

    public void start_next_sequence(int prev_sequence){

        if(disable) return;

        Debug.Log("starting sequence " + sequence_index);

        sequence[sequence_index++].Invoke();

        return;

    }
    public void load_next_level(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void trigger_door_open(){
        on_door_open.Invoke();
    }

}