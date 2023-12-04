using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCard : Card
{
    int health;
    int attack;
    int color;

    void Start()
    {
        InitializeCard(); // Ajoutez les parenthèses ici
    }
    
    void InitializeCard(){
        base.InitializeCard();
        base.gameObject.name = "MonsterCard";
        this.health = Random.Range(0, 100);
        this.attack = Random.Range(0, 100);
        this.color = Random.Range(0, 100);
        base.rend.material.color = Color.red;
    }

    public MonsterCard(int health, int attack, int color) : base()
    {
        
    }
}
