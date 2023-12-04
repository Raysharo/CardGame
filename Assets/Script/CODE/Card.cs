using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    // ATTRIBUTS
    public int id;
    public Vector3 offset;

    // OBJECT
    public Renderer rend;
    public BoxCollider2D collid;
    
    public void InitializeCard()
    {
        // Récupère la référence à l'objet auquel ce script est attaché
        this.id = Random.Range(0, 1000000);
        // Utilise gameObject.AddComponent pour ajouter des composants au GameObject
        // this.rend = gameObject.AddComponent<SpriteRenderer>();
        // this.collid = gameObject.AddComponent<BoxCollider2D>();

        //

        this.rend = gameObject.GetComponent<Renderer>();
        this.collid = gameObject.GetComponent<BoxCollider2D>();

    }

    void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
        // Change color to red when clicked
        rend.material.color = Color.red;
        // Calculez l'offset entre la position initiale et la position actuelle du clic
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
    }

    void OnMouseUp()
    {
        Debug.Log("OnMouseUp");
        // Change color to green when mouse is released
        rend.material.color = Color.white;
    }

    void OnMouseDrag()
    {
        // Move the card while preserving the offset
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition) + offset;
        transform.position = objPosition;
    }
}   