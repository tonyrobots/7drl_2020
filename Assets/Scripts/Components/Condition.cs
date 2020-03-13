using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Condition {

    protected string name;
    protected string hexcolor;
    protected int duration;
    protected int timer;
    protected Actor parent;


    public string Name { get => name; set => name = value; }
    public Actor Owner { get => parent; set => parent = value; }
    public string Hexcolor { get => hexcolor; set => hexcolor = value; }

    public virtual void Tick() {
        timer -= 1;
        if (timer <= 0) End();


    }

    public virtual void Start() {
        timer = duration;


    }

    public virtual void End(){
        Owner.conditions.Remove(this);
    
    }

    public virtual Condition Compound(Condition preexistingCondition) {
        // by default, when adding the same condition to itself, set the timer to whichever is longest
        if (preexistingCondition.timer > timer) {
            return preexistingCondition;
        } else {
            return this;
        }
    }


}

public class PoisonedCondition:Condition {

    int damagePerTurn = 2;

    public PoisonedCondition(int _duration) {
        Name="Poisoned";
        Hexcolor="#14541c";
        duration = _duration;
        
    }

    public override void Start() {
        Owner.Map.Game.Log($"{Owner.Name} has been poisoned.");
        base.Start();
    }

    public override void End() {
        Owner.Map.Game.Log($"{Owner.Name} is no longer poisoned.");
        base.End();
    }

    public override void Tick() 
    {
        Owner.health.TakeDamage(damagePerTurn, "poison");
        base.Tick();
    }

}

public class BleedingCondition : Condition
{

    int damagePerTurn = 1;

    public BleedingCondition(int _duration)
    {
        Name = "Bleeding";
        Hexcolor = "#dd0000";
        duration = _duration;

    }

    public override void Start()
    {
        Owner.Map.Game.Log($"{Owner.Name} starts to bleed.");
        base.Start();
    }

    public override void End()
    {
        Owner.Map.Game.Log($"{Owner.Name} is no longer bleeding.");
        base.End();
    }

    public override void Tick()
    {
        Owner.health.TakeDamage(damagePerTurn, "loss of blood");
        base.Tick();
    }

}


