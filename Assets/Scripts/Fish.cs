using UnityEngine;

public class Fish : MonoBehaviour
{

    private AudioSource audioSource;
    [SerializeField] private AudioClip eatSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Fish")
        {
            Debug.Log("fish");
            audioSource.clip = eatSound;
            audioSource.Play();
            Destroy(collision.gameObject);

        }

    }
}
