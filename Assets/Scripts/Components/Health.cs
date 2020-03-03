using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health

{
    int _hitpoints;
    int _maxHitpoints;
    int bleedingTimer=0;
    Actor _parent;

    public int Hitpoints { get => _hitpoints; set => _hitpoints = value; }
    public int MaxHitpoints { get => _maxHitpoints; set => _maxHitpoints = value; }

    public Health(int maxHP, Actor parent) {
        _hitpoints = MaxHitpoints = maxHP;
        _parent = parent;
    }

    public void TakeDamage(int damage) {
        _hitpoints -= damage;
        _parent.Map.Game.Log($"{_parent.Name} takes {damage} damage, and has {_hitpoints} hitpoints left.");
        if (_hitpoints <= 0){
            _parent.Die();
        }
    }

    public void Tick() {
        // like an turn-based update function
        if (bleedingTimer > 0) {
            _parent.Map.Game.Log($"{_parent.Name} is bleeding...");
            TakeDamage(1);
            bleedingTimer--;
        }
    }

    public void InflictBleeding (int duration) {
        bleedingTimer += duration;
        _parent.Map.Game.Log($"{_parent.Name} starts to bleed ({bleedingTimer})");
    }

    
}

