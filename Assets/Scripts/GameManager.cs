using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Card baseCard; // ������� ������� �����

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool CanMoveCard(Card card)
    {
        int baseValue = baseCard.cardValue;
        int cardValue = card.cardValue;

        // ����� ����� ���� ����������, ���� ��� �� ���� ������ ��� �� ���� ������ ������� �����
        // ����� ��������� ������� �� ������ � ���� � �� ���� � ������
        bool canMove = Mathf.Abs(cardValue - baseValue) == 1 ||
                       (baseValue == 1 && cardValue == 13) || // ������� �� ���� � ������
                       (baseValue == 13 && cardValue == 1);  // ������� �� ������ � ����

        Debug.Log($"CanMoveCard: {card.cardName} with base card {baseCard.cardName} -> {canMove}");
        return canMove;
    }


    public void MoveCardToBase(Card card)
    {
        // ������ ������� �����
        Card oldBaseCard = baseCard;

        // ���������� ����� ����� �� ������� ������
        card.transform.position = baseCard.transform.position;
        card.ShowFront();

        // ��������� ������� �����
        baseCard = card;

        // ������������ ������ ������� ����� ����� ����������
        if (oldBaseCard != null)
        {
            oldBaseCard.gameObject.SetActive(false);
        }

        // ��������� ��������� ���� � ��������� �� �������/��������
        CheckForWinOrLose();
    }

    private void CheckForWinOrLose()
    {
        // ����� ����� �������� ������ ��� �������� �������� ��� ���������
        // ��������, ���� ��� ����� ���������� ��� ���� ��� ��������� �����
    }

    public void DrawCardFromDeck()
    {
        Debug.Log("DrawCardFromDeck called"); // ���������� ���������
        CardManager.Instance.DrawCardFromDeck();
    }
}
