using System.Collections.Generic;
using UnityEngine;

public class catControl : MonoBehaviour
{
    private GameObject animal;
    private Animator animator;
    private Rigidbody rb;

    [Range(0.001f, 0.4f)]
    public float moveSpeed = 0.05f;
    [Range(100f, 5000f)]
    public float turnSpeed = 2500.0f;
    public float jumpForce = 3f;
    public float jumpForwardForce = 1f;

    private float animateTimer = 0f;
    public float animateDuration = 0.04f;

    void Start()
    {
        animal = GameObject.Find("Cat").gameObject;
        animator = animal.GetComponent<Animator>();
        rb = animal.GetComponent<Rigidbody>();
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
            return;
        }
        // heavy knock
        if (Input.GetMouseButtonDown(1))
        {
            animator.Play("Hit");
            animateTimer = animateDuration;
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
