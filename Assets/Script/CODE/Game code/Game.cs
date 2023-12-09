using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    Team Team1;
    Team Team2;

    public Game(Team team1, Team team2)
    {
        Team1 = team1;
        Team2 = team2;
    }

    public void PlayRound()
    {
        int team1Attack = Team1.CalculateTotalAttack();
        int team2Attack = Team2.CalculateTotalAttack();

        if (team1Attack > team2Attack)
        {
            // L'équipe 1 gagne le round
        }
        else if (team2Attack > team1Attack)
        {
            // L'équipe 2 gagne le round
        }
        else
        {
            // Égalité
        }
    }
}

