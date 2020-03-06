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

}
