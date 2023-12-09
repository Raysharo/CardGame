using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2
{
    public List<Card2> Deck = new List<Card2>();

    public Card2 PlayCard()
    {
        if (Deck.Count > 0)
        {
            Card2 playedCard = Deck[0];
            Deck.RemoveAt(0);
            return playedCard;
        }

        return null; // ou une carte par défaut si nécessaire
    }
}

