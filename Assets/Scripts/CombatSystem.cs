using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RogueSharp.DiceNotation;

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
        // TODO incororate weapons, at least for the player:
        int weaponDamage =0;
        if (attacker == game.Player) {
            weaponDamage += Dice.Roll(attacker.myWeapon.DamageDice);
        }
        int damage = Mathf.CeilToInt(attacker.strength/5f) + weaponDamage - target.armor; 

        if (damage <= 0) {
            game.Log ($"{target.Name}'s armor absorbs the force of {attacker.Name}'s blow");
            return;
        }

        if (crit) damage *= 2;
        //msg += $"for {damage} damage.";
        game.Log(msg);
        // apply the damage
        target.health.TakeDamage(damage);
    }

    public bool Evaded(Actor attacker, Actor target) {
        return (Random.Range(1,101) < target.agility); // need to make this not just a straight agility check, include armor weight?
    }
}
