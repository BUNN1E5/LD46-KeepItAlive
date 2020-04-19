using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] float minRestInterval, maxRestInterval, trialDuration, difficulty, maxHealth;
#pragma warning restore 0649

    Slider health;
    WaitWhile whileTrying;
    bool gameIsOver, isInjecting;
    int trialNum;
    float timer;

    void Start()
    {
        health = GetComponentInChildren<Slider>();
        whileTrying = new WaitWhile(() => IsTrying());
        health.value = health.maxValue = maxHealth;
        StartCoroutine(BeginTrials());
    }

    void Update()
    {
        isInjecting = Input.GetButton("Fire1");
        gameIsOver = health.value <= 0f;
        if (gameIsOver)
        {
            Debug.Log("Game Over.");
            this.enabled = false;
        }
    }

    IEnumerator BeginTrials()
    {
        while (!gameIsOver)
        {
            yield return whileTrying;
            yield return new WaitForSeconds(Random.Range(minRestInterval, maxRestInterval) / difficulty);
            timer = trialDuration * difficulty;
            trialNum = Random.Range(0, 2);
            switch (trialNum)
            {
                case 0:
                    StartCoroutine(Resuscitate());
                    break;
                case 1:
                    StartCoroutine(Inject());
                    break;
            }
        }
    }

    IEnumerator Resuscitate()
    {
        Debug.Log("Resuscitating...");
        while (IsTrying())
        {
            health.value -= Time.deltaTime;
            timer -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("Done!");
    }

    IEnumerator Inject()
    {
        Debug.Log("Injecting...");
        while (IsTrying())
        {
            health.value += isInjecting ? Time.deltaTime / difficulty : -Time.deltaTime;
            timer -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("Done!");
    }

    bool IsTrying()
    {
        return timer > 0f && !gameIsOver;
    }
}
