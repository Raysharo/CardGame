using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    public Player2 Player1;
    public Player2 Player2;

    public Team(Player2 player1, Player2 player2)
    {
        Player1 = player1;
        Player2 = player2;
    }

    public int CalculateTotalAttack()
    {
        return Player1.PlayCard().AttackValue + Player2.PlayCard().AttackValue;
    }
}

