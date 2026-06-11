using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class level_1_handler : MonoBehaviour
{
    public int sequence_index = 0;
    public bool disable = false;
    public UnityEvent[] sequence;

    public void start_next_sequence(){

        if(disable) return;

        Debug.Log("starting sequence " + sequence_index);

        sequence[sequence_index++].Invoke();

        return;

    }
    public void load_next_level(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}