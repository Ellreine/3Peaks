using UnityEngine;
using System.Collections.Generic;

public class Card : MonoBehaviour
{
    public string cardName;
    public int cardValue;
    public string suit;
    public bool isFaceUp;

    private SpriteRenderer spriteRenderer;
    public Sprite frontSprite;
    public Sprite backSprite;

    public List<Card> coveredByCards = new List<Card>(); // Карты, которые перекрывают текущую карту
    public List<Card> coveringCards = new List<Card>(); // Карты, которые закрывает текущая карта

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ShowFront()
    {
        isFaceUp = true;
        spriteRenderer.sprite = frontSprite;
    }

    public void ShowBack()
    {
        isFaceUp = false;
        spriteRenderer.sprite = backSprite;
    }

    public void CheckIfCanBeOpened()
    {
        // Если карту не перекрывает ни одна другая карта, она должна быть открыта
        if (coveredByCards.Count == 0)
        {
            ShowFront();
        }
    }

    public void RemoveCoveringCard(Card card)
    {
        coveredByCards.Remove(card);
        CheckIfCanBeOpened();
    }

    void OnMouseDown()
    {
        Debug.Log($"Card {cardName} clicked"); // Отладочное сообщение

        if (isFaceUp && GameManager.Instance.CanMoveCard(this))
        {
            GameManager.Instance.MoveCardToBase(this);

            // Обновляем состояние перекрытых карт
            foreach (Card coveringCard in coveringCards)
            {
                coveringCard.RemoveCoveringCard(this);
            }
        }
    }
}
