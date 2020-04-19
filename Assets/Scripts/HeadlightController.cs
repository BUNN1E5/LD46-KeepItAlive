using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadlightController : MonoBehaviour
{
    Light hl;

    void Start()
    {
        hl = GetComponent<Light>();
    }

    public void OnHonk()
    {
        StartCoroutine(honk());
    }

    IEnumerator honk()
    {
        hl.intensity = 100f;
        yield return new WaitForSeconds(1f);
        hl.intensity = 25f;
    }
}
