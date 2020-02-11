using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class Particle : MonoBehaviour
{

    private float appearTime = 2;
    private float idleTime = 36;
    private float fadeTime = 6;

    private Vector3 startScale;

    IEnumerator Start()
    {
        startScale = transform.localScale;
        transform.localScale = Vector3.zero;

        yield return Scale(0, 1, 2);

        yield return new WaitForSeconds(36);

        yield return Scale(1, 0, 6);

        Destroy(gameObject);

    }

    public void Init(float newAppearTime, float newIdleTime, float newFadeTime)
    {
        appearTime = newAppearTime;
        idleTime = newIdleTime;
        fadeTime = newFadeTime;
    }

    IEnumerator Scale(float start, float end, float duration)
    {

        float startTime = Time.time;
        float endTime = startTime + duration;


        while (Time.time < endTime)
        {

            float alpha = (Time.time - startTime) / (endTime - startTime);

            float value = (alpha * (end - start)) + start;

            transform.localScale = startScale * value;

            yield return null;

        }

    }

}
