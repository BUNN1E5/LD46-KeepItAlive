using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public UnityEvent toggleSiren, toggleTrafficLights, fell;

#pragma warning disable 0649
    [SerializeField] float accel, steering, handbrake, traction, maxVel, maxReverse, tireRot;
#pragma warning restore 0649

    Rigidbody rb;
    Transform[] tires;
    Light[] lights;
    AudioSource[] sources;
    float vert, horiz, vert2, horiz2;
    bool isHandbrakeOn, isSirenOn;
    int groundContactPoints;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tires = GetComponentsInChildren<Transform>();
        lights = GetComponentsInChildren<Light>();
        sources = GetComponents<AudioSource>();
    }

    void Update()
    {
        isHandbrakeOn = Input.GetAxisRaw("Fire3") > 0f;
        if (!isHandbrakeOn)
        {
            vert = Input.GetAxis("Vertical");
        }
        horiz = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            // TODO pitch rise and fall for siren on/off and engine speed
            if (isSirenOn = !isSirenOn) sources[1].Play();
            else sources[1].Stop();
            toggleSiren.Invoke();
        }
        if (Input.GetButtonDown("Jump2"))
        {
            toggleTrafficLights.Invoke();
        }
        if (Input.GetButtonDown("Jump3"))
        {
            StartCoroutine(Honk());
        }
        if (transform.position.y < -10f) fell.Invoke();
        if (!rb.useGravity)
        {
            vert2 = Input.GetAxisRaw("Vertical2");
            horiz2 = Input.GetAxisRaw("Horizontal2");
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
        if (!rb.useGravity)
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles + transform.forward * steering * vert2 + transform.up * steering * horiz2);
        }
        tires[2].localEulerAngles = tires[3].localEulerAngles = Vector3.up * tireRot * horiz;
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
        accel = 25f;
        steering = 5f;
        maxVel = float.MaxValue;
        maxReverse = float.MaxValue;
        tireRot = 90f;
        rb.useGravity = false;
        groundContactPoints = int.MaxValue;
    }
}
