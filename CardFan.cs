using UnityEngine;

public class CardFan : MonoBehaviour
{
    public float radius = 100.0f;
    public float arcAngle = 60.0f;
    public float defaultAngle = 10.0f;
    public int cards = 1;
    public GameObject cardSlotPrefab; // Prefab for the CardSlot

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ArrangeCardSlotsInArc();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ArrangeCardSlotsInArc()
    {
        if (cards * defaultAngle > arcAngle)
        {
            float startAngle = 90.0f;
            if (cards > 1)
            {
                startAngle = 90 + (arcAngle / 2);
            }
            float angleStep = arcAngle / (cards - 1);

            for (int i = 0; i < cards; i++)
            {
                float angle = startAngle - i * angleStep;
                float radian = angle * Mathf.Deg2Rad;
                float x = transform.position.x + Mathf.Cos(radian) * radius;
                float y = transform.position.y + Mathf.Sin(radian) * radius;
                Vector3 cardPosition = new Vector3(x, y, transform.position.z);

                // Calculate the rotation to face upwards
                Quaternion rotation = Quaternion.Euler(0, 0, angle - 90);

                Instantiate(cardSlotPrefab, cardPosition, rotation);
            }
        }
        else
        {
            float startAngle = 90.0f;
            if (cards > 1)
            {
                startAngle = 90 + (defaultAngle * (cards-1)) / 2;
            }

            for (int i = 0; i < cards; i++)
            {
                float angle = startAngle - i * defaultAngle;
                float radian = angle * Mathf.Deg2Rad;
                float x = transform.position.x + Mathf.Cos(radian) * radius;
                float y = transform.position.y + Mathf.Sin(radian) * radius;
                Vector3 cardPosition = new Vector3(x, y, transform.position.z);

                // Calculate the rotation to face upwards
                Quaternion rotation = Quaternion.Euler(0, 0, angle - 90);

                Instantiate(cardSlotPrefab, cardPosition, rotation);
            }
        }
    }
}