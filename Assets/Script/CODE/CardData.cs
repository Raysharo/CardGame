
using System;
using UnityEngine;
[System.Serializable]
public class CardData
{
    public int id;
    public int attackPoints;
    public int defensePoints;
    public int idPlayer;
    public string cardType;

    public CardData(Card card)
    {
        id = card.id;
        attackPoints = card.attackPoints;
        defensePoints = card.defensePoints;
        idPlayer = card.owner.id;
        cardType = card.GetType().Name;
    }
}
