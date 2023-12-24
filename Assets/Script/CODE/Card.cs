using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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


    private float clickDuration = 0.3f; // Seuil de durée pour un clic bref en secondes
    private float mouseDownTime;

    public Player owner;
    public int attackPoints;
    public int defensePoints;

    void Start()
    {
        Program = GameObject.Find("Program").GetComponent<Program>();
    }

    public void InitializeCard()
    {
        // Récupère la référence à l'objet auquel ce script est attaché
        this.id = Random.Range(0, 1000000);
        // Utilise gameObject.AddComponent pour ajouter des composants au GameObject

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
            if(currentSceneName != "TableScene")
            {
                StartCoroutine(ResetPosition());
            }      
        }
        CheckAndAdjustPosition();

    }

    void TaskOnClik()
    {
        Debug.Log("TaskOnClik");
        CardMessage messageObject = new CardMessage(this.id,this.GetType().Name, attackPoints, defensePoints);
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

    void OnMouseUp()
    {
        // Debug.Log("OnMouseUp");
        onInteraction = false;
        rend.material.color = this.color;
        float clickDuration = Time.time - mouseDownTime;


        if (clickDuration < this.clickDuration)
        {
            TaskOnClik();
        }
    }

    void OnMouseDrag()
    {
        // Move the card while preserving the offset
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition) + offset;
        transform.position = objPosition;
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

}