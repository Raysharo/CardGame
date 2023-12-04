using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Program : MonoBehaviour
{
    private WebSocket webSocket;

    void Start()
    {
        SocketConnexion();
       createGoldCard();

    }

    void createGoldCard()
    {
        // GameObject goldCard = new GameObject("GoldCard");
        // create rectangle
        GameObject goldCard = GameObject.CreatePrimitive(PrimitiveType.Cube);



        goldCard.AddComponent<GoldCard>();



    }

    void createMonsterCard()
    {
        GameObject monsterCard = new GameObject("MonsterCard");
        monsterCard.AddComponent<MonsterCard>();
        // monsterCard.AddComponent<Card>();
    }

    void SocketConnexion(){
        //webSocket = new WebSocket("wss://localhost:3000/0");
        string adresseNgrok = AdresseSingleton.GetSharedValue();
        webSocket = new WebSocket("wss://" + adresseNgrok + ".ngrok-free.app/0");
        webSocket.OnMessage += (sender, e) =>
        {
            Debug.Log("Message reçu du serveur : " + e.Data);
            // Ajoutez ici la logique pour traiter les messages reçus du serveur (éventuellement des messages des joueurs)
        };

        webSocket.Connect();
    }

}
