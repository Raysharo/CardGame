using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCard : Card
{
    void Start()
    {
        InitializeCard();
    }

    void InitializeCard()
    {
        base.color = Color.green;
        // Autres propriétés spécifiques à GreenCard
        
        base.InitializeCard();
        base.rend.material.color = base.color;
        base.gameObject.name = "GreenCard" + base.id;
    }
}
