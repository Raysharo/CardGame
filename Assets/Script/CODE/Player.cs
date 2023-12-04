using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WebSocketSharp;

public class Player {

    public int id;
    // Deck
    // Hand
    // Board
    public int lifePoints;
    
    // public List<Card> Hand;
    // public List<Card> Deck;
    // public List<Card> Board;

    public WebSocket PlayerSocket;

    Player(int id, string adresseNgrok )
    {
        this.id = id;
        // string adresseNgrok = "e740-46-193-3-79";
        PlayerSocket = new WebSocket("wss://" + adresseNgrok + ".ngrok-free.app/" + id);
        PlayerSocket.OnMessage += (sender, e) =>
        {
            UnityEngine.Debug.Log("Message reçu du serveur : " + e.Data);
        };
        PlayerSocket.Connect();

        
        GameObject goldCard = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // modify size to make it a card
        goldCard.transform.localScale = new Vector3(1, 1.5f, 0.1f);
        // make it 
        goldCard.AddComponent<GoldCard>();
    }

    public void SendMessageToPlayers(string message)
    {
        if (PlayerSocket != null && PlayerSocket.ReadyState == WebSocketState.Open)
        {
            PlayerSocket.Send(message);
        }
    }



}