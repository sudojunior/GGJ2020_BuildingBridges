using System.Collections;
using UnityEngine;

public class SCR_LightAnimation : MonoBehaviour
{
    public Light PL;

    public float delay;

    public float minDelay;
    public float maxDelay;

    public float minIntensity;
    public float maxIntensity;

    public float transitionSpeed;
    private Coroutine transitionCoroutine;
    
    private void Start()
    {
        PL = GetComponent<Light>();
        delay = GetRandomDelay();
    }

    private void Update()
    {
        delay -= Time.deltaTime;
        if (delay <= 0)
        {
            ChangeIntensity();
        }
    }

    private void ChangeIntensity()
    {
        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(TransitionLight(transitionSpeed));

        delay = GetRandomDelay();
    }

    private float GetRandomDelay()
    {
        return Random.Range(minDelay, maxDelay);
    }

    private IEnumerator TransitionLight(float transitionSpeed)
    {
        float t = 0;
        float intensity = Random.Range(minIntensity, maxIntensity);

        while (t < 1)
        {
            t += Time.deltaTime * transitionSpeed;
            PL.intensity = Mathf.Lerp(PL.intensity, intensity, t);
            
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
}
