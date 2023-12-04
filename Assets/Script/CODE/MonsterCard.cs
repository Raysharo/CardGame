using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCard : Card
{
    int health;
    int attack;
    int color;

    public MonsterCard(int health, int attack, int color) : base()
    {
        this.health = health;
        this.attack = attack;
        this.color = color;
        this.rend.material.color = Color.red;  
    }
}
