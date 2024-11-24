using UnityEngine;

public class EndTurn : MonoBehaviour
{
    public Sprite normal;
    public Sprite hover;

    private SpriteRenderer spriteRenderer;
    private void OnMouseDown()
    {
        GameObject parentObject = transform.parent.gameObject;
        GameManager gameManager = parentObject.GetComponent<GameManager>();
        if (gameManager != null)
        {
            StartCoroutine(gameManager.EndTurn());
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ensure the normal sprite is set initially
        spriteRenderer.sprite = normal;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called when the mouse enters the collider attached to the GameObject
    void OnMouseEnter()
    {
        Debug.Log("Mouse entered");
        spriteRenderer.sprite = hover;
    }

    // Called when the mouse exits the collider attached to the GameObject
    void OnMouseExit()
    {
        Debug.Log("Mouse exited");
        spriteRenderer.sprite = normal;
    }
}
