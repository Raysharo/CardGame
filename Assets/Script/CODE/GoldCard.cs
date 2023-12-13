using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCard : Card
{
    int gold;

    void Start()
    {
        attackPoints = 20; 
        defensePoints = 10; 
        InitializeCard(); // Ajoutez les parenthèses ici
    }

    void InitializeCard() // Ajoutez les parenthèses ici
    {
        base.color = Color.yellow;
        this.gold = Random.Range(0, 100);
        
        
        base.InitializeCard();
        base.rend.material.color = base.color;
        base.gameObject.name = "GoldCard" + base.id;
    }
}
