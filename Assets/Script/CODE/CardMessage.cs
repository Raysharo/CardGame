[System.Serializable] // Assurez-vous d'ajouter ceci pour permettre la sérialisation
public class CardMessage {
    public string type = "move card to player";
    public int cardId; 

    public CardMessage(int id) {
        this.cardId = id;
    }
}