using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour
{

    #pragma warning disable 0649
    [SerializeField] float accel, steering, handbrake, traction, maxVel, maxReverse, tireRot;
    #pragma warning restore 0649

    Rigidbody rb;
    Transform[] tires;
    Light[] lights;
    AudioSource[] sources;

    [SerializeField, Range(0, 1)]
    float vert, horiz;
    bool isGrounded, isHandbrakeOn, isSirenOn;


    public Road road;

    void Start()
    {
        isGrounded = true;
        rb = GetComponent<Rigidbody>();
        tires = GetComponentsInChildren<Transform>();
        lights = GetComponentsInChildren<Light>();
        sources = GetComponents<AudioSource>();
    }

    void FixedUpdate()
    {

        if(road == null) //If road doesn't exist, just give up
            return;

        RaycastHit hit;

        vert = 1;
        if(Physics.SphereCast(this.transform.position, 1, this.transform.forward, out hit, 3)){
            //SlowDown based on the distance
            
            //vert = hit.distance - GetSpeedNormalized();
        }
        horiz = Vector3.SignedAngle(this.transform.forward, road.ChooseDirectionFromIntersection(this.gameObject.GetInstanceID()), Vector3.up) / 180;


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

    void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Road"){
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
}
