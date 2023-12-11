using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


using WebSocketSharp;

public class Player
{

    public int id;
    // Deck
    // Hand
    // Board
    public int lifePoints;

    // public List<Card> Hand;
    // public List<Card> Deck;
    // public List<Card> Board;

    public WebSocket PlayerSocket;

    public Player(int id, string adresseNgrok = "6ec8-2a02-8440-c201-d671-5460-1f39-d614-7ca8")
    {
        this.id = id;
        // string adresseNgrok = "e740-46-193-3-79";
        PlayerSocket = new WebSocket("wss://" + adresseNgrok + ".ngrok-free.app/" + id);
        PlayerSocket.OnMessage += (sender, e) =>
        {
            UnityEngine.Debug.Log("Message reçu du serveur : " + e.Data);
        };
        PlayerSocket.Connect();
        CreateCard(new Vector3(-2, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(GoldCard));
        CreateCard(new Vector3(0, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(GoldCard));
        CreateCard(new Vector3(2, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(BlueCard));
        CreateCard(new Vector3(4, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(GreenCard));
    }


     public void CreateCard(Vector3 position, Vector3 scale, Type cardType)
    {
        // Créer l'objet de la carte
        GameObject cardObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // Modifier la taille pour en faire une carte
        cardObject.transform.localScale = scale;
        cardObject.transform.position = position;
        // Ajouter le composant de carte du type spécifié
        Card cardComponent = (Card)cardObject.AddComponent(cardType);
        // Initialiser la carte (si vous avez une fonction pour cela dans votre classe Card)
        cardComponent.InitializeCard();
        // Assigner le joueur comme propriétaire de la carte
        cardComponent.owner = this;
    }
    public void SendMessageToTAble(string message)
    {
        if (PlayerSocket != null && PlayerSocket.ReadyState == WebSocketState.Open)
        {
            PlayerSocket.Send(message);
        }
    }
}