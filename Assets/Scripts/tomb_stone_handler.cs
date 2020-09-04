using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tomb_stone_handler : Grapplelable{
    public override void attach(grapple_chain_handler handler){
        base.attach(handler);
        level_1_handler.instance.tomb_stone_grappled();
    }
    public override void reset(){
        level_1_handler.instance.tomb_stone_ungrappled();
    } 
}
