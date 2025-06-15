using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameMaster : MonoBehaviour
{
    public static gameMaster Instance { get; private set; }
    // Score and Text
    public TextMeshProUGUI scoreText;
    public float score;
    public Button restartButton;
    public GameObject win;

    // Objects
    public Transform destructibles;
    public Transform player;

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Avoid duplicates
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        scoreText.text = "" + score;
    }

    // Update is called once per frame
    void Update()
    {
        if (score >= 1900)
        { 
            win.SetActive(true);
        }
    }
    public void Reset()
    {
        foreach (Transform child in destructibles)
        {
            GameObject c = child.gameObject;
            Rigidbody rb = c.GetComponent<Rigidbody>();
            rb.linearVelocity = new Vector3 (0, 0, 0);
            SoundScript objScript = c.GetComponent<SoundScript>();
            if (objScript != null) { 
                objScript.Reset();
            }
            c.SetActive(true);
        }

        Rigidbody playerRB = player.GetComponent<Rigidbody>();
        playerRB.linearVelocity = new Vector3(0, 0, 0);
        player.position = new Vector3(-1, 0, 7);
        
        score = 0;
        scoreText.text = "" + score;
    }
}
 