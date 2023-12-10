    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

using System;
using System.Diagnostics;

using WebSocketSharp;

using Tool;


public class Program : MonoBehaviour
{
    public Player player1;
    public Player player2;
    public Player player3;
    public Player player4;

    string adresseNgrok = "d1d0-46-193-3-79";

    void Start()
    {
        // LaunchServerJS();
        // createGoldCard();
        player1 = new Player(1,adresseNgrok);
    }
}
