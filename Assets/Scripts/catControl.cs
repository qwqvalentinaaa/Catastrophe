using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class catControl : MonoBehaviour
{
    private GameObject animal;
    private Animator animator;
    private Rigidbody rb;

    
    [Range(0.6f, 2f)]
    public float moveSpeed = 0.05f;
    [Range(100f, 5000f)]
    public float turnSpeed = 2500.0f;
    public float jumpForce = 3f;
    public float jumpForwardForce = 1f;

    private float animateTimer = 0f;
    public float animateDuration = 0.04f;

    // Audio Vars
    private AudioSource audioSource;
    [SerializeField] private AudioClip eatSound;

    void Start()
    {
        animal = GameObject.Find("Cat").gameObject;
        animator = animal.GetComponent<Animator>();
        rb = animal.GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

    }

    // Swipe Vars
    [Range(0f, 5f)]
    public float capsuleRadius = 10f;      // The "thickness" of the swipe area
    [Range(0f, 5f)]
    public float capsuleLength = 20f;      // How far in front of the cat the swipe reaches
    [Range(0f, 0.07f)]
    public float height = 0f;
    [Range(0f, 5f)]
    public float forward = .1f;
    [Range(0f, 20f)]
    public float pushForce = 10f;           // How hard objects are pushed

    // Swipe takes a swipe at any object in front of the cat sending it flying!
    // Spawns a sphere hitbox anything within the hitbox is flung.
    void Shove()
    {
        // If you have a swipeOrigin (e.g., an empty GameObject at the cat's chest), use it
        Vector3 start = transform.position + transform.up * height + transform.forward * forward;
        Vector3 end = start + transform.forward * capsuleLength;

        Collider[] hits = Physics.OverlapSphere(start, capsuleRadius, ~0);
        foreach (Collider hit in hits)
        {
            //Debug.Log(hit.gameObject.name);
            // Ignore self
            if (hit.attachedRigidbody != null && hit.gameObject != gameObject)
            {
                
                Vector3 dir = (hit.transform.position - transform.position).normalized;
                hit.attachedRigidbody.AddForce(dir * pushForce, ForceMode.Impulse);
            }
        }
    }

    void Nudge()
    {
        // If you have a swipeOrigin (e.g., an empty GameObject at the cat's chest), use it
        Vector3 start = transform.position + transform.up * height + transform.forward * forward;
        Vector3 end = start + transform.forward * capsuleLength;

        Collider[] hits = Physics.OverlapSphere(start, capsuleRadius, ~0);
        foreach (Collider hit in hits)
        {
            //Debug.Log(hit.gameObject.name);
            // Ignore self
            if (hit.attachedRigidbody != null && hit.gameObject != gameObject)
            {
                SoundScript objScript = hit.gameObject.GetComponent<SoundScript>();
                if (objScript != null)
                {
                    objScript.PlayNudge();
                }
                Vector3 dir = (hit.transform.position - transform.position).normalized;
                hit.attachedRigidbody.AddForce(dir * .93f, ForceMode.Impulse);
            }
        }
    }

    // Draws hitbox of Swipe. Uncomment to use.
    void OnDrawGizmos()
    {
        Vector3 start = transform.position + transform.up * height + transform.forward * forward;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(start, capsuleRadius);
    }

    // Checks for collision with fish (eat the fish yum)
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
    void Update()
    {
        if (animateTimer > 0f)
        {
            animateTimer -= Time.deltaTime/10;
            return;
        }

        // knock
        if (Input.GetMouseButtonDown(0))  // Left mouse button
        {
            animator.Play("Attack");
            animateTimer = animateDuration;
            Nudge();
            return;
        }
        // heavy knock
        if (Input.GetMouseButtonDown(1))
        {
            animator.Play("Hit");
            animateTimer = animateDuration;
            Shove();
            return;
        }
        // jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.Play("Jump");
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            Vector3 jumpDirection;
            if (Input.GetKey(KeyCode.W))
            {
                jumpDirection = animal.transform.forward * jumpForwardForce + Vector3.up * jumpForce;
            }
            else
            {
                jumpDirection = Vector3.up * jumpForce;
            }
            rb.AddForce(jumpDirection, ForceMode.Impulse);
            animateTimer = animateDuration;
            return;
        }
        // Move 
        if (Input.GetKey(KeyCode.W))
        {
            //animal.transform.rotation = Quaternion.Euler(0, 270f, 0);
            animal.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            animator.Play("Walk");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //animal.transform.rotation = Quaternion.Euler(0, 90f, 0);
            animal.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
            animator.Play("Walk");
        }
        else
        {
            animator.Play("Idle_A");
        }
        // Turn 
        if (Input.GetKey(KeyCode.A))
        {
            animal.transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            animal.transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
        }
    }

    
}
