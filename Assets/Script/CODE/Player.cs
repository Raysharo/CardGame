using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WebSocketSharp;
using UnityEngine.UI;


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
        if (id == 1)
        {
            CreateCard(new Vector3(-2, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(GoldCard)  , 20, 10);
            CreateCard(new Vector3(0, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(GoldCard)   , 20, 10);
            CreateCard(new Vector3(2, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(BlueCard)    , 20, 10);
            CreateCard(new Vector3(4, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(GreenCard)  , 20, 10);
        }
        else
        {
            CreateCard(new Vector3(-2, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(GreenCard), 20, 10);
            CreateCard(new Vector3(0, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(GoldCard),20 , 10  );
            CreateCard(new Vector3(2, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(GreenCard),20 , 10  );
            CreateCard(new Vector3(4, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(GreenCard),20 , 10 );
        }

    }

    public void CreateCard(Vector3 position, Vector3 scale, Type cardType, int attackPoints , int defensePoints)
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
        string text = $"Attaque: {attackPoints}\nDéfense: {defensePoints}";
        AddTextToCardUI(cardObject, text, new Vector3(0, -0.5f, 0));
        cardComponent.owner = this;
    }
    public void SendMessageToTAble(string message)
    {
        if (PlayerSocket != null && PlayerSocket.ReadyState == WebSocketState.Open)
        {
            PlayerSocket.Send(message);
        }
    }

    void AddTextToCardUI(GameObject cardObject, string text, Vector3 localPosition)
    {
        // Créez un nouveau GameObject pour le Canvas s'il n'existe pas déjà sur la carte
        Canvas canvas = cardObject.GetComponentInChildren<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObject = new GameObject("CardCanvas");
            canvasObject.transform.SetParent(cardObject.transform, false);
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            CanvasScaler canvasScaler = canvasObject.AddComponent<CanvasScaler>();
            canvasScaler.scaleFactor = 0.1f; // Ajustez selon la taille de la carte
            canvasScaler.dynamicPixelsPerUnit = 10f;
            canvasObject.AddComponent<GraphicRaycaster>();
            // Ajustez la taille et la position du Canvas pour qu'il corresponde à la carte
            RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();
            canvasRectTransform.sizeDelta = new Vector2(100, 100); // Ajustez cette taille selon vos besoins
            canvasRectTransform.localPosition = new Vector3(0, 0, 0); // Centrez sur la carte
        }

        // Créer l'objet de texte en tant qu'enfant de cardObject
        GameObject textObject = new GameObject("CardTextUI");
        textObject.transform.SetParent(canvas.transform, false);

        RectTransform rectTransform = textObject.AddComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, 0, -1); // Centrez sur la carte

        // Ajouter et configurer le composant de texte
        Text textComponent = textObject.AddComponent<Text>();
        textComponent.text = text;
        textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        textComponent.fontSize = 3;
        textComponent.alignment = TextAnchor.MiddleCenter;
        textComponent.color = Color.black;

        // Ajuster la taille de l'échelle pour rendre le texte proportionnel à la carte
        textObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f); // Ajustez cette échelle selon vos besoins

    }
}