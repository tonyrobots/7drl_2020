using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon:Item {

    string damageDice;
    public string DamageDice { get => damageDice; set => damageDice = value; }

    public Weapon(Tile startingTile, string symbol, Color color, string name, string damageDice):base(startingTile, symbol, color, name) {
        DamageDice = damageDice;
    }

}
