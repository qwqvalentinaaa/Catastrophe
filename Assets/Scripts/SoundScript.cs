using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class SoundScript : MonoBehaviour
{
    // Audio
    [SerializeField] private AudioClip bumpSound;
    [SerializeField] private AudioClip breakSound;
    

    // How much to darken when die
    //public float darkenAmount = 0.5f; // 1 = original brightness, 0 = black

    // Object State
    private bool alive;
    public float dollarValue;
    [Range(0, 10f)]
    public float breakVelocity = 2;

    Renderer renderer;
    Color ogColor;
    public Vector3 pos;
    public Quaternion rot;

    private AudioSource audioSource;
    void Start()
    {
        pos = transform.position;
        rot = transform.rotation;
        audioSource = GetComponent<AudioSource>();
        alive = true;
        renderer = GetComponent<Renderer>();
        ogColor = renderer.material.color;
    }
     
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && alive)
        {
            PlayNudge();
        }

        else
        {
            if(collision.relativeVelocity.magnitude >= breakVelocity && alive)
            {
                audioSource.clip = breakSound;
                audioSource.Play();
                alive = false;
                gameMaster.Instance.score += dollarValue;
                gameMaster.Instance.scoreText.text = "" + gameMaster.Instance.score;
                Die();
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
            gameMaster.Instance.score += dollarValue;
            gameMaster.Instance.scoreText.text = "" + gameMaster.Instance.score;
            Die();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayNudge()
    {
        audioSource.clip = bumpSound;
        audioSource.Play();
    }

    public void Reset()
    {
        transform.position = pos;
        transform.rotation = rot;
        alive = true;
        Live();
    }
    
    void Die()
    {
        if (renderer != null)
        {
            renderer.material.color = ogColor * 0.25f;
        }
    }

    void Live()
    {
        if (renderer != null)
        {
            renderer.material.color = ogColor;
        }
    }
}


    