using WebSocketSharp;

public class Player {

    public int id;
    // public Cart



    public WebSocket PlayerSocket;

    Player(int id, string adresseNgrok )
    {
        this.id = id;
        // string adresseNgrok = "e740-46-193-3-79";
        PlayerSocket = new WebSocket("wss://" + adresseNgrok + ".ngrok-free.app/" + id);
        PlayerSocket.OnMessage += (sender, e) =>
        {
            UnityEngine.Debug.Log("Message reçu du serveur : " + e.Data);
        };
        PlayerSocket.Connect();
    }

    public void SendMessageToPlayers(string message)
    {
        if (PlayerSocket != null && PlayerSocket.ReadyState == WebSocketState.Open)
        {
            PlayerSocket.Send(message);
        }
    }  

}