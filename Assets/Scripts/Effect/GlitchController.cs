using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using URPGlitch;

public class GlitchController : MonoBehaviour
{
    [SerializeField] private Volume volume;
    [SerializeField] private float analogDuration = 1f;
    [SerializeField] private float digitalDuration = 3f;
    private bool isEnabled = true;

    private AnalogGlitchVolume analogGlitchVolume;
    private DigitalGlitchVolume digitalGlitchVolume;

    private void Start()
    {
        volume.profile.TryGet<AnalogGlitchVolume>(out analogGlitchVolume);
        volume.profile.TryGet<DigitalGlitchVolume>(out digitalGlitchVolume);
        Reset();
    }

    public void Reset()
    {
        analogGlitchVolume.scanLineJitter.value = 0f;
        analogGlitchVolume.verticalJump.value = 0f;
        analogGlitchVolume.horizontalShake.value = 0f;
        analogGlitchVolume.colorDrift.value = 0f;
        digitalGlitchVolume.intensity.value = 0f;
    }
    
    [ContextMenu("Toggle Effects")]
    public void ToggleEffects()
    {
        isEnabled = !isEnabled;
        analogGlitchVolume.active = isEnabled ? true : false;
        digitalGlitchVolume.active = isEnabled ? true : false;
    }

    [ContextMenu("Random Settings")]
    public void RandomSettings()
    {
        analogGlitchVolume.scanLineJitter.value = Random.Range(0f, 1f);
        analogGlitchVolume.verticalJump.value = Random.Range(0f, 1f);
        analogGlitchVolume.horizontalShake.value = Random.Range(0f, 1f);
        analogGlitchVolume.colorDrift.value = Random.Range(0f, 1f);

        digitalGlitchVolume.intensity.value = Random.Range(0f, 1f);
    }

    [ContextMenu("Glitch Pulse")]
    public void TestGlitchPulse()
    {
        PlayAnalogGlitch(analogDuration);
    }

    public void PlayAnalogGlitch(float duration)
    {
        StartCoroutine(ScanLineGlitchCoroutine(duration));
    }

    private IEnumerator ScanLineGlitchCoroutine(float duration)
    {
        float timer = 0f;

        // Set horizontal shake once
        // analogGlitchVolume.horizontalShake.value = 0.2f;

        while (timer < duration)
        {
            float t = timer / duration;
            float wave = Mathf.Sin(t * Mathf.PI); // 0 → 1 → 0
            analogGlitchVolume.scanLineJitter.value = wave * 0.6f;

            timer += Time.deltaTime;
            yield return null;
        }

        // Reset values at end
        analogGlitchVolume.scanLineJitter.value = 0f;
        analogGlitchVolume.horizontalShake.value = 0f;
    }

    [ContextMenu("Play Digital Glitch")]
    public void TestDigitalGlitch()
    {
        PlayDigitalGlitch(digitalDuration); // 1 second glitch burst
    }
    
    public void PlayDigitalGlitch(float duration)
    {
        StartCoroutine(DigitalGlitchCoroutine(duration));
    }

    private IEnumerator DigitalGlitchCoroutine(float duration)
    {
        if (digitalGlitchVolume == null) yield break;

        // Enable and set intensity
        digitalGlitchVolume.active = true;
        digitalGlitchVolume.intensity.value = 0.4f;

        // Wait for the duration
        yield return new WaitForSeconds(duration);

        // Disable and reset
        digitalGlitchVolume.intensity.value = 0f;
        digitalGlitchVolume.active = false;
    }



}
