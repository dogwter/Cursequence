using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public Sprite normal;
    public Sprite hover;

    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ensure the normal sprite is set initially
        spriteRenderer.sprite = normal;
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

    void OnMouseDown()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
