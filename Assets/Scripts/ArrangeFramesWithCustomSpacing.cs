using UnityEngine;

public class ArrangeFramesWithGroupSpacing : MonoBehaviour
{
    public float normalSpacing = 2.0f; // ���������� ����� ������� � ������
    public float groupSpacing = 5.0f; // �������������� ���������� ����� ��������
    public int normalSpacingCount = 3; // ���������� ���� � ������ � ������� ��������
    public Vector3 startPosition = new Vector3(-9.0f, 0.0f, 0.0f); // ��������� ������� ������ �����
    public string sortingLayerName = "Default"; // ��� �������������� ����
    public int sortingOrderBase = 0; // ��������� ������� ���������

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
