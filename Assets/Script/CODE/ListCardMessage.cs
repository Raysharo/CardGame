using System.Collections.Generic; // Ajoutez cette ligne
using UnityEngine;

[System.Serializable]
public class ListCardMessage 
{
    public string action = "requestCards";
   // public List<Card> cards;

    public List<CardData> cards;

    public int requestingPlayerId;
    public int targetPlayerId;

    public ListCardMessage(string action, List<CardData> cards, int requestingPlayerId, int targetPlayerId)
    {
        this.action = action;
        this.cards = cards;
        this.requestingPlayerId = requestingPlayerId;
        this.targetPlayerId = targetPlayerId;
    }
}
