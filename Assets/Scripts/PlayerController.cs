using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] float accel, steering, handbrake, maxVel, maxReverse;
#pragma warning restore 0649

    Rigidbody rb;
    float vert, horiz;
    bool isGrounded, isHandbrakeOn;

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
		isHandbrakeOn = Input.GetAxisRaw("Fire3") > 0f;
        if (!isHandbrakeOn)
        {
            vert = Input.GetAxis("Vertical");
        }
		horiz = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        if (isGrounded)
        {
            if ((GetSpeed() < maxVel && vert > 0f) || (GetSpeed() < maxReverse && vert < 0f))
            {
                rb.AddForce(transform.forward * accel * vert);
            }
            rb.MoveRotation(rb.rotation * Quaternion.Euler(transform.up * (isHandbrakeOn ? handbrake : steering) * horiz));
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

	float GetSpeed() {
		return rb.velocity.magnitude;
	}
}
