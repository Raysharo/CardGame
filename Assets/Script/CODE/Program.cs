using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;




public class Program : MonoBehaviour
{
    string adresseNgrok;
    public Player player;

    public TextMeshProUGUI deckCountDisplay;


    void Start()
    {
        int playerID = GameManager.Instance.PlayerID;
        adresseNgrok = NgrokManager.GetAdresseNgrok();
        player = new Player(playerID,adresseNgrok,deckCountDisplay);
    }
}

