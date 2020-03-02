using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    int _hitpoints;

    public int Hitpoints { get => _hitpoints; set => _hitpoints = value; }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TakeDamage(int damage) {
        _hitpoints -= damage;
    }

    
}
