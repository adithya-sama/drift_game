using UnityEngine;

public class bullet_1 : MonoBehaviour {
    public float speed, max_existence_time;
    public int damage;
    Rigidbody2D rigid_body;
    GameObject animation_instance;
    float time_watcher;
    Ienemy_with_damage eobj;
    void Awake(){
        rigid_body = gameObject.GetComponent<Rigidbody2D>();
    }
    void OnEnable(){
        time_watcher = 0;
        rigid_body.AddForce(transform.up * speed, ForceMode2D.Impulse);
    }
    void Update(){
        time_watcher += Time.deltaTime;
        if(time_watcher >= max_existence_time){
            bullet_1_pool.instance.return_to_pool(this);
        }
    }
    void OnCollisionEnter2D(Collision2D collision){
        if(collision.collider.tag != "Player"){
            eobj = collision.collider.GetComponent<Ienemy_with_damage>();
            if(eobj != null){
                eobj.Damage(damage);
            }
            animation_instance = bullet_1_explosion_pool.instance.get().gameObject;
            animation_instance.transform.position = transform.position;
            animation_instance.transform.rotation = transform.rotation;
            animation_instance.SetActive(true);

            bullet_1_pool.instance.return_to_pool(this);
        }
    }
}