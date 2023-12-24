using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Program : MonoBehaviour
{
    string adresseNgrok = "8354-46-193-3-79";
    public Player player;

    void Start()
    {
        int playerID = GameManager.Instance.PlayerID;
        player = new Player(playerID,adresseNgrok);
    }
}

