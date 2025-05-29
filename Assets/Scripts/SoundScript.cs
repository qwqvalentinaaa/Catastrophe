using Unity.VisualScripting;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private AudioClip bumpSound;
    [SerializeField] private AudioClip breakSound;
    [Range(10f, 100f)]
    public float dollarValue=50;
    [Range(0, 10f)]
    public float breakVelocity = 2;

    private bool alive;

    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        alive = true;
    }
     
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && alive)
        {
            audioSource.clip = bumpSound;
            audioSource.Play();
        }

        else
        {
            if(collision.relativeVelocity.magnitude >= breakVelocity && alive)
            {
                audioSource.clip = breakSound;
                audioSource.Play();
                alive = false;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag != "Player" && collision.relativeVelocity.magnitude >= breakVelocity && alive)
        {
            audioSource.clip = breakSound;
            audioSource.Play();
            alive = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
