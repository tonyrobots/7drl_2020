using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health

{
    int _hitpoints;
    int _maxHitpoints;
    int bleedingTimer=0;
    int regenerationTime = 10;
    int regenerationCountdown = 10;
    Actor _parent;

    public int Hitpoints { get => _hitpoints; set => _hitpoints = value; }
    public int MaxHitpoints { get => _maxHitpoints; set => _maxHitpoints = value; }

    public Health(int maxHP, Actor parent) {
        _hitpoints = MaxHitpoints = maxHP;
        _parent = parent;
    }

    public void TakeDamage(int damage) {
        _hitpoints -= damage;
        _parent.Map.Game.Log($"{_parent.Name} takes {damage} damage.");
        if (_hitpoints <= 0){
            _parent.Die();
        }
    }

    public void Tick() {
        // like an turn-based update function
        if (bleedingTimer > 0) {
            _parent.Map.Game.Log($"{_parent.Name} is bleeding.");
            TakeDamage(1);
            bleedingTimer--;
        } else {
            _parent.statuses.Remove("Bleeding");

        }

        // heal slowly over time
        if (Hitpoints < MaxHitpoints) {
            regenerationCountdown--;
            if (regenerationCountdown == 0) {
                Heal(1);
                regenerationCountdown = regenerationTime;
            }
        }
    }

    public void InflictBleeding (int duration) {
        bleedingTimer += duration;
        _parent.Map.Game.Log($"{_parent.Name} starts to bleed ({bleedingTimer})");
        _parent.statuses.Add("Bleeding");
    }

    public int Heal (int attemptedAmount) {
        int amount = Mathf.Min(attemptedAmount, (MaxHitpoints - Hitpoints));
        _hitpoints += amount;
        _parent.Map.Game.Log($"You heal by {amount} health.");
        return amount;
    }

    
}

