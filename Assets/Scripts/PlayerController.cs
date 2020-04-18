using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] float accel, steering, maxVel, maxReverse;
#pragma warning restore 0649

    Rigidbody rb;
    float vert, horiz;
    bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        isGrounded = true;
    }

    void Start()
    {

    }

    void Update()
    {
        vert = Input.GetAxis("Vertical");
        horiz = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        if (isGrounded)
        {
            if ((rb.velocity.magnitude < maxVel && vert > 0f) || (rb.velocity.magnitude < maxReverse && vert < 0f))
            {
                rb.AddForce(transform.forward * accel * vert);
            }
			rb.MoveRotation(rb.rotation * Quaternion.Euler(transform.up * steering * horiz));
        }
    }

    void OnCollisionEnter()
    {
        isGrounded = true;
    }

    void OnCollisionExit()
    {
        isGrounded = false;
    }
}
