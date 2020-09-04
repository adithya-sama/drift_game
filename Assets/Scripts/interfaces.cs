using UnityEngine;
public class Grapplelable: MonoBehaviour
{
    protected grapple_chain_handler gc_handler;
    public bool grappled;
    public virtual void attach(grapple_chain_handler handler){
        gc_handler = handler;
        grappled = true;
    }
    public virtual void forced_detach(){
        gc_handler.release();
    }
    public virtual void reset(){
        gc_handler = null;
        grappled = false;
    }
    public virtual void become_ungrapplelable(){
        gameObject.layer = 16;
    }
    public virtual void become_grapplelable(){
        gameObject.layer = 0;
    }
}
public interface Ienemy_with_damage
{
    void Damage(int damage);
}
public interface Ipullable{
    void pull();    
    void set_grapple_handler(grapple_handle_handler handler);
    void grappled();
    void ungrappled();
}