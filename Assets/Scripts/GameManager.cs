using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Card baseCard; // Текущая базовая карта

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

        // Карта может быть перемещена, если она на одну больше или на одну меньше базовой карты
        // Также учитываем переход от короля к тузу и от туза к двойке
        bool canMove = Mathf.Abs(cardValue - baseValue) == 1 ||
                       (baseValue == 1 && cardValue == 13) || // переход от туза к королю
                       (baseValue == 13 && cardValue == 1);  // переход от короля к тузу

        Debug.Log($"CanMoveCard: {card.cardName} with base card {baseCard.cardName} -> {canMove}");
        return canMove;
    }


    public void MoveCardToBase(Card card)
    {
        // Старая базовая карта
        Card oldBaseCard = baseCard;

        // Перемещаем новую карту на базовую стопку
        card.transform.position = baseCard.transform.position;
        card.ShowFront();

        // Обновляем базовую карту
        baseCard = card;

        // Деактивируем старую базовую карту после обновления
        if (oldBaseCard != null)
        {
            oldBaseCard.gameObject.SetActive(false);
        }

        // Обновляем состояние игры и проверяем на выигрыш/проигрыш
        CheckForWinOrLose();
    }

    private void CheckForWinOrLose()
    {
        // Здесь можно добавить логику для проверки выигрыша или проигрыша
        // Например, если все карты перемещены или если нет возможных ходов
    }

    public void DrawCardFromDeck()
    {
        Debug.Log("DrawCardFromDeck called"); // Отладочное сообщение
        CardManager.Instance.DrawCardFromDeck();
    }
}
