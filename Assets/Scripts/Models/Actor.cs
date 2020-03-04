using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : Entity
{

    public bool isAlive = true;

    //stats
    public int strength = 10;
    public int agility = 10;
    public int armor = 0;
    public int gold = 0;
    public int level = 1;



    // TODO Will need to develop this system, probably also using components? Wish I were using unity more. Too late?
    public List<string> statuses = new List<string>();
    
    // components
    public Health health;

    public void Wait()
    {
        Map.Game.Log($"{Name} takes a beat to assess the situation.");

        // using this callback to represent turn end, there's probably a smarter way to handle
        if (cbEntityChanged != null) cbEntityChanged(this); // call callbacks        

    }

    public virtual void Die() {
        Map.Game.Log($"{Name} dies, theoretically.");
        if (cbEntityChanged != null) cbEntityChanged(this); // call callbacks        
    }

}
