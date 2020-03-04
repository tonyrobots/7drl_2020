using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon:Item {

    string damageDice;
    public string DamageDice { get => damageDice; set => damageDice = value; }

    public Weapon(Map map) : base(map) {
        DamageDice = damageDice;
    }

}
