using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;

    public GameObject cardPrefab;
    public Sprite cardBackSprite;
    public Transform[] pyramidFrameGroups; // массив групп фреймов
    public Transform closedDeckTransform;
    public Transform baseCardTransform;

    private List<Card> deck;
    private Stack<Card> closedDeck; // Стек для закрытой колоды

    void Awake()
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

    void Start()
    {
        // Загружаем спрайт рубашки карт
        cardBackSprite = Resources.Load<Sprite>("Sprites/Cards/back_red");

        InitializeDeck();
        ShuffleDeck();
        StartCoroutine(ArrangeAndDealCards());
    }

    void InitializeDeck()
    {
        deck = new List<Card>();
        closedDeck = new Stack<Card>();

        string[] suits = { "clubs", "diamonds", "hearts", "spades" };
        string[] values = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "j", "q", "k", "a" };

        foreach (string suit in suits)
        {
            foreach (string value in values)
            {
                string cardName = suit + "_" + value;
                Sprite cardSprite = Resources.Load<Sprite>("Sprites/Cards/" + cardName);

                if (cardSprite == null)
                {
                    Debug.LogError("Sprite not found: " + cardName);
                    continue;
                }

                GameObject cardObject = Instantiate(cardPrefab);
                Card card = cardObject.GetComponent<Card>();
                card.cardName = cardName;
                card.cardValue = System.Array.IndexOf(values, value) + 1; // назначение значения карты
                card.suit = suit;
                card.frontSprite = cardSprite;
                card.backSprite = cardBackSprite;
                card.ShowBack();
                deck.Add(card);
            }
        }
    }

    void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = UnityEngine.Random.Range(0, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    IEnumerator ArrangeAndDealCards()
    {
        // Ждем один кадр, чтобы фреймы были расставлены
        yield return null;

        List<Transform> pyramidFrames = new List<Transform>();

        // Объединяем все фреймы из групп в один список
        foreach (Transform group in pyramidFrameGroups)
        {
            foreach (Transform frame in group)
            {
                pyramidFrames.Add(frame);
            }
        }

        // Переменная для отслеживания порядка отрисовки
        int sortingOrder = 0;

        // Присваиваем карты фреймам и устанавливаем перекрытия
        for (int i = 0; i < pyramidFrames.Count; i++)
        {
            Card card = deck[i];
            card.transform.position = pyramidFrames[i].position;

            // Если карта находится в нижнем ряду
            if (IsInBottomRow(i, pyramidFrames.Count))
            {
                card.ShowFront();
            }
            else
            {
                card.ShowBack();
            }

            SpriteRenderer renderer = card.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sortingOrder = sortingOrder; // Установка порядка отрисовки
            }
            sortingOrder++; // Увеличиваем порядок отрисовки

            // Добавляем перекрытия карт
            int row = i / 3; // Предположим, что в каждой строке по 3 карты
            if (row > 0)
            {
                // Текущая карта перекрывает две карты на строке ниже
                int belowIndex1 = (row - 1) * 3 + i % 3;
                int belowIndex2 = belowIndex1 + 1;

                if (belowIndex1 < pyramidFrames.Count)
                {
                    card.coveringCards.Add(deck[belowIndex1]);
                    deck[belowIndex1].coveredByCards.Add(card);
                }

                if (belowIndex2 < pyramidFrames.Count)
                {
                    card.coveringCards.Add(deck[belowIndex2]);
                    deck[belowIndex2].coveredByCards.Add(card);
                }
            }

            Debug.Log($"Card {card.cardName} placed at {pyramidFrames[i].position}");
        }

        // Устанавливаем базовую карту
        Card baseCard = deck[pyramidFrames.Count];
        baseCard.transform.position = baseCardTransform.position;
        baseCard.ShowFront();
        GameManager.Instance.baseCard = baseCard;

        SpriteRenderer baseRenderer = baseCard.GetComponent<SpriteRenderer>();
        if (baseRenderer != null)
        {
            baseRenderer.sortingOrder = sortingOrder; // Установка порядка отрисовки для базовой карты
        }
        sortingOrder++; // Увеличиваем порядок отрисовки

        Debug.Log($"Base card {baseCard.cardName} placed at {baseCardTransform.position}");

        for (int i = pyramidFrames.Count + 1; i < deck.Count; i++)
        {
            Card card = deck[i];
            card.transform.position = closedDeckTransform.position;
            card.ShowBack();

            closedDeck.Push(card); // Добавляем карту в закрытую колоду

            SpriteRenderer closedRenderer = card.GetComponent<SpriteRenderer>();
            if (closedRenderer != null)
            {
                closedRenderer.sortingOrder = sortingOrder; // Установка порядка отрисовки для закрытых карт
            }
            sortingOrder++; // Увеличиваем порядок отрисовки

            Debug.Log($"Card {card.cardName} placed at {closedDeckTransform.position}");
        }

        // Перемещаем closedDeckTransform поверх всех карт
        closedDeckTransform.position = new Vector3(closedDeckTransform.position.x, closedDeckTransform.position.y, -1);
    }

    private bool IsInBottomRow(int index, int totalCount)
    {
        // Предположим, что нижний ряд содержит последние 10 карт
        // Вы можете изменить это значение в зависимости от вашей раскладки карт
        int bottomRowCount = 10;
        return index >= totalCount - bottomRowCount;
    }

    public void DrawCardFromDeck()
    {
        Debug.Log("DrawCardFromDeck called"); // Отладочное сообщение
        if (closedDeck.Count > 0)
        {
            Card newBaseCard = closedDeck.Pop();
            newBaseCard.transform.position = baseCardTransform.position;
            newBaseCard.ShowFront();

            // Старая карта больше не будет участвовать в игре
            if (GameManager.Instance.baseCard != null)
            {
                GameManager.Instance.baseCard.gameObject.SetActive(false);
            }

            // Устанавливаем новую базовую карту
            GameManager.Instance.baseCard = newBaseCard;
        }
        else
        {
            Debug.Log("No more cards in the closed deck.");
        }
    }
}
