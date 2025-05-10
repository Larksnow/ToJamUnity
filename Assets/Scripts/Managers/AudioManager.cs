using UnityEngine;
using AK.Wwise;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    // Singleton instance
    public static AudioManager main { get; private set; }

    // Inspector fields for Wwise references (optional)
    [SerializeField] private string _initBankName = "Init";


    // Cached event IDs for performance
    private Dictionary<string, uint> _eventIDs = new Dictionary<string, uint>();

    void Awake()
    {
        if (main) Destroy(gameObject);
        else main = this;
    }
    void Start()
    {
        SetDefaultParamters();
        PostEvent("Play_Music");
    }
    // Initialize Wwise and cache common event IDs
    private void InitializeWwise()
    {
        if (!AkSoundEngine.IsInitialized())
            Debug.LogError("Wwise failed to initialize!");

        // Pre-cache frequently used event IDs
        CacheEventIDs(new[] { "Player_Jump", "UI_Click", "Music_Play" });
    }
    public void SetDefaultParamters()
    {
        
    }

    private void CacheEventIDs(string[] eventNames)
    {
        foreach (var name in eventNames)
        {
            _eventIDs[name] = AkSoundEngine.GetIDFromString(name);
        }
    }

    // Bank Management
    

    // Event Playback
    public void PostEvent(string eventName, GameObject target = null)
    {
        // Get or cache the event ID
        uint eventId = GetEventID(eventName);
        
        if (eventId != AkSoundEngine.AK_INVALID_UNIQUE_ID)
        {
            AkSoundEngine.PostEvent(eventId, target ? target : gameObject);
        }
        else
        {
            Debug.LogError($"Event not found: {eventName}");
        }
    }
    
    private uint GetEventID(string eventName)
    {
        // Check cache first
        if (_eventIDs.TryGetValue(eventName, out uint cachedId))
        {
            return cachedId;
        }

        // Not cached? Query Wwise and store it
        uint newId = AkSoundEngine.GetIDFromString(eventName);
        if (newId != AkSoundEngine.AK_INVALID_UNIQUE_ID)
        {
            _eventIDs[eventName] = newId;
            return newId;
        }

        return AkSoundEngine.AK_INVALID_UNIQUE_ID; // Invalid event
    }

    public void StopEvent(string eventName, GameObject target = null, float fadeOut = 0f)
    {
        AkSoundEngine.ExecuteActionOnEvent(
            eventName,
            AkActionOnEventType.AkActionOnEventType_Stop,
            target ? target : gameObject,
            (int)(fadeOut * 1000), // Convert seconds to milliseconds
            AkCurveInterpolation.AkCurveInterpolation_Linear
        );
    }

    // RTPC (Parameter) Control
    public void SetRTPC(string rtpcName, float value, GameObject target = null)
    {
        AkSoundEngine.SetRTPCValue(rtpcName, value, target ? target : gameObject);
    }

    public float GetRTPCValue(string rtpcName)
    {
        float value = 0f;
        int type = 1;
        AkSoundEngine.GetRTPCValue( rtpcName, gameObject, 0, out value, ref type );
        
        return value;
    }


    // Switch Control
    public void SetSwitch(string switchGroup, string switchState, GameObject target = null)
    {
        AkSoundEngine.SetSwitch(switchGroup, switchState, target ? target : gameObject);
    }

    // State Control
    public void SetState(string stateGroup, string state)
    {
        AkSoundEngine.SetState(stateGroup, state);
    }

    // Cleanup
    private void OnDestroy()
    {
        if (main == this)
        {
            AkSoundEngine.StopAll();
            // AkSoundEngine.ClearBanks();
        }
    }

    [ContextMenu("ChangeState")]
    void ChangeState()
    {
        AudioManager.main.SetState("Damage", "Five");
    }
}