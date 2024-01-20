using System;
using UnityEngine;
[System.Serializable]
public class CardDataCoequipier
{

    public string type ;
    public int idPlayerCoequipier;
    public int idCard;
    public string cardType;
    public int attackPoints;
    public int defensePoints;
    public string iconCard; 
    public CardDataCoequipier(string type, int idPlayerCoequipier, int idCard, string cardType, int attackPoints, int defensePoints, string iconCard)
    {
        this.type = type;
        this.idPlayerCoequipier = idPlayerCoequipier;
        this.idCard = idCard;
        this.cardType = cardType;
        this.attackPoints = attackPoints;
        this.defensePoints = defensePoints;
        this.iconCard = iconCard;
    }
}