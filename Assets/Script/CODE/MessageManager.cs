using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{

    public Player owner;
    public static MessageManager Instance { get; private set; }
    private Queue<string> messageQueue = new Queue<string>();

     void Awake()
    {
        // Vérifier s'il y a déjà une instance de MessageManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ne pas détruire lors du chargement des scènes
        }
        else
        {
            Destroy(gameObject); // S'il y a déjà une instance, détruire ce nouvel objet
        }
    }

    void Update()
    {
        while (messageQueue.Count > 0)
        {
            string message = messageQueue.Dequeue();
            ProcessMessage(message);
        }
    }

    public void EnqueueMessage(string message,Player owner)
    {
        Debug.Log("Message reçu : " + message);
        this.owner = owner;
        messageQueue.Enqueue(message);
    }

    private void ProcessMessage(string message)
    {
        // Traitez le message ici
        Debug.Log("Message traité : " + message);
        // Vous pouvez appeler une fonction de Player ou tout autre gestionnaire ici

          MessageTypeIdentifier messageType = JsonUtility.FromJson<MessageTypeIdentifier>(message);

            Debug.Log("Type de message reçu : " + messageType.type);

            switch (messageType.type)
            {
                case "requestCards":
                    Debug.Log("Cartes demandées par le serveur");
                    owner.HandleRequestCardsMessage(message);
                    break;
                case "giveCards":
                    owner.HandleGiveCardsMessage(message);
                    break;
                case "carteDetruite":
                    owner.HandleCarteDetruiteMessage(message);
                    break;
                    /*
                case "cardSelected":
                    owner.HandleCardSelectedMessage(message);
                    break;
                */
                default:
                    Debug.LogError("Type de message inconnu.");
                    break;
            }


    }
}
