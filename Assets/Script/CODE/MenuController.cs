using UnityEngine;
// using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    public void OnPlayerButtonClicked(int playerID)
    {
        // EventSystem currentEventSystem = FindObjectOfType<EventSystem>();
        // if (currentEventSystem != null)
        // {
        //     Destroy(currentEventSystem.gameObject);
        // }
        GameManager.Instance.LoadPlayerScene(playerID);
    }
}