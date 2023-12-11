using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCard : Card
{
    void Start()
    {
        attackPoints = 5; 
        defensePoints = 15; 
        InitializeCard();
    }

    void InitializeCard()
    {
        base.color = Color.blue;
        // Autres propriétés spécifiques à BlueCard
        
        base.InitializeCard();
        base.rend.material.color = base.color;
        base.gameObject.name = "BlueCard" + base.id;
    }
}
