using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : SingletonGeneric<UIController>
{
#pragma warning disable 0649
    [SerializeField] float minRestInterval, maxRestInterval, trialDuration, difficulty, maxHealth, healRatio;
#pragma warning restore 0649

    Slider health;
    WaitWhile whileTrying;
    bool gameIsOver, isInjecting;
    int trialNum;
    float timer, countResuscitations;

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
        countResuscitations += Input.GetButtonDown("Fire2") ? 1f : 0f;
    }

    IEnumerator BeginTrials()
    {
        while (!gameIsOver)
        {
            yield return whileTrying;
            health.gameObject.SetActive(false);
            if (gameIsOver)
            {
                Debug.Log("Game Over.");
                enabled = false;
                break;
            }
            yield return new WaitForSeconds(Random.Range(minRestInterval, maxRestInterval) / difficulty);
            timer = trialDuration * difficulty;
            trialNum = Random.Range(0, 2);
            health.gameObject.SetActive(true);
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
            health.value += countResuscitations * healRatio * maxHealth / difficulty - Time.deltaTime;
            countResuscitations = 0f;
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

    public void CheckGameOver()
    {
        if (health != null) gameIsOver = health.value <= 0f;
    }

    bool IsTrying()
    {
        return timer > 0f && !gameIsOver;
    }
}
