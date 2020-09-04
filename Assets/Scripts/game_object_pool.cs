using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class game_object_pool<T> : MonoBehaviour where T : Component{
    public static game_object_pool<T> instance { get; private set; }
    public T pooled_obj;
    T tmp;
    Queue<T> pool = new Queue<T>();
    void Awake(){
        instance = this;
    }
    public T get(){
        if(pool.Count == 0)
            add(1);
        return pool.Dequeue();
    }
    void add(int n){
        for(int i = 0; i < n; i++){
            tmp = Instantiate(pooled_obj);
            tmp.gameObject.SetActive(false);
            pool.Enqueue(tmp);
        }
    }
    public void return_to_pool(T a){
        a.gameObject.SetActive(false);
        pool.Enqueue(a);
    }
}