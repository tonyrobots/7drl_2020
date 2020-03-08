using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon:Item {

    string damageDice;
    public string DamageDice { get => damageDice; set => damageDice = value; }

    int weight;
    public int Weight { get => weight; set => weight = value; }



    public Weapon() : base() {
    }

    public void Initialize(string name, string symbol, Color color, string damageDice, int weight,  ItemEffectFunction itemEffectFunction = null) {
        Name = name;
        Symbol = symbol;
        Color = color;
        DamageDice = damageDice;
        Weight = weight;
        myEffectFunction = itemEffectFunction;
    }

    public void Sharpen() {
        if (myEffectFunction == null) { //only if doesn't already have an effect. In future effects should be a list, and should be set as an attribute rather than ad hoc like this
            Name += " of Sharpness";
            myEffectFunction = (actor, item) => { ItemEffects.Bleeder(actor, item, 5); };
        }
    }

}
