using UnityEngine;

public class ArrangeFramesWithGroupSpacing : MonoBehaviour
{
    public float normalSpacing = 2.0f; // расстояние между картами в группе
    public float groupSpacing = 5.0f; // дополнительное расстояние между группами
    public int normalSpacingCount = 3; // количество карт в группе с обычным отступом
    public Vector3 startPosition = new Vector3(-9.0f, 0.0f, 0.0f); // начальная позиция первой карты
    public string sortingLayerName = "Default"; // имя сортировочного слоя
    public int sortingOrderBase = 0; // начальный порядок отрисовки

    void Start()
    {
        Arrange();
    }

    void Arrange()
    {
        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            int groupIndex = i / normalSpacingCount;
            int withinGroupIndex = i % normalSpacingCount;
            float offset = groupIndex * (normalSpacingCount * normalSpacing + groupSpacing) + withinGroupIndex * normalSpacing;
            child.position = startPosition + new Vector3(offset, 0, 0);

            SpriteRenderer renderer = child.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sortingLayerName = sortingLayerName;
                renderer.sortingOrder = sortingOrderBase + i;
            }
        }
    }
}
