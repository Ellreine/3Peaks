using UnityEngine;

public class ClosedDeck : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("Closed deck clicked"); // Отладочное сообщение
        GameManager.Instance.DrawCardFromDeck();
    }
}
