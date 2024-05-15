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

    public List<Card> coveredByCards = new List<Card>(); // �����, ������� ����������� ������� �����
    public List<Card> coveringCards = new List<Card>(); // �����, ������� ��������� ������� �����

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
        // ���� ����� �� ����������� �� ���� ������ �����, ��� ������ ���� �������
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
        Debug.Log($"Card {cardName} clicked"); // ���������� ���������

        if (isFaceUp && GameManager.Instance.CanMoveCard(this))
        {
            GameManager.Instance.MoveCardToBase(this);

            // ��������� ��������� ���������� ����
            foreach (Card coveringCard in coveringCards)
            {
                coveringCard.RemoveCoveringCard(this);
            }
        }
    }
}
