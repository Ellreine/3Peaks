using UnityEngine;

public class ClosedDeck : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("Closed deck clicked"); // ���������� ���������
        GameManager.Instance.DrawCardFromDeck();
    }
}
