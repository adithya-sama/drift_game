using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class main_menu_handler : MonoBehaviour
{
    public UnityEvent[] sequence;
    private int sequence_index = 0;

    public void start_next_sequence(){

        Debug.Log("starting next sequence");

        sequence[sequence_index++].Invoke();

    }

    public void load_next_scene(){

        Debug.Log("loading next scene");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }


}