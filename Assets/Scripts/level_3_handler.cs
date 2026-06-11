using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class level_3_handler: MonoBehaviour
{
    [SerializeField]
    public UltEvents.UltEvent[] sequence;
    [SerializeField]
    public UltEvents.UltEvent on_all_locks_open;
    public int sequence_index = 0;
    public bool disable = false;

    int num_of_closed_locks = 3;

    public void lock_opened(){
        num_of_closed_locks--;
        if(num_of_closed_locks == 0){
            sequence_index = sequence.Length - 2;
            on_all_locks_open.Invoke();
        }
    }

    public void start_next_sequence(int prev_sequence){

        if(disable) return;

        Debug.Log("starting sequence " + sequence_index);

        sequence[sequence_index++].Invoke();

        return;

    }
    public void load_next_level(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}