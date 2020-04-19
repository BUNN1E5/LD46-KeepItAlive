using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public UnityEvent toggleSiren, toggleTrafficLights, honk;

#pragma warning disable 0649
    [SerializeField] float accel, steering, handbrake, traction, maxVel, maxReverse, tireRot;
#pragma warning restore 0649

    Rigidbody rb;
    Transform[] tires;
    float vert, horiz;
    bool isGrounded, isHandbrakeOn;

    void Start()
    {
        isGrounded = true;
        rb = GetComponent<Rigidbody>();
        tires = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        isHandbrakeOn = Input.GetAxisRaw("Fire3") > 0f;
        if (!isHandbrakeOn)
        {
            vert = Input.GetAxis("Vertical");
        }
        horiz = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump")) // toggle siren
        {
            toggleSiren.Invoke();
        }
        if (Input.GetButtonDown("Jump2")) // toggle traffic lights
        {
            toggleTrafficLights.Invoke();
        }
        if (Input.GetButtonDown("Jump3")) // honk
        {
            honk.Invoke();
        }
    }

    void FixedUpdate()
    {
        if (isGrounded)
        {
			// acceleration
            if ((rb.velocity.magnitude < maxVel && vert > 0f) || (rb.velocity.magnitude < maxReverse && vert < 0f))
            {
                rb.AddForce(transform.forward * accel * vert);
            }

			// steering
            if (isHandbrakeOn)
            {
                rb.MoveRotation(rb.rotation * Quaternion.Euler(transform.up * Mathf.Pow(handbrake, traction) * horiz * rb.velocity.magnitude / maxVel));
            }
            else
            {
                rb.MoveRotation(rb.rotation * Quaternion.Euler(transform.up * steering * horiz));
            }
        }

        tires[2].localEulerAngles = tires[3].localEulerAngles = Vector3.up * tireRot * horiz;
    }

    void OnCollisionEnter()
    {
        isGrounded = true;
    }

    void OnCollisionExit()
    {
        isGrounded = false;
    }

    public float GetSpeedNormalized()
    {
        return vert < 0f ? -rb.velocity.magnitude / maxReverse : rb.velocity.magnitude / maxVel;
    }

    public float GetAccelNormalized()
    {
        return vert;
    }
}
