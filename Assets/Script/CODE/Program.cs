using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Program : MonoBehaviour
{
    string adresseNgrok;
    public Player player;


    void Start()
    {

        int playerID = GameManager.Instance.PlayerID;
        adresseNgrok = NgrokManager.GetAdresseNgrok();
        player = new Player(playerID,adresseNgrok);
    }
}

