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
    Button[] buttons;
    Animator[] anims;
    Text[] splashes;
    WaitWhile whileTrying;
    bool isDead, isInjecting;
    int trialNum;
    float timer, countResuscitations;

    void Start()
    {
        health = GetComponentInChildren<Slider>();
        buttons = GetComponentsInChildren<Button>();
        anims = GetComponentsInChildren<Animator>();
        splashes = GetComponentsInChildren<Text>();
        whileTrying = new WaitWhile(() => IsTrying());
        health.value = health.maxValue = maxHealth;
        splashes[0].gameObject.SetActive(false);
        splashes[1].gameObject.SetActive(false);
        splashes[2].gameObject.SetActive(false);
        StartCoroutine(BeginTrials());
    }

    void Update()
    {
        isInjecting = Input.GetButton("Fire1");
        countResuscitations += Input.GetButtonDown("Fire2") ? 1f : 0f;
        anims[1].SetBool("Flash", trialNum == 1);
    }

    IEnumerator BeginTrials()
    {
        difficulty -= 0.1f;
        while (!isDead)
        {
            yield return whileTrying;
            difficulty += 0.1f;
            health.gameObject.SetActive(false);
            buttons[0].gameObject.SetActive(false);
            buttons[1].gameObject.SetActive(false);
            yield return new WaitForSeconds(Random.Range(minRestInterval, maxRestInterval) / difficulty);
            timer = trialDuration + difficulty - 1f;
            trialNum = Random.Range(0, 2);
            if (isDead) break;
            health.gameObject.SetActive(true);
            buttons[0].gameObject.SetActive(true);
            buttons[1].gameObject.SetActive(true);
            switch (trialNum)
            {
                case 0:
                    StartCoroutine(Inject());
                    break;
                case 1:
                    StartCoroutine(Resuscitate());
                    break;
            }
        }
        if (health.value <= 0f) splashes[0].gameObject.SetActive(true);
        else splashes[1].gameObject.SetActive(true);
        splashes[2].gameObject.SetActive(true);
        enabled = false;
    }

    IEnumerator Resuscitate()
    {
        anims[2].SetBool("Alert", true);
        yield return new WaitForSeconds(2f);
        anims[2].SetBool("Alert", false);
        while (IsTrying())
        {
            health.value += countResuscitations * healRatio * maxHealth - Time.deltaTime * difficulty;
            countResuscitations = 0f;
            timer -= Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator Inject()
    {
        anims[2].SetBool("Alert", true);
        anims[trialNum].SetTrigger("Press");
        yield return new WaitForSeconds(1f);
        anims[trialNum].SetTrigger("Press");
        yield return new WaitForSeconds(1f);
        anims[2].SetBool("Alert", false);
        while (IsTrying())
        {
            health.value += isInjecting ? Time.deltaTime / difficulty : -Time.deltaTime;
            timer -= Time.deltaTime;
            yield return null;
        }
        anims[trialNum].SetTrigger("Release");
    }

    public void CheckGameOver()
    {
        if (health != null) isDead = health.value <= 0f;
    }

    bool IsTrying()
    {
        return timer > 0f && !isDead;
    }

    public void OnVictory()
    {
        isDead = true;
    }

    public void OnFall()
    {
        health.value = 0f;
        isDead = true;
    }
}
