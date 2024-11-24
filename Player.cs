using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int hp = 100;
    public int mana = 0;
    public int status = -1;
    public int extraCardsDraw = 0;
    public Queue<Card> cardsUsed = new Queue<Card>();
    public List<Card> hand = new List<Card>(); // Assuming you have a hand list to store drawn cards
    public Image healthImage;
    public Image manaImage;
    public Image manaImageB;
    public Image manaImageC;
    public GameObject P;
    public GameObject otherP;
    public Animator animator;
    public Animator otherAnimator;

    public float radius = 100.0f;
    public float arcAngle = 60.0f;
    public float defaultAngle = 10.0f;
    public float xPosition = 0.0f;
    public float yPosition = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = P.GetComponent<Animator>();
        otherAnimator = otherP.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ArrangeCardSlotsInArc()
    {
        int cards = hand.Count; // Assuming you want to arrange the cards in hand
        if (cards == 0) return;

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
                float x = transform.position.x + Mathf.Cos(radian) * radius + xPosition;
                float y = transform.position.y + Mathf.Sin(radian) * radius + yPosition;
                float z = transform.position.z - i * (0.01f);
                Vector3 cardPosition = new Vector3(x, y, z);

                // Calculate the rotation to face upwards
                Quaternion rotation = Quaternion.Euler(0, 0, angle - 90);

                // Move and rotate the card
                hand[i].transform.position = cardPosition;
                hand[i].transform.rotation = rotation;
            }
        }
        else
        {
            float startAngle = 90.0f;
            if (cards > 1)
            {
                startAngle = 90 + (defaultAngle * (cards - 1)) / 2;
            }

            for (int i = 0; i < cards; i++)
            {
                float angle = startAngle - i * defaultAngle;
                float radian = angle * Mathf.Deg2Rad;
                float x = transform.position.x + Mathf.Cos(radian) * radius + xPosition;
                float y = transform.position.y + Mathf.Sin(radian) * radius + yPosition;
                float z = transform.position.z - i*(0.1f);
                Vector3 cardPosition = new Vector3(x, y, z);

                // Calculate the rotation to face upwards
                Quaternion rotation = Quaternion.Euler(0, 0, angle - 90);

                // Move and rotate the card
                hand[i].transform.position = cardPosition;
                hand[i].transform.rotation = rotation;
            }
        }
    }

    public void PlayCards(Card card)
    {
        cardsUsed.Enqueue(card);
        hand.Remove(card);
        ArrangeCardSlotsInArc(); // Rearrange cards after playing one
    }

    public void AddCardToHand(Card card)
    {
        hand.Add(card);
        ArrangeCardSlotsInArc(); // Rearrange cards after adding one
    }

    public List<Card> GetCardChildren()
    {
        List<Card> cardChildren = new List<Card>();
        foreach (Transform child in transform)
        {
            Card card = child.GetComponent<Card>();
            if (card != null)
            {
                cardChildren.Add(card);
            }
        }
        return cardChildren;
    }

    public void UpdateHealth()
    {
        healthImage.fillAmount = (float)hp / 100.0f;
    }

    public void UpdateMana()
    {
        manaImage.fillAmount = (float)mana / 100.0f;
        manaImageB.fillAmount = (float)mana / 100.0f;
        manaImageC.fillAmount = (float)mana / 100.0f;

        if (status == 0)
        {
            manaImage.enabled = false;
            manaImageB.enabled = false;
            manaImageC.enabled = true;
        }
        else if (status == 1)
        {
            manaImage.enabled = false;
            manaImageB.enabled = true;
            manaImageC.enabled = false;
        }
        else {
            manaImage.enabled = true;
            manaImageB.enabled = false;
            manaImageC.enabled = false; 
        }
    }
}
