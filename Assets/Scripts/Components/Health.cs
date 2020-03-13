﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health

{
    int _hitpoints;
    int _maxHitpoints;
    int REGENERATION_TIME = 12;
    int regenerationTimer = 12;
    Actor _parent;

    public int Hitpoints { get => Mathf.Max(_hitpoints,0); set => _hitpoints = value; }
    public int MaxHitpoints { get => _maxHitpoints; set => _maxHitpoints = value; }

    public Health(int maxHP, Actor parent) {
        _hitpoints = MaxHitpoints = maxHP;
        _parent = parent;
    }

    public void TakeDamage(int damage, string cause) {
        _hitpoints -= damage;
        _parent.killedBy = cause;
        // _parent.Map.Game.Log($"{_parent.Name} takes {damage} damage.");

        //reset the regeneration counter
        regenerationTimer = REGENERATION_TIME;
        if (_hitpoints <= 0 ){ 
            _parent.Die();
        } else if ((_hitpoints < _maxHitpoints * .25) && (_parent.GetType() == typeof(Monster)))
        {
            _parent.Map.Game.Log($"{_parent.Name} is badly hurt.");
        }
    }

    public void Tick() {
        if (!_parent.isAlive) return; // ticker has stopped ;)

        // Tick() is like an turn-based update function

        // heal slowly over time
        if (Hitpoints < MaxHitpoints) {
            regenerationTimer--;
            if (regenerationTimer == 0) {
                Heal(1);
                regenerationTimer = REGENERATION_TIME;
            }
        }
    }

    public void InflictBleeding (int duration) {

    }

    public int Heal (int attemptedAmount) {
        int amount = Mathf.Min(attemptedAmount, (MaxHitpoints - Hitpoints));
        _hitpoints += amount;
        _parent.Map.Game.Log($"{_parent.Name} recovers {amount} health.");
        return amount;
    }

    
}

