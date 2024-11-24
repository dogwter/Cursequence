using UnityEngine;

public class SceneController : MonoBehaviour
{
    public int winner;
    public GameObject player1;
    public GameObject player2;
    public GameObject player1L;
    public GameObject player2L;
    public GameObject Sign;
    public GameObject SignR;
    public GameObject SignB;

    void Start()
    {
        winner = PlayerPrefs.GetInt("Winner");
        if (winner == 0)
        {
            player1.SetActive(false);
            player2.SetActive(false);
            player1L.SetActive(true);
            player2L.SetActive(true);
            Sign.SetActive(true);
        }
        else if (winner == 1)
        {
            player2.SetActive(false);
            player2L.SetActive(true);
            SignR.SetActive(true);
        }
        else { 
            player1.SetActive(false);
            player1L.SetActive(true);
            SignB.SetActive(true);
        }
    }
}
