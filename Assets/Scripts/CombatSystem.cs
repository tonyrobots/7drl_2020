using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem 
{
    Game game;

    public CombatSystem(Game game) {
        this.game = game;
    }

    public void Attack(Actor attacker, Actor target) {

        // is the attack evaded?
            if (Evaded(attacker, target)) {
                game.Log($"{target.Name} nimbly evades the attack of {attacker.Name}");
                return;
            }

        // is the attack blocked?

        string msg = $"{attacker.Name} hits {target.Name} ";
        // was it a critical hit?
        bool crit = (Random.Range(1, 101) < attacker.agility/2);
        if (crit) msg += "<#ff2222>critically</color> ";
        // determine the damage
        // TODO incororate weapons
        int weaponDamage = Random.Range(1,11); // this is a shim 
        int damage = Mathf.RoundToInt((attacker.strength)/5) + weaponDamage - target.armor; 
        if (crit) damage *= 2;
        msg += $"for {damage} damage.";
        game.Log(msg);
        // apply the damage
        target.health.TakeDamage(damage);
    }

    public bool Evaded(Actor attacker, Actor target) {
        return (Random.Range(1,101) < target.agility); // need to make this not just a straight agility check, include armor weight?
    }
}
