using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int turn = 1;
    public int playerTurn = 1; //1 = player 1, 2 = player 2
    public int cardsDraw;
    public GameObject player1;
    public GameObject player2;
    public GameObject deck;
    public GameObject cardPrefab; // Reference to the card prefab
    public TextMeshProUGUI turnText;

    private Transform p1Transform;
    private Transform p2Transform;
    private Transform p3Transform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the transforms of P1, P2, and P3
        p1Transform = deck.transform.Find("P1");
        p2Transform = deck.transform.Find("P2");
        p3Transform = deck.transform.Find("P3");
        StartTurn();
    }
    public void StartTurn()
    {
        Color player1Color;
        Color player2Color;

        // Parse the hex color strings
        ColorUtility.TryParseHtmlString("#5FA7ED", out player1Color); // Custom blue color
        ColorUtility.TryParseHtmlString("#ED5F77", out player2Color); // Custom red color

        if (playerTurn == 1)
        {
            turnText.color = player1Color;
        }
        else
        {
            turnText.color = player2Color;
        }
        turnText.text = "" + turn;
        GameObject player = playerTurn == 1 ? player1 : player2;
        Player currentPlayer = player.GetComponent<Player>();
        GameObject pOther = playerTurn != 1 ? player1 : player2;
        Player otherPlayer = pOther.GetComponent<Player>();

        Player p1 = player1.GetComponent<Player>();
        Player p2 = player2.GetComponent<Player>();

        if(p1.hp <= 0 || p2.hp <= 0) {
            if (p1.hp <= 0 && p2.hp <= 0)
            {
                PlayerPrefs.SetInt("Winner", 0);
            } else if (p1.hp <= 0)
            {
                PlayerPrefs.SetInt("Winner", 2);
            } else
            {
                PlayerPrefs.SetInt("Winner", 1);
            }
            SceneManager.LoadScene("Game Over");
        }
        if (currentPlayer.status == 2)
        {
            currentPlayer.status = -1;
            StartCoroutine(EndTurn());
        }
        DrawCard();
    }

    public void DrawCard()
    {
        GameObject player = playerTurn == 1 ? player1 : player2;
        Player currentPlayer = player.GetComponent<Player>();
        Transform currentDeckTransform = null;

        if (turn >= 1 && turn <= 5)
        {
            currentDeckTransform = p1Transform;
            cardsDraw = 1;
        }
        else if (turn >= 6 && turn <= 10)
        {
            currentDeckTransform = p2Transform;
            cardsDraw = 2;
        }
        else if (turn >= 11)
        {
            currentDeckTransform = p3Transform;
            cardsDraw = 4;
        }

        cardsDraw += currentPlayer.extraCardsDraw;

        if (currentDeckTransform != null)
        {
            // Get the children of the current deck transform
            List<Transform> cardTransforms = new List<Transform>();
            foreach (Transform cardTransform in currentDeckTransform)
            {
                cardTransforms.Add(cardTransform);
            }

            if (cardTransforms.Count > 0)
            {
                for (int i = 0; i < cardsDraw; i++)
                {
                    int randomIndex = Random.Range(0, cardTransforms.Count);
                    Transform drawnCardTransform = cardTransforms[randomIndex];
                    Card drawnCard = drawnCardTransform.GetComponent<Card>();

                    if (drawnCard != null)
                    {
                        // Instantiate a copy of the card prefab
                        GameObject cardObject = Instantiate(cardPrefab, currentPlayer.transform);
                        Card cardComponent = cardObject.GetComponent<Card>();

                        // Enable the Collider on the new card object
                        BoxCollider2D boxCollider = cardObject.GetComponent<BoxCollider2D>();
                        if (boxCollider != null)
                        {
                            boxCollider.enabled = true;
                        }

                        // Copy the properties from the drawn card to the new card instance
                        cardComponent.hpCost = drawnCard.hpCost;
                        cardComponent.mCost = drawnCard.mCost;
                        cardComponent.pool = drawnCard.pool;
                        cardComponent.damage = drawnCard.damage;
                        cardComponent.status = drawnCard.status;
                        cardComponent.effect = drawnCard.effect;
                        cardComponent.draw = drawnCard.draw;

                        // Copy the child object from the drawn card to the new card instance
                        Transform drawnCardChild = drawnCardTransform.Find("GameObject");
                        if (drawnCardChild != null)
                        {
                            Transform newCardChild = cardObject.transform.Find("CardRender");
                            if (newCardChild != null)
                            {

                                // Copy the SpriteRenderer properties
                                SpriteRenderer drawnSpriteRenderer = drawnCardChild.GetComponent<SpriteRenderer>();
                                SpriteRenderer newSpriteRenderer = newCardChild.GetComponent<SpriteRenderer>();
                                if (drawnSpriteRenderer != null && newSpriteRenderer != null)
                                {
                                    newSpriteRenderer.sprite = drawnSpriteRenderer.sprite;
                                }
                                newSpriteRenderer.enabled = true;
                            }
                        }
                        // Add the card to the player's hand and arrange the cards
                        currentPlayer.AddCardToHand(cardComponent);
                    }
                }
            }
        }

        currentPlayer.extraCardsDraw = 0;
    }

    public IEnumerator EndTurn()
    {
     
        GameObject pCurr = playerTurn == 1 ? player1 : player2;
        GameObject pOther = playerTurn != 1 ? player1 : player2;
        Player currentPlayer = pCurr.GetComponent<Player>();
        Player otherPlayer = pOther.GetComponent<Player>();
        float damageDealtMultiplier = 1.0f;
        float damageTakenMultiplier = 1.0f;

        while (currentPlayer.cardsUsed.Count > 0)
        {
            Card card = currentPlayer.cardsUsed.Dequeue();
            if (card.effect != -1)
            {
                if (card.effect == 0)
                {
                    currentPlayer.status = 0;
                    currentPlayer.animator.SetInteger("anim", 0);
                    yield return new WaitForSeconds(1.0f);
                    currentPlayer.animator.SetInteger("anim", -1);
                    currentPlayer.UpdateHealth();
                }
                if (card.effect == 1)
                {
                    currentPlayer.status = 1;
                    currentPlayer.animator.SetInteger("anim", 1);
                    yield return new WaitForSeconds(1.0f);
                    currentPlayer.animator.SetInteger("anim", -1);
                    currentPlayer.UpdateHealth();
                }
                else {
                    currentPlayer.status = card.effect;
                }
            }
            if (card.status != -1) {
                if (card.status == 0)
                {
                    otherPlayer.status = 0;
                    currentPlayer.animator.SetInteger("anim", 3);
                    currentPlayer.otherAnimator.SetInteger("anim", 0);
                    yield return new WaitForSeconds(1.0f);
                    currentPlayer.animator.SetInteger("anim", -1);
                    currentPlayer.otherAnimator.SetInteger("anim", -1);
                    otherPlayer.UpdateHealth();
                }
                if (card.status == 1)
                {
                    otherPlayer.status = 1;
                    currentPlayer.animator.SetInteger("anim", 3);
                    currentPlayer.otherAnimator.SetInteger("anim", 1);
                    yield return new WaitForSeconds(1.0f);
                    currentPlayer.animator.SetInteger("anim", -1);
                    currentPlayer.otherAnimator.SetInteger("anim", -1);
                    otherPlayer.UpdateHealth();
                }
                if (card.status == 2)
                {
                    otherPlayer.status = 2;
                    currentPlayer.animator.SetInteger("anim", 3);
                    currentPlayer.otherAnimator.SetInteger("anim", 2);
                    yield return new WaitForSeconds(1.0f);
                    currentPlayer.animator.SetInteger("anim", -1);
                    currentPlayer.otherAnimator.SetInteger("anim", -1);
                    otherPlayer.UpdateHealth();
                }
                else {
                    otherPlayer.status = card.effect;
                }
            }
            if (card.effect == 3)
            {
                currentPlayer.animator.SetInteger("anim", 7);
                yield return new WaitForSeconds(1.0f);
                currentPlayer.animator.SetInteger("anim", -1);
                currentPlayer.status = -1;
                currentPlayer.UpdateHealth();
            }
            if (card.status == 3)
            {
                currentPlayer.otherAnimator.SetInteger("anim", 7);
                yield return new WaitForSeconds(1.0f);
                currentPlayer.otherAnimator.SetInteger("anim", -1);
                otherPlayer.status = -1;
                otherPlayer.UpdateHealth();
            }

            if (currentPlayer.status == 0)
            {
                damageTakenMultiplier = 1.5f;
            }
            if (otherPlayer.status == 0)
            {
                damageDealtMultiplier = 1.5f;
            }
            if (currentPlayer.status == 1)
            {
                damageDealtMultiplier = 0.75f;
            }
            if (otherPlayer.status == 1)
            {
                damageTakenMultiplier = 0.75f;
            }
            Debug.Log(damageDealtMultiplier + " " + damageTakenMultiplier);

            if ((int)(damageDealtMultiplier * card.damage) != 0)
            {
                currentPlayer.animator.SetInteger("anim", 3);
                currentPlayer.otherAnimator.SetInteger("anim", 6);
                yield return new WaitForSeconds(1.0f);
                currentPlayer.animator.SetInteger("anim", -1);
                currentPlayer.otherAnimator.SetInteger("anim", -1);
            }
            otherPlayer.hp = Mathf.Min(otherPlayer.hp - (int)(damageDealtMultiplier * card.damage), 100);
            otherPlayer.UpdateHealth();

            if ((int)(damageTakenMultiplier * card.hpCost) < 0) {
                currentPlayer.animator.SetInteger("anim", 4);
                yield return new WaitForSeconds(1.0f);
                currentPlayer.animator.SetInteger("anim", -1);
            } else if ((int)(damageTakenMultiplier * card.hpCost) > 0) {
                currentPlayer.animator.SetInteger("anim", 6);
                yield return new WaitForSeconds(1.0f);
                currentPlayer.animator.SetInteger("anim", -1);
            }
            currentPlayer.hp = Mathf.Min(currentPlayer.hp - (int)(damageTakenMultiplier * card.hpCost), 100);
            currentPlayer.UpdateHealth();
            currentPlayer.extraCardsDraw += card.draw;
            
        }
        if (currentPlayer.status == 1)
        {
            currentPlayer.animator.SetInteger("anim", 1);
            yield return new WaitForSeconds(1.0f);
            currentPlayer.animator.SetInteger("anim", -1);
            currentPlayer.hp -= 5;
            otherPlayer.UpdateHealth();
        }
        currentPlayer.UpdateHealth();
        otherPlayer.UpdateHealth();
        currentPlayer.UpdateMana();
        otherPlayer.UpdateMana();
        if (playerTurn == 2)
        {
            turn++;
        }
        playerTurn = playerTurn == 1 ? 2 : 1;
        pCurr.SetActive(false);
        pOther.SetActive(true);
        StartTurn();
    }
}
