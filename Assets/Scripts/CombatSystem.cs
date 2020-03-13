using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RogueSharp.DiceNotation;

public class CombatSystem 
{

    public float STRENGTH_BONUS_MULTIPLIER=.111f;

    Game game;

    public CombatSystem(Game game) {
        this.game = game;
    }

    public void Attack(Actor attacker, Actor target) {

        // are both participants target still alive?
        if (!target.isAlive || !attacker.isAlive) return;

        // is the attack evaded?
            if (Evaded(attacker, target)) {
                game.Log($"{target.Name} nimbly evades the attack of {attacker.Name}.");
                return;
            }

        // is the attack blocked?

        string msg = $"{attacker.Name} hits {target.Name} ";
        // was it a critical hit?
        bool crit = (Random.Range(1, 101) < attacker.agility/5);
        if (crit) msg += "<#ff2222>critically</color> ";

        // determine the damage
        int weaponDamage = Dice.Roll(attacker.DamageDice);
        if (crit) weaponDamage += Dice.Roll("2d6"); // crit increases damage before strength and armor are applied


        int damage = Mathf.RoundToInt(attacker.strength*STRENGTH_BONUS_MULTIPLIER) + weaponDamage - target.armor; 

        if (damage <= 0) {
            game.Log ($"{target.Name}'s armor absorbs the force of {attacker.Name}'s blow.");
            return;
        }

        msg += $"for {damage} damage.";
        game.Log(msg);
        // apply the damage
        target.health.TakeDamage(damage, "a " + attacker.Name);

        // if the weapon has any special effects, do them now
        if (attacker.myWeapon != null) attacker.myWeapon.ActivateItem(target);

        // what about inherent monster/player effects?
        // TODO - implement!

        // temp - poison the target!
        target.AddCondition(new PoisonedCondition(3));

        if (target.isAlive == false && attacker== game.Player) {
            game.Player.GainXP(target.XPvalue());
        }
    }

    public bool Evaded(Actor attacker, Actor target) {
        // lighter weapons are harder to evade
        int evasionChance = target.agility;
        if (attacker.myWeapon != null) evasionChance += (2*attacker.myWeapon.Weight);
        return (Random.Range(1,101) < evasionChance); 
    }
}
