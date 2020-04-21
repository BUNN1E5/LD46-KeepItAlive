using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    public Road road;

#pragma warning disable 0649
    [SerializeField] float accel, steering, handbrake, traction, maxVel, maxReverse;
    [SerializeField, Range(0, 1)] float vert, horiz;
#pragma warning restore 0649

    Rigidbody rb;
    Transform[] tires;
    bool isGrounded;

    const float tireRot = 35f;

    void Start()
    {
        isGrounded = true;
        rb = GetComponent<Rigidbody>();
        tires = GetComponentsInChildren<Transform>();
    }

    void FixedUpdate()
    {
        if (road == null) return; // If road doesn't exist, just give up

        vert = 1;
        horiz = Vector3.SignedAngle(transform.forward, road.intersectionDirections[0], Vector3.up) / 180;

        if (isGrounded)
        {
			// acceleration
            if ((rb.velocity.magnitude < maxVel && vert > 0f) || (rb.velocity.magnitude < maxReverse && vert < 0f))
            {
                rb.AddForce(transform.forward * accel * vert);
            }

			// steering
            rb.MoveRotation(rb.rotation * Quaternion.Euler(transform.up * steering * horiz));
        }

        tires[2].localEulerAngles = tires[3].localEulerAngles = Vector3.up * tireRot * horiz;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Road")
        {
            road = other.gameObject.GetComponent<Road>();
        }
    }

    void OnCollisionStay(){
        isGrounded = true;
    }

    void OnCollisionExit()
    {
        isGrounded = false;
    }
}
