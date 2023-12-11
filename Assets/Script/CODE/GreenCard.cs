using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCard : Card
{
    void Start()
    {
        attackPoints = 10; 
        defensePoints = 5; 
        InitializeCard();
    }

    void InitializeCard()
    {
        // base.color = Color.green;

        // base.InitializeCard();
        // base.rend.material.color = base.color;
        // base.gameObject.name = "GreenCard" + base.id;

        base.color = Color.green;
        Material newMat = new Material(Shader.Find("Unlit/Color")); // ou un autre shader compatible
        newMat.color = base.color;
        base.rend.material = newMat;

        base.InitializeCard();
        base.gameObject.name = "GreenCard" + base.id;
    }


}
