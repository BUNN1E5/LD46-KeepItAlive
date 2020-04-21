using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public UnityEvent toggleSiren, toggleTrafficLights, fell;

#pragma warning disable 0649
    [SerializeField] float accel, steering, handbrake, traction, maxVel, maxReverse;
#pragma warning restore 0649

    Rigidbody rb;
    Transform[] tires;
    Light[] lights;
    AudioSource[] sources;
    float vert, horiz, vert2, horiz2;
    bool isHandbrakeOn, isSirenOn, isInvincible;
    int groundContactPoints;

    const float tireRot = 35f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tires = GetComponentsInChildren<Transform>();
        lights = GetComponentsInChildren<Light>();
        sources = GetComponents<AudioSource>();
    }

    void Update()
    {
        if (transform.position.y < -10f) fell.Invoke();
        horiz = Input.GetAxis("Horizontal");
        vert2 = Input.GetAxisRaw("Vertical2");
        horiz2 = Input.GetAxisRaw("Horizontal2");

        isHandbrakeOn = Input.GetAxisRaw("Fire3") > 0f;
        if (!isHandbrakeOn)
        {
            vert = Input.GetAxis("Vertical");
        }

        if (Input.GetButtonDown("Jump3"))
        {
            StartCoroutine(Honk());
        }

        if (Input.GetButtonDown("Jump"))
        {
            isSirenOn = !isSirenOn;
            toggleSiren.Invoke();
        }
        if (isSirenOn && sources[1].volume < 1f)
        {
            sources[1].volume = Mathf.Min(sources[1].volume + Time.deltaTime, 1f);
        }
        else if (!isSirenOn && sources[1].volume > 0f)
        {
            sources[1].volume = Mathf.Max(sources[1].volume - Time.deltaTime, 0f);
        }

        if (Input.GetButtonDown("Jump2"))
        {
            toggleTrafficLights.Invoke();
        }
    }

    void FixedUpdate()
    {
        if (groundContactPoints > 0)
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

        if (isInvincible)
        {
            rb.AddForce(transform.up * accel * vert2);
            rb.AddForce(transform.right * accel * horiz2);
        }
    }

    void OnCollisionEnter()
    {
        groundContactPoints++;
    }

    void OnCollisionExit()
    {
        groundContactPoints--;
    }

    public float GetSpeedNormalized()
    {
        return vert < 0f ? -rb.velocity.magnitude / maxReverse : rb.velocity.magnitude / maxVel;
    }

    public float GetAccelNormalized()
    {
        return vert;
    }

    IEnumerator Honk()
    {
        sources[0].pitch = Random.Range(0.95f, 1.05f);
        sources[0].PlayOneShot(sources[0].clip);
        lights[0].intensity = lights[1].intensity = 100f;
        yield return new WaitForSeconds(1f);
        lights[0].intensity = lights[1].intensity = 25f;
    }

    public void MaximumOverdrive()
    {
        isInvincible = true;
        accel = 25f;
        maxVel = float.MaxValue;
        maxReverse = float.MaxValue;
        groundContactPoints = int.MaxValue;
    }
}
