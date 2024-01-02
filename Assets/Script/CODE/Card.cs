using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;

// using Program;


public abstract class Card : MonoBehaviour
{
    // ATTRIBUTS
    public int id;
    public Vector3 offset;

    // OBJECT
    public Renderer rend;
    // public BoxCollider2D collid;
    public BoxCollider collid;

    public Program Program;
    public bool onInteraction = false;

    private float boxScaleFactorX = 1.5f;
    private float boxScaleFactorY = 1.5f;
    public Color color;
    Light pointLight;
    private TableManager tableManager;


    private float clickDuration = 0.3f; // Seuil de durée pour un clic bref en secondes
    private float mouseDownTime;

    public Player owner;
    public int attackPoints;
    public int defensePoints;
    public int idPlayer;
    public string iconCard;
    private int currentZonePlayerId = -1; // -1  la carte n'est dans aucune zone

    private Vector2 topLeftPlayer1 = new Vector2(-4.3f, -4.68f);
    private Vector2 topRightPlayer1 = new Vector2(4.3f, -4.68f);
    private Vector2 bottomLeftPlayer1 = new Vector2(-4.3f, -1.0f);
    private Vector2 bottomRightPlayer1 = new Vector2(4.3f, -2.27f);

    private Vector2 topLeftPlayer2 = new Vector2(-10.0f, -2.54f);
    private Vector2 topRightPlayer2 = new Vector2(-4.3f, 2.68f);
    private Vector2 bottomLeftPlayer2 = new Vector2(-10.0f, -7.0f);
    private Vector2 bottomRightPlayer2 = new Vector2(-4.3f, -6.27f);

    private Vector2 topLeftPlayer3 = new Vector2(-4.3f, 0.68f);
    private Vector2 topRightPlayer3 = new Vector2(4.3f, 0.68f);
    private Vector2 bottomLeftPlayer3 = new Vector2(-4.3f, -8.27f);
    private Vector2 bottomRightPlayer3 = new Vector2(4.3f, -8.27f);

    private Vector2 topLeftPlayer4 = new Vector2(4.3f, -2.54f);
    private Vector2 topRightPlayer4 = new Vector2(11.0f, 2.68f);
    private Vector2 bottomLeftPlayer4 = new Vector2(4.3f, -7.0f);
    private Vector2 bottomRightPlayer4 = new Vector2(11.0f, -6.27f);



    private Rect playerZone1;
    private Rect playerZone2;
    private Rect playerZone3;
    private Rect playerZone4;

    void Start()
    {
        Program = GameObject.Find("Program").GetComponent<Program>();

    }


    public void InitializeCard(int attackPoints,int defensePoints)
    {
        // Récupère la référence à l'objet auquel ce script est attaché
        this.id = UnityEngine.Random.Range(0, 1000000);
        // Utilise gameObject.AddComponent pour ajouter des composants au GameObject
        this.attackPoints = attackPoints;
        this.defensePoints = defensePoints;

        this.rend = gameObject.GetComponent<Renderer>();

        // Stocke une référence au BoxCollider
        this.collid = gameObject.GetComponent<BoxCollider>();

        this.collid.size = new Vector2(transform.localScale.x * boxScaleFactorX, transform.localScale.y * boxScaleFactorY);

    }
    
    void Update()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
       

        if (transform.position.y != 0 && !onInteraction)
        {
            if (currentSceneName != "TableScene")
            {
                // if(this.idPlayer !=){
                    
                // }
                // StartCoroutine(ResetPosition());
            }
        }
        CheckAndAdjustPosition();

    }

    void TaskOnClik()
    {
        Debug.Log("TaskOnClik");
        CardMessage messageObject = new CardMessage(this.id, this.GetType().Name, attackPoints, defensePoints, this.iconCard);
        string message = JsonUtility.ToJson(messageObject);
        owner.SendMessageToTAble(message);
        owner.DestroyCard(this.id);

    }

    void OnMouseDown()
    {
        // Debug.Log("OnMouseDown");

        onInteraction = true;
        mouseDownTime = Time.time;

        // Change color to red when clicked
        rend.material.color = Color.red;
        // Calculez l'offset entre la position initiale et la position actuelle du clic
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
    }


    void OnDrawGizmos()
    {
        Vector3 bottomLeft = new Vector3(playerZone4.x, playerZone4.y, 0);
        Vector3 topLeft = new Vector3(playerZone4.x, playerZone4.y + playerZone4.height, 0);
        Vector3 topRight = new Vector3(playerZone4.x + playerZone4.width, playerZone4.y + playerZone4.height, 0);
        Vector3 bottomRight = new Vector3(playerZone4.x + playerZone4.width, playerZone4.y, 0);

        Debug.DrawLine(bottomLeft, topLeft, Color.green);
        Debug.DrawLine(topLeft, topRight, Color.green);
        Debug.DrawLine(topRight, bottomRight, Color.green);
        Debug.DrawLine(bottomRight, bottomLeft, Color.green);

    }

    void OnMouseUp()
    {
        // Debug.Log("OnMouseUp");
        onInteraction = false;
        rend.material.color = this.color;
        float clickDuration = Time.time - mouseDownTime;

        if (SceneManager.GetActiveScene().name == "TableScene")
        {
            tableManager = GameObject.Find("GameObject").GetComponent<TableManager>();
            if (tableManager == null)
            {
                Debug.LogError("TableManager not found in the scene!");
            }
            else
            {

                playerZone1 = new Rect(
                            topLeftPlayer1.x,
                            topLeftPlayer1.y,
                            Mathf.Abs(bottomRightPlayer1.x - topLeftPlayer1.x), // width est la différence absolue en x
                            Mathf.Abs(topLeftPlayer1.y - bottomLeftPlayer1.y) // height est la différence absolue en y
                                );

                playerZone2 = new Rect(
                            topLeftPlayer2.x,
                            topLeftPlayer2.y,
                            Mathf.Abs(bottomRightPlayer2.x - topLeftPlayer2.x), // width est la différence absolue en x
                            Mathf.Abs(topLeftPlayer2.y - bottomLeftPlayer2.y) // height est la différence absolue en y
                                );

                playerZone3 = new Rect(
                            topLeftPlayer3.x,
                            topLeftPlayer3.y,
                            Mathf.Abs(bottomRightPlayer3.x - topLeftPlayer3.x), // width est la différence absolue en x
                            Mathf.Abs(topLeftPlayer3.y - bottomLeftPlayer3.y - 4.5f) // height est la différence absolue en y
                                );

                playerZone4 = new Rect(
                            topLeftPlayer4.x,
                            topLeftPlayer4.y,
                            Mathf.Abs(bottomRightPlayer4.x - topLeftPlayer4.x), // width est la différence absolue en x
                            Mathf.Abs(topLeftPlayer4.y - bottomLeftPlayer4.y) // height est la différence absolue en y
                                );
                OnDrawGizmos();
                CheckZonesForCards();
                Vector2 cardPosition2D = new Vector2(this.transform.position.x, this.transform.position.y);
                int attackPoints = 0;
                if (playerZone1.Contains(cardPosition2D))
                {
                    Debug.Log("Carte levée dans la zone du joueur " + currentZonePlayerId);
                    // Vous pouvez appeler ici la fonction pour attaquer ou autre action
                    Debug.Log("TableManager found: " + tableManager.gameObject.name);
                    if (this.idPlayer != 1 && this.idPlayer != 3)
                    {
                        Debug.Log("Carte  peut attaque  ");
                        attackPoints = this.attackPoints - PointDeffenceCardsInZone(playerZone1, this.idPlayer);
                        Debug.Log("attackPoints" + attackPoints);
                        tableManager.AttackPlayer(this.idPlayer, 1, attackPoints);
                        //tableManager.UpdatePositionCards(this.idPlayer);
                        tableManager.NextPlayerTurn();
                    }
                }
                else if (playerZone2.Contains(cardPosition2D))
                {
                    Debug.Log("Carte levée dans la zone du joueur " + currentZonePlayerId);
                    // Vous pouvez appeler ici la fonction pour attaquer ou autre action
                    Debug.Log("TableManager found: " + tableManager.gameObject.name);
                    if (this.idPlayer != 2 && this.idPlayer != 4)
                    {
                        Debug.Log("Carte  peut attaque  ");
                        attackPoints = this.attackPoints - PointDeffenceCardsInZone(playerZone2, this.idPlayer);
                        Debug.Log("attackPoints" + attackPoints);
                        tableManager.AttackPlayer(this.idPlayer, 2, attackPoints);
                        //tableManager.UpdatePositionCards(this.idPlayer);
                        tableManager.NextPlayerTurn();
                    }
                }
                else if (playerZone3.Contains(cardPosition2D))
                {
                    Debug.Log("Carte levée dans la zone du joueur " + currentZonePlayerId);
                    // Vous pouvez appeler ici la fonction pour attaquer ou autre action
                    Debug.Log("TableManager found: " + tableManager.gameObject.name);
                    if (this.idPlayer != 1 && this.idPlayer != 3)
                    {
                        Debug.Log("Carte  peut attaque  ");
                        attackPoints = this.attackPoints - PointDeffenceCardsInZone(playerZone3, this.idPlayer);
                        Debug.Log("attackPoints" + attackPoints);
                        tableManager.AttackPlayer(this.idPlayer, 3, attackPoints);
                        //tableManager.UpdatePositionCards(this.idPlayer);
                        tableManager.NextPlayerTurn();
                    }
                }
                else if (playerZone4.Contains(cardPosition2D))
                {
                    Debug.Log("Carte levée dans la zone du joueur " + currentZonePlayerId);
                    // Vous pouvez appeler ici la fonction pour attaquer ou autre action
                    Debug.Log("TableManager found: " + tableManager.gameObject.name);
                    if (this.idPlayer != 2 && this.idPlayer != 4)
                    {
                        Debug.Log("Carte  peut attaque  ");
                        attackPoints = this.attackPoints - PointDeffenceCardsInZone(playerZone4, this.idPlayer);
                        Debug.Log("attackPoints" + attackPoints);
                        tableManager.AttackPlayer(this.idPlayer, 4, attackPoints);
                        //tableManager.UpdatePositionCards(this.idPlayer);
                        tableManager.NextPlayerTurn();
                    }
                }
                else
                {
                    Debug.Log("Carte levée dans aucune zone");
                }

                tableManager.UpdatePositionCards(this.idPlayer);
            }
        }

        if (clickDuration < this.clickDuration)
        {
            TaskOnClik();
        }
    }

    public void SetCurrentZone(int playerId)
    {
        this.currentZonePlayerId = playerId;
    }

    void OnMouseDrag()
    {
        tableManager = GameObject.Find("GameObject").GetComponent<TableManager>();

        if (!tableManager)
        {
            Debug.LogError("TableManager component not found on the GameObject!");
        }
        else
        {
            if (tableManager.currentPlayer == this.idPlayer)
            {
                // Move the card while preserving the offset
                Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
                Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition) + offset;
                transform.position = objPosition;
            }

        }

        // Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        // Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition) + offset;
        // transform.position = objPosition;

    }

    IEnumerator ResetPosition()
    {
        yield return new WaitForSeconds(0.3f);
        // Move slowly the card to y=0
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0, transform.position.z), 0.1f);
    }

    void CheckAndAdjustPosition()
    {
        // Check if the card is in collision with another card
        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);
        // Debug.Log("hitColliders" + hitColliders.Length);

        // Sort the colliders by their horizontal positions
        System.Array.Sort(hitColliders, (a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != gameObject)
            {
                // Determine the direction based on relative horizontal positions
                float direction = (hitCollider.transform.position.x < transform.position.x) ? 1f : -1f;

                // Slowly move the card to the right or left based on the direction
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + (0.1f * direction), transform.position.y, transform.position.z), 0.1f);
            }
        }
    }

    public int CountCardsInZone(Rect zone)
    {
        Card[] allCards = FindObjectsOfType<Card>(); // Trouvez toutes les cartes dans la scène
        int count = 0;
        int pDeffence = 0;

        foreach (Card card in allCards)
        {
            Vector2 cardPos = new Vector2(card.transform.position.x, card.transform.position.y);
            if (zone.Contains(cardPos))
            {
                count++;
                pDeffence += card.defensePoints;
            }
        }

        return count; // Retourne le nombre de cartes dans la zone
    }

    // Vous pouvez appeler cette fonction pour chaque zone pour obtenir le compte
    public void CheckZonesForCards()
    {
        int cardsInZone1 = CountCardsInZone(playerZone1);
        int cardsInZone2 = CountCardsInZone(playerZone2);
        int cardsInZone3 = CountCardsInZone(playerZone3);
        int cardsInZone4 = CountCardsInZone(playerZone4);

        // Logique ou actions basées sur le nombre de cartes dans chaque zone
        Debug.Log("Zone 1 a " + cardsInZone1 + " cartes.");
        Debug.Log("Zone 2 a " + cardsInZone2 + " cartes.");
        Debug.Log("Zone 3 a " + cardsInZone3 + " cartes.");
        Debug.Log("Zone 4 a " + cardsInZone4 + " cartes.");
    }

    public int PointDeffenceCardsInZone(Rect zone, int idPlayer)
    {
        Card[] allCards = FindObjectsOfType<Card>(); // Trouvez toutes les cartes dans la scène
        int pDeffence = 0;
        int pAttack = 0;

        foreach (Card card in allCards)
        {
            Vector2 cardPos = new Vector2(card.transform.position.x, card.transform.position.y);
            if (zone.Contains(cardPos) && card.idPlayer != idPlayer)
            {
                pDeffence += card.defensePoints;
            }
            if (card.idPlayer == idPlayer)
            {
                pAttack += card.attackPoints;
            }
        }

        // suppriemr toutes les cartes dans la zone sauf la carte qui attaque si elle est plus forte que la carte deffence
        foreach (Card card in allCards)
        {
            Vector2 cardPos = new Vector2(card.transform.position.x, card.transform.position.y);
            if (pAttack > pDeffence && zone.Contains(cardPos) && card.idPlayer != idPlayer)
            {
                Debug.Log("pAttack" + pAttack);
                Debug.Log("pDeffence" + pDeffence); 
                Debug.Log("card dertroyed voir le paln ");
                // how to destroy card;
                UnityEngine.Object.Destroy(card.gameObject);
            }
        }

        return pDeffence; // Retourne le nombre de cartes dans la zone
    }
    public int PlayerIDCarteInZone(Rect zone)
    {
        Card[] allCards = FindObjectsOfType<Card>(); // Trouvez toutes les cartes dans la scène
        int idPlayer = 0;

        foreach (Card card in allCards)
        {
            Vector2 cardPos = new Vector2(card.transform.position.x, card.transform.position.y);
            if (zone.Contains(cardPos))
            {
                idPlayer = card.idPlayer;
            }
        }

        return idPlayer; // Retourne le nombre de cartes dans la zone
    }

}