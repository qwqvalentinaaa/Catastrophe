using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{

    public GameObject win;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame

    public void OnHomeButton()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnRetryAfterWin()
    {
        win.SetActive(false);
    }
}
