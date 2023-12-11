using System.Collections;
using UnityEngine;
using WebSocketSharp;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class TableManager : MonoBehaviour
{
    private WebSocket ws;
    private Queue<Action> actionsToExecuteOnMainThread = new Queue<Action>();

    void Start()
    {
        string adresseNgrok = "6ec8-2a02-8440-c201-d671-5460-1f39-d614-7ca8";
        // Initialiser la connexion WebSocket
        ws = new WebSocket("wss://" + adresseNgrok + ".ngrok-free.app/0");

        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message reçu: " + e.Data);
            HandleCardMessage(e.Data);
        };

        ws.Connect();
    }

    void Update()
    {
        while (actionsToExecuteOnMainThread.Count > 0)
        {
            actionsToExecuteOnMainThread.Dequeue().Invoke();
        }
    }

    void HandleCardMessage(string data)
    {
        Debug.Log("HandleCardMessage - Données brutes reçues: " + data);

        // Tentez de parser le message JSON reçu
        CardMessage message;
        try
        {
            message = JsonUtility.FromJson<CardMessage>(data);
            Debug.Log($"Message parsé - Type: {message.type}, CardId: {message.cardId}, cardType: {message.cardType}, attackPoints: {message.attackPoints}, defensePoints: {message.defensePoints}");
        }
        catch (Exception e)
        {
            Debug.LogError("Erreur lors du parsing JSON: " + e.Message);
            return;
        }

        if (message.type == "move card to player")
        {
            Debug.Log($"Traitement du message - Déplacement de la carte ID {message.cardId} vers la table.");
            // Créer ou mettre à jour la carte sur la table
            Type cardType = Type.GetType(message.cardType); // Convertissez le nom du type en Type
            int attackPoints = message.attackPoints;
            int defensePoints = message.defensePoints;
            if (cardType != null)
            {
                CreateOrUpdateCardOnTable(message.cardId, cardType, attackPoints, defensePoints);
            }
            else
            {
                Debug.LogError("Type de carte non pris en charge: " + message.cardType);
            }
        }
    }



    // Assurez-vous de fermer la connexion WebSocket lors de la destruction de l'objet
    void OnDestroy()
    {
        if (ws != null)
        {
            ws.Close();
        }
    }

    void CreateOrUpdateCardOnTable(int cardId, Type cardType, int attackPoints, int defensePoints)
    {
        Debug.Log("CreateOrUpdateCardOnTable - Création de la carte ID " + cardId);
        Debug.Log("CreateOrUpdateCardOnTable - attackPoints " + attackPoints);
        Debug.Log("CreateOrUpdateCardOnTable - defensePoints " + defensePoints);
        string text = $"Attaque: {attackPoints}\nDéfense: {defensePoints}";
        CreateCard(new Vector3(2, 0, 0), new Vector3(1, 1.5f, 0.1f), cardType, attackPoints, defensePoints, text);
    }
    // void AddTextToCardUI(GameObject cardObject, string text, Vector2 position)
    // {
    //     GameObject textObject = new GameObject("CardTextUI");
    //     textObject.transform.SetParent(cardObject.transform, false);

    //     RectTransform rectTransform = textObject.AddComponent<RectTransform>();
    //     rectTransform.anchoredPosition = position;

    //     Text textComponent = textObject.AddComponent<Text>();
    //     textComponent.text = text;
    //     textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); // Modifier ici
    //     // Autres réglages de style comme la taille, la couleur, l'alignement, etc.
    // }

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

        // Configurer RectTransform pour le texte
        // RectTransform rectTransform = textObject.AddComponent<RectTransform>();
        // rectTransform.anchoredPosition3D = localPosition; // Utilisez localPosition pour positionner le texte sur la carte
        // rectTransform.sizeDelta = new Vector2(100, 50); // Taille du texte, ajustez selon vos besoins

        // // Ajouter et configurer le composant de texte
        // Text textComponent = textObject.AddComponent<Text>();
        // textComponent.text = text;
        // textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        // textComponent.fontSize = 1;
        // textComponent.alignment = TextAnchor.MiddleCenter;
        // textComponent.color = Color.black;

        // // chenge scale to 0.16
        // textObject.transform.localScale = new Vector3(0.16f, 0.16f, 0.16f);
        // // change sorting order to 1
        // textComponent.canvas.sortingOrder = 1;
        // //CalculerCouleurOpposee(Color.black);
        // Configurer RectTransform pour le texte
        RectTransform rectTransform = textObject.AddComponent<RectTransform>();
        // Ajustez localPosition pour que le texte soit devant la carte, pas dessus
        //rectTransform.anchoredPosition3D = new Vector3(localPosition.x, localPosition.y, localPosition.z - 0.01f); // Déplacez légèrement vers la caméra
        //rectTransform.sizeDelta = new Vector2(100, 50); // Taille du texte
        //modified z position to -1
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

    Color CalculerCouleurOpposee(Color couleurOriginale)
    {
        // Calculer la couleur opposée pour chaque composante RVB
        Color couleurOpposee = new Color(1.0f - couleurOriginale.r, 1.0f - couleurOriginale.g, 1.0f - couleurOriginale.b);

        return couleurOpposee;
    }

    void CreateCard(Vector3 position, Vector3 scale, Type cardType, int attackPoints, int defensePoints, string text)
    {
        actionsToExecuteOnMainThread.Enqueue(() =>
        {
            try
            {
                Debug.Log("CreateCard - Tentative de création de la carte");
                // Créer l'objet de la carte
                GameObject cardObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                // Modifier la taille pour en faire une carte
                cardObject.transform.localScale = scale;
                cardObject.transform.position = position;
                // Ajouter le composant de carte du type spécifié
                Card cardComponent = (Card)cardObject.AddComponent(cardType);
                cardComponent.attackPoints = attackPoints;
                cardComponent.defensePoints = defensePoints;
                // Initialiser la carte
                cardComponent.InitializeCard();
                AddTextToCardUI(cardObject, text, new Vector3(0, -0.5f, 0));
                Debug.Log("CreateCard - Carte créée avec succès");
            }
            catch (Exception e)
            {
                Debug.LogError("CreateCard - Erreur lors de la création de la carte: " + e.Message);
            }
        });
    }
}
