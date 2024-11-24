using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour
{

    public int hpCost;
    public int mCost;

    public int pool; //what pool the card is part of
    public int damage;
    public int status; //0 curse 1 burn 2 shock 3 clense

    public int effect; //0 curse 1 burn 2 shock 3 clense

    public int draw;
    bool cursorIn = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && cursorIn) // Right mouse button
        {
            Debug.Log("Right mouse button clicked");
            StartCoroutine(handleRightClick());
        }
    }

    public IEnumerator handleRightClick() {
        GameObject parentObject = transform.parent.gameObject;
        Player parentPlayer = parentObject.GetComponent<Player>();
        float manaMultiplier = 1.0f;
        if (parentPlayer.status == 0)
        {
            manaMultiplier = 0.5f;
        }
        if (parentPlayer != null)
        {
            parentPlayer.mana = Mathf.Min(parentPlayer.mana + (int)(manaMultiplier * 5), 100);
            parentPlayer.UpdateMana();
            parentPlayer.hand.Remove(this);
            parentPlayer.ArrangeCardSlotsInArc(); // Rearrange cards after playing one
            parentPlayer.animator.SetInteger("anim", 5);
            yield return new WaitForSeconds(1.0f);
            parentPlayer.animator.SetInteger("anim", -1);
            Destroy(this.gameObject);
        }
    }
    private void OnMouseEnter()
    {
        Transform childTransform = this.transform.Find("CardRender");
        if (childTransform != null)
        {
            childTransform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            childTransform.localPosition = new Vector3(0.0f, 0.3f, -5f);
        }
        cursorIn = true;
    }

    private void OnMouseExit()
    {
        Transform childTransform = this.transform.Find("CardRender");
        if (childTransform != null)
        {
            childTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            childTransform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        }
        cursorIn = false;
    }

    private void OnMouseDown()
    {
        GameObject parentObject = transform.parent.gameObject;
        Player parentPlayer = parentObject.GetComponent<Player>();
        if (parentPlayer != null)
        {
            if (parentPlayer.mana >= mCost)
            {
                parentPlayer.PlayCards(this);
                parentPlayer.mana -= mCost;
                parentPlayer.UpdateMana();
                Destroy(this.gameObject);
            }
        }
    }

}
