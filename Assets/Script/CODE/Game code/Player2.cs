using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2
{
    public List<Card> Deck = new List<Card>();

    public Card PlayCard()
    {
        if (Deck.Count > 0)
        {
            Card playedCard = Deck[0];
            Deck.RemoveAt(0);
            return playedCard;
        }

        return null; // ou une carte par défaut si nécessaire
    }
}

