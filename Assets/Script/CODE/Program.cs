using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Program : MonoBehaviour
{
    string adresseNgrok = "6ec8-2a02-8440-c201-d671-5460-1f39-d614-7ca8";
    public Player player;

    void Start()
    {
        int playerID = GameManager.Instance.PlayerID;
        player = new Player(playerID,adresseNgrok);
    }

//     // public Player player;

//     // string adresseNgrok = "6ec8-2a02-8440-c201-d671-5460-1f39-d614-7ca8";

//     // void Awake()
//     // {
//     //     DontDestroyOnLoad(this.gameObject);
//     // }

//     // void Start()
//     // {
//     //     SceneManager.sceneLoaded += myFunction;

//     // }

//     // void myFunction(Scene scene, LoadSceneMode mode)
//     // {
//     //     Debug.Log("Scene loaded");
//     //     if (scene.name == "SampleScene")
        
//     //     {
//     //         Debug.Log("Scene SampleScene loaded");
//     //         // wait for scene to load
//     //         Debug.Log("Scene SampleScene loaded");
//     //         // ...

//     //         // Se désabonner après que la scène a été chargée pour éviter des appels multiples
//     //         SceneManager.sceneLoaded -= myFunction;
//     //         int playerId = GameManager.PlayerId;

//     //         player = new Player(playerId, adresseNgrok);
//     //     }
//     // }

//     // public void OnClick()
//     // {
//     //     Debug.Log("Button clicked");
//     //     // get the name last char of the name of the button
//     //     string buttonName = this.name;
//     //     char buttonNumber = buttonName[buttonName.Length - 1];
//     //     Debug.Log("Button number: " + buttonNumber);
//     //     // TODO send message to server


//     //     // Switch scene
//     //     SceneManager.LoadScene("SampleScene");

//     //     // // wait for the scene to load
//     //     // StartCoroutine(LoadSceneAsync("SampleScene"));



//     // }

//     // IEnumerator LoadSceneAsync(string sceneName)
//     // {
//     //     // Commencez le chargement de la scène de manière asynchrone
//     //     AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

//     //     // Attendez que le chargement soit complet
//     //     while (!asyncOperation.isDone)
//     //     {
//     //         float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

//     //         // Vous pouvez utiliser la valeur de 'progress' pour afficher une barre de chargement, par exemple
//     //         // Debug.Log("Chargement de la scène : " + (progress * 100) + "%");

//     //         yield return null; // Attendre la prochaine frame
//     //     }

//     //     // Le chargement est maintenant terminé
//     //     Debug.Log("La scène est chargée !");
//     // }








//     // void Awake()
//     // {
//     //     if (Instance == null)
//     //     {
//     //         Instance = this;
//     //         DontDestroyOnLoad(gameObject);
//     //     }
//     //     else if (Instance != this)
//     //     {
//     //         Destroy(gameObject); // Destroy any duplicate
//     //     }
//     // }

//     // void OnEnable()
//     // {
//     //     SceneManager.sceneLoaded += myFunction; // Subscribe
//     // }

//     // void OnDisable()
//     // {
//     //     SceneManager.sceneLoaded -= myFunction; // Unsubscribe
//     // }

//     // void myFunction(Scene scene, LoadSceneMode mode)
//     // {
//     //     Debug.Log("Scene loaded");
//     //     if (scene.name == "SampleScene")
//     //     {
//     //         Debug.Log("SampleScene loaded");
//     //         // Initialize player here or in another method
//     //         InitializePlayer(GameManager.PlayerId);
//     //     }
//     // }

//     // void InitializePlayer(int playerId)
//     // {
//     //     // Your logic to initialize the player
//     //     player = new Player(playerId, adresseNgrok);
//     // }

//     // Rest of your methods...


}

