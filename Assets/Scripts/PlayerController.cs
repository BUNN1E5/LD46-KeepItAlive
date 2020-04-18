using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] float speedForce, torqueForce, driftFactorSticky, driftFactorSlippy, maxStickyVelocity;
#pragma warning restore 0649

    Rigidbody rb;
    float vert, horiz;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        float driftFactor = driftFactorSticky;
        if (RightVelocity().magnitude > maxStickyVelocity)
        {
            driftFactor = driftFactorSlippy;
        }

        rb.velocity = ForwardVelocity() + RightVelocity() * driftFactor;

		rb.AddForce(transform.forward * speedForce * vert);

        rb.angularVelocity = new Vector3(horiz * Mathf.Lerp(0, torqueForce, rb.velocity.magnitude / 2), 0, 0);
    }

    Vector3 ForwardVelocity()
    {
        return transform.forward * Vector3.Dot(rb.velocity, transform.forward);
    }

    Vector3 RightVelocity()
    {
        return transform.right * Vector3.Dot(rb.velocity, transform.right);
    }
}
