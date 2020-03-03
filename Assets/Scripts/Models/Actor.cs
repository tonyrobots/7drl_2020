using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : Entity
{

    public bool isAlive = true;

    // TODO Will need to develop this system, probably also using components? Wish I were using unity more. Too late?
    public List<string> statuses = new List<string>();
    
    // components
    public Health health;

    public void Wait()
    {
        // using this callback to represent turn end, probably a smarter way to handle
        if (cbEntityChanged != null) cbEntityChanged(this); // call callbacks        
        Map.Game.Log($"{Name} takes a beat to assess the situation.");

    }

    public virtual void Die() {
        Map.Game.Log($"{Name} dies, theoretically.");
        if (cbEntityChanged != null) cbEntityChanged(this); // call callbacks        
    }

}
