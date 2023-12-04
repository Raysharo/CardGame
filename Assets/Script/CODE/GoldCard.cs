using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCard : Card
{
    int gold;

    void Start()
    {
        InitializeCard(); // Ajoutez les parenthèses ici
    }

    void InitializeCard() // Ajoutez les parenthèses ici
    {
        base.InitializeCard();
        this.gold = Random.Range(0, 100);
        base.gameObject.name = "Hello";
        base.id = 44444;
        base.rend.material.color = Color.yellow;
    }
}
