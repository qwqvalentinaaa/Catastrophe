using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class catControl : MonoBehaviour
{
    public GameObject animal;

    private string currentAnim = "";
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

    public Transform orientation;

    // Audio Vars
    private AudioSource audioSource;
    [SerializeField] private AudioClip eatSound;

    void Start()
    {
        
        animator = animal.GetComponent<Animator>();
        rb = animal.GetComponent<Rigidbody>();
        audioSource = animal.GetComponent<AudioSource>();

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
        Vector3 start = animal.transform.position + animal.transform.up * height + animal.transform.forward * forward;
        Vector3 end = start + animal.transform.forward * capsuleLength;

        Collider[] hits = Physics.OverlapSphere(start, capsuleRadius, ~0);
        foreach (Collider hit in hits)
        {
            //Debug.Log(hit.gameObject.name);
            // Ignore self
            if (hit.attachedRigidbody != null && hit.gameObject != gameObject)
            {
                
                Vector3 dir = (hit.transform.position - animal.transform.position).normalized;
                hit.attachedRigidbody.AddForce(dir * pushForce, ForceMode.Impulse);
            }
        }
    }

    void Nudge()
    {
        // If you have a swipeOrigin (e.g., an empty GameObject at the cat's chest), use it
        Vector3 start = animal.transform.position + animal.transform.up * height + animal.transform.forward * forward;
        Vector3 end = start + animal.transform.forward * capsuleLength;

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
                Vector3 dir = (hit.transform.position - animal.transform.position).normalized;
                hit.attachedRigidbody.AddForce(dir * .93f, ForceMode.Impulse);
            }
        }
    }

    // Draws hitbox of Swipe. Uncomment to use.
    void OnDrawGizmos()
    {
        Vector3 start = animal.transform.position + animal.transform.up * height + animal.transform.forward * forward;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(start, capsuleRadius);
    }



    private void FixedUpdate()
    
    {
        MovePlayer();
    }

    private void MovePlayer()
{
    Vector3 moveDir = Vector3.zero;

    if (Input.GetKey(KeyCode.W))
        moveDir += orientation.forward;
    if (Input.GetKey(KeyCode.S))
        moveDir -= orientation.forward;
    if (Input.GetKey(KeyCode.A))
        moveDir -= orientation.right;
    if (Input.GetKey(KeyCode.D))
        moveDir += orientation.right;

    if (moveDir != Vector3.zero)
    {
        moveDir.Normalize();

        rb.linearVelocity = new Vector3(moveDir.x * moveSpeed, rb.linearVelocity.y, moveDir.z * moveSpeed);

        if (currentAnim != "Walk")
            {
                animator.Play("Walk");
                currentAnim = "Walk";
            }
    }
    else
    {
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        
        if (currentAnim != "Idle_A")
            {
                animator.Play("Idle_A");
                currentAnim = "Idle_A";
            }
    }
}

    void Update()
    {
        
        if (animateTimer > 0f)
        {
            animateTimer -= Time.deltaTime / 10;
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

        // Move 
        // Move in 8 directions (WASD - world axes)
        // Get the direction from key input

        // // If any direction key is pressed
        // if (moveDir != Vector3.zero)
        // {
        //     moveDir.Normalize();
        //     animal.transform.position += moveDir * moveSpeed * Time.deltaTime;
        //     // Face the direction of movement
        //     // animal.transform.rotation = Quaternion.LookRotation(moveDir, Vector3.up);s
        //     animator.Play("Walk");
        // }
        // else
        // {
        //     animator.Play("Idle_A");
        // }
        // jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.speed = 0.5f;
            animator.Play("Jump");
            // rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            // Vector3 jumpDirection;
            // jumpDirection = moveDir * jumpForwardForce + Vector3.up * jumpForce;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animateTimer = 0.08f;
            return;
        } else
        {
            animator.speed = 1f;
        }

    }


}
