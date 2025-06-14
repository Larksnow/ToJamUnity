using UnityEngine;
using UnityEngine.Rendering;

public class TogglePostProcessing : MonoBehaviour
{
    private Volume volume;

    void Start()
    {
        volume = GetComponent<Volume>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            volume.enabled = !volume.enabled;
        }
    }
}
