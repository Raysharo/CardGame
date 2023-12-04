    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

using System;
using System.Diagnostics;

using WebSocketSharp;

using Tool;


public class Program : MonoBehaviour
{
    Player player1;
    Player player2;
    Player player3;
    Player player4;

    string adresseNgrok = "e740-46-193-3-79";

    void Start()
    {
        // LaunchServerJS();
        createGoldCard();
    }

    void LaunchServerJS()
    {
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

        // Obtenez la sortie standard (résultats de l'exécution du script)
        string resultat = processNode.StandardOutput.ReadToEnd();

        // Attendez la fin de l'exécution du processus
        processNode.WaitForExit();

        // Affichez la sortie du script Node.js
        Console.WriteLine("Résultat de l'exécution du script Node.js :");
        Console.WriteLine(resultat);

        // // Fermez le processus
        // processNode.Close();
    }

    void createGoldCard()
    {
        // GameObject goldCard = new GameObject("GoldCard");
        // create rectangle
        GameObject goldCard = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // modify size to make it a card
        goldCard.transform.localScale = new Vector3(1, 1.5f, 0.1f);
        // make it 
        goldCard.AddComponent<GoldCard>();
    }

    void createMonsterCard()
    {
        GameObject monsterCard = new GameObject("MonsterCard");
        monsterCard.AddComponent<MonsterCard>();
        // monsterCard.AddComponent<Card>();
    }
}
