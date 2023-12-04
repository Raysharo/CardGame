using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Diagnostics;

using WebSocketSharp;


public class Program : MonoBehaviour
{
        private WebSocket webSocket;


    void Start()
    {
        // SocketConnexion();
        createGoldCard();




        //webSocket = new WebSocket("wss://localhost:3000/0");
        // string adresseNgrok = AdresseSingleton.GetSharedValue();
        string adresseNgrok = "e740-46-193-3-79";
        webSocket = new WebSocket("wss://" + adresseNgrok + ".ngrok-free.app/0");
        webSocket.OnMessage += (sender, e) =>
        {
            UnityEngine.Debug.Log("Message reçu du serveur : " + e.Data);
            // Ajoutez ici la logique pour traiter les messages reçus du serveur (éventuellement des messages des joueurs)
        };

        webSocket.Connect();

        // Envoyer un message aux joueurs
        SendMessageToPlayers("Message de la Lucas!");


    }

     // Fonction pour envoyer un message aux joueurs depuis la table
    public void SendMessageToPlayers(string message)
    {
        if (webSocket != null && webSocket.ReadyState == WebSocketState.Open)
        {
            webSocket.Send(message);
        }
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
        // Spécifiez le chemin du fichier script.js
        string cheminDuScript = "Server/server.js";

        // Créez un processus pour exécuter Node.js
        Process processNode = new Process();
        processNode.StartInfo.FileName = "node"; // Assurez-vous que "node" est dans le PATH
        processNode.StartInfo.Arguments = cheminDuScript;
        processNode.StartInfo.RedirectStandardOutput = true;
        processNode.StartInfo.UseShellExecute = false;
        processNode.StartInfo.CreateNoWindow = true;

        // Démarrez le processus
        processNode.Start();    

        // // Obtenez la sortie standard (résultats de l'exécution du script)
        // string resultat = processNode.StandardOutput.ReadToEnd();

        // // Attendez la fin de l'exécution du processus
        // processNode.WaitForExit();

        // // Affichez la sortie du script Node.js
        // Console.WriteLine("Résultat de l'exécution du script Node.js :");
        // Console.WriteLine(resultat);

        // // Fermez le processus
        // processNode.Close();
    }

}
