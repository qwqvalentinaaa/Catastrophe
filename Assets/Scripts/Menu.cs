using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private GameObject animal;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animal = GameObject.Find("Cat").gameObject;
        animator = animal.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.Play("Sit");
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene("Room");
    }
}
