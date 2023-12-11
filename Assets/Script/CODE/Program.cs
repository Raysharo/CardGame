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

    string adresseNgrok = "6ec8-2a02-8440-c201-d671-5460-1f39-d614-7ca8";

    void Start()
    {
        // LaunchServerJS();
        // createGoldCard();
        player1 = new Player(1,adresseNgrok);
    }
}
