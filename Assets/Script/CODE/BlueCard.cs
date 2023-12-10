using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCard : Card
{
    void Start()
    {
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
