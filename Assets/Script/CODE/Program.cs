using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Program : MonoBehaviour
{
   
    void Start()
    {
       createGoldCard();

    }

    void createGoldCard()
    {
        // GameObject goldCard = new GameObject("GoldCard");
        // create rectangle
        GameObject goldCard = GameObject.CreatePrimitive(PrimitiveType.Cube);



        goldCard.AddComponent<GoldCard>();



    }

    void createMonsterCard()
    {
        GameObject monsterCard = new GameObject("MonsterCard");
        monsterCard.AddComponent<MonsterCard>();
        // monsterCard.AddComponent<Card>();
    }

}
