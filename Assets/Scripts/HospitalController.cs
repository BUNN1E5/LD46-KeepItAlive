using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HospitalController : MonoBehaviour
{
    public UnityEvent victory;
    public GameObject ambulence;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == ambulence) victory.Invoke();
    }
}
