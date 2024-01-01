using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WebSocketSharp;
using UnityEngine.UI;


using WebSocketSharp;
using Unity.VisualScripting;
//using UnityEditor.VersionControl;
//using UnityEditor.PackageManager.Requests;

public class Player
{

    public int id;
    public List<Card> cards = new List<Card>(); // Liste pour suivre les cartes du joueur
    // Deck
    // Hand
    // Board
    public int lifePoints;

    // public List<Card> Hand;
    // public List<Card> Deck;
    // public List<Card> Board;

    public WebSocket PlayerSocket;

    // private Queue<string> messageQueue = new Queue<string>();



    public Player(int id, string adresseNgrok)
    {
        this.id = id;
        PlayerSocket = new WebSocket("wss://" + adresseNgrok + ".ngrok-free.app/" + id);
        PlayerSocket.OnMessage += (sender, e) =>
        {
            UnityEngine.Debug.Log("Message reçu du serveur : " + e.Data);
            try
            {
                // MessageManager messageManager = FindObjectOfType<MessageManager>();
                // if (messageManager == null)
                // {
                //     Debug.LogError("MessageManager n'est pas assigné dans l'inspecteur!");
                //     return;
                // }
                // messageManager.EnqueueMessage(e.Data);
                MessageManager.Instance.EnqueueMessage(e.Data, this);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("Erreur : " + ex.Message);
            }


            // lock (messageQueue)
            // {
            //     messageQueue.Enqueue(e.Data);
            // }



            // UnityEngine.Debug.Log("Message reçu du serveur : " + e.Data);

            // MessageTypeIdentifier messageType = JsonUtility.FromJson<MessageTypeIdentifier>(e.Data);

            // Debug.Log("Type de message reçu : " + messageType.type);

            // switch (messageType.type)
            // {
            //     case "requestCards":
            //         Debug.Log("Cartes demandées par le serveur");
            //         HandleRequestCardsMessage(e.Data);
            //         break;
            //     case "giveCards":
            //         HandleGiveCardsMessage(e.Data);
            //         break;
            //     default:
            //         Debug.LogError("Type de message inconnu.");
            //         break;
            // }
        };
        PlayerSocket.Connect();
        if (id == 1)
        {
            CreateCard(new Vector3(-2, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(GoldCard), 25, 9, false);
            CreateCard(new Vector3(0, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(GoldCard), 29, 14, false);
            CreateCard(new Vector3(2, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(BlueCard), 35, 12, false);
            CreateCard(new Vector3(4, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(GreenCard), 17, 13, false);
        }
        else if (id == 2)
        {
            CreateCard(new Vector3(-2, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(GreenCard), 12, 5, false);
            CreateCard(new Vector3(0, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(GoldCard), 15, 10, false);
            CreateCard(new Vector3(2, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(GreenCard), 30, 10, false);
            CreateCard(new Vector3(4, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(GreenCard), 20, 15, false);
        }
        else if (id == 3)
        {
            CreateCard(new Vector3(-2, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(BlueCard), 17, 13, false);
            CreateCard(new Vector3(0, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(GoldCard), 20, 14, false);
            CreateCard(new Vector3(2, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(BlueCard), 18, 1, false);
            CreateCard(new Vector3(4, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(GreenCard), 11, 5, false);
        }
        else if (id == 4)
        {
            CreateCard(new Vector3(-2, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(GreenCard), 24, 3, false);
            CreateCard(new Vector3(0, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(GoldCard), 16, 11, false);
            CreateCard(new Vector3(2, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(BlueCard), 13, 10, false);
            CreateCard(new Vector3(4, 0, 0), new Vector3(1, 1.5f, 0.1f), typeof(GreenCard), 19, 15, false);
        }

    }

    // void Update()
    // {
    //      Debug.Log("Traitement du message ");
    //     while (messageQueue.Count > 0)
    //     {
    //         string message;
    //         lock (messageQueue)
    //         {
    //             message = messageQueue.Dequeue();
    //         }

    //         Debug.Log("Traitement du message : " + message); 

    //         ProcessMessage(message);
    //     }
    // }

    private void ProcessMessage(string message)
    {
        // Votre logique de traitement des messages...

        try
        {
            UnityEngine.Debug.Log("Message reçu du serveur : " + message);
            MessageTypeIdentifier messageType = JsonUtility.FromJson<MessageTypeIdentifier>(message);

            // ... (le reste de votre logique de traitement de message)
            // Debug.Log("Type de message reçu : " + messageType.type);
            switch (messageType.type)
            {
                case "requestCards":
                    Debug.Log("Cartes demandées par le serveur");
                    HandleRequestCardsMessage(message);
                    break;
                case "giveCards":
                    HandleGiveCardsMessage(message);
                    break;
                default:
                    Debug.LogError("Type de message inconnu.");
                    break;
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("Une erreur est survenue lors du traitement du message : " + ex.Message);
            // Vous pouvez ajouter ici d'autres logiques pour gérer l'erreur si nécessaire
        }

        // UnityEngine.Debug.Log("Message reçu du serveur : " + message);

        // MessageTypeIdentifier messageType = JsonUtility.FromJson<MessageTypeIdentifier>(message);

        // Debug.Log("Type de message reçu : " + messageType.type);
        // switch (messageType.type)
        // {
        //     case "requestCards":
        //         Debug.Log("Cartes demandées par le serveur");
        //         HandleRequestCardsMessage(message);
        //         break;
        //     case "giveCards":
        //         HandleGiveCardsMessage(message);
        //         break;
        //     default:
        //         Debug.LogError("Type de message inconnu.");
        //         break;
        // }
    }


    public void HandleRequestCardsMessage(string message)
    {
        // Convertir la chaîne JSON en objet
        Debug.Log("Message reçu du serveur : " + message);
        RequestInfo requestInfo = JsonUtility.FromJson<RequestInfo>(message);
        Debug.Log("Action reçue : " + requestInfo.action);

        // Vérifier le type d'action
        if (requestInfo.action == "requestCards")
        {
            // Si le joueur ciblé est ce joueur
            if (requestInfo.targetPlayerId == this.id)
            {
                Debug.Log("Cartes demandées par le joueur " + requestInfo.requestingPlayerId);
                // Envoyer les informations sur les cartes
                SendCardInfo(requestInfo.requestingPlayerId, requestInfo.targetPlayerId);
            }
        }
    }

    public void HandleGiveCardsMessage(string message)
    {
        // Convertir la chaîne JSON en objet
        ListCardMessage cardMessage = JsonUtility.FromJson<ListCardMessage>(message);
        int x = -2;
        foreach (var card in cardMessage.cards)
        {
            Debug.Log("Carte trouvée : " + card.attackPoints + "  " + card.defensePoints + "  " + card.idPlayer + "  " + card.cardType);
            Type cardType = Type.GetType(card.cardType);
            CreateCard(new Vector3(x, 1, 0), new Vector3(1, 1.5f, 0.1f), cardType, card.attackPoints, card.defensePoints, true);
            x += 2;
        }
    }

    private void SendCardInfo(int requestingPlayerId, int targetPlayerId)
    {
        Debug.Log("Nombre de cartes trouvées : " + cards.Count);

        Debug.Log("Cartes demandées par le joueur requestingPlayerId " + requestingPlayerId);
        Debug.Log("Cartes envoyées au joueur targetPlayerId " + targetPlayerId);

        List<CardData> cardDataList = new List<CardData>();
        foreach (var card in cards)
        {
            Debug.Log("Carte trouvée : " + card.attackPoints + "  " + card.defensePoints);
            CardData cardData = new CardData(card);
            cardDataList.Add(cardData);
        }

        // how to send liste cards  to player 1 use web socket to send liste cards  to player 1

        if (PlayerSocket != null && PlayerSocket.ReadyState == WebSocketState.Open)
        {
            // Créer un objet CardMessage
            //ListCardMessage cardMessage = new ListCardMessage("giveCards", cards);
            ListCardMessage cardMessage = new ListCardMessage("giveCards", cardDataList, requestingPlayerId, targetPlayerId);
            string message = JsonUtility.ToJson(cardMessage);
            PlayerSocket.Send(message);
        }
    }

    public void RequestCards(int requestingPlayerId, int targetPlayerId)
    {
        if (PlayerSocket != null && PlayerSocket.ReadyState == WebSocketState.Open)
        {
            Debug.Log("Requesting cards from player " + targetPlayerId + " to player " + requestingPlayerId);
            // Créer un objet RequestInfo
            RequestInfo requestInfo = new RequestInfo("requestCards", requestingPlayerId, targetPlayerId);
            string message = JsonUtility.ToJson(requestInfo);
            PlayerSocket.Send(message);
        }
    }

    public void CreateCard(Vector3 position, Vector3 scale, Type cardType, int attackPoints, int defensePoints,bool isGetCard)
    {
        // Créer l'objet de la carte
        GameObject cardObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // Modifier la taille pour en faire une carte
        cardObject.transform.localScale = scale;
        cardObject.transform.position = position;
        // Ajouter le composant de carte du type spécifié
        //cardType.BaseType.GetMethod("InitializeCard").Invoke(cardObject.AddComponent(cardType), new object[] { attackPoints, defensePoints });
        Card cardComponent = (Card)cardObject.AddComponent(cardType);
        // Initialiser la carte (si vous avez une fonction pour cela dans votre classe Card)
        //cardComponent.InitializeCard();
        cardComponent.InitializeCard(attackPoints, defensePoints);
        // Assigner le joueur comme propriétaire de la carte
        string text = $"Attaque: {attackPoints}\nDéfense: {defensePoints}";
        AddTextToCardUI(cardObject, text, new Vector3(0, -0.5f, 0));
        if (isGetCard == false)
        {
            cardComponent.owner = this;
            cardComponent.idPlayer = this.id;
            cards.Add(cardComponent); // Ajoutez la carte à la liste du joueur
        }
    }


    // Fonction pour trouver l'ensemble des cartes presnt sur la scene
    public void FindAllCards()
    {
        // Trouvez toutes les cartes dans la scène
        Card[] allCards = UnityEngine.Object.FindObjectsOfType<Card>();
        foreach (var card in allCards)
        {
            // Vérifiez si la carte appartient à ce joueur
            if (card.owner == this)
            {
                // Ajoutez la carte à la liste du joueur*
                Debug.Log("Carte trouvée : " + card.attackPoints + "  " + card.defensePoints);
                //cards.Add(card);
            }
        }
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

    public void DestroyCard(int cardId)
    {
        // Trouvez toutes les cartes dans la scène
        Card[] allCards = UnityEngine.Object.FindObjectsOfType<Card>();
        foreach (var card in allCards)
        {
            // Vérifiez si l'ID de la carte correspond à celui que nous voulons détruire
            if (card.id == cardId)
            {
                // Détruisez la carte
                UnityEngine.Object.Destroy(card.gameObject);
                break; // Sortez de la boucle si la carte a été trouvée et détruite
            }
        }
    }


}