using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using System.Collections.Generic;

public class PlayerDeviceAssigner : MonoBehaviour
{
    public PlayerInput player1;
    public PlayerInput player2;

    public ObjectEventSO changeControlSchemeEvent1;
    public ObjectEventSO changeControlSchemeEvent2;

    string actionKey1 = "UseItem1";
    string actionKey2 = "UseItem2";
    string actionKey3 = "UseItem3";

    void Start()
    {
        AssignDevices();
   
        
        
    }

    void AssignDevices()
    {
        var gamepads = Gamepad.all;
        var keyboard = Keyboard.current;

        if (gamepads.Count >= 2)
        {
            Debug.Log("Two gamepads detected");
            // Two gamepads
            PairDevice(player1, gamepads[0]);
            PairDevice(player2, gamepads[1]);
            string device1 = GetDeviceKey(player1);
            string device2 = GetDeviceKey(player2);
            changeControlSchemeEvent1.RaiseEvent(device1, this);
            changeControlSchemeEvent2.RaiseEvent(device2, this);
        }
        else if (gamepads.Count == 1)
        {
            // One gamepad and one keyboard
            PairDevice(player1, keyboard);
            PairDevice(player2, gamepads[0]);
            string device1 = "Keyboard_P1";
            string device2 = GetDeviceKey(player2);
            changeControlSchemeEvent1.RaiseEvent(device1, this);
            changeControlSchemeEvent2.RaiseEvent(device2, this);
        }
        else
        {
            // // Two players share one keyboard
            changeControlSchemeEvent1.RaiseEvent("Keyboard_P1", this);
            changeControlSchemeEvent2.RaiseEvent("Keyboard_P2", this);
        }
    }

    void PairDevice(PlayerInput player, InputDevice device)
    {
        if (player == null || device == null) return;

        // Detach any existing device bindings
        player.user.UnpairDevices();
        
        // Pair the new device to the user
        InputUser.PerformPairingWithDevice(device, player.user);

        // Make sure the user is active
        player.user.ActivateControlScheme(player.defaultControlScheme);

        Debug.Log($"Assigned {device.displayName} to {player.gameObject.name}");
    }

    string GetDeviceKey(PlayerInput player)
    {
        foreach (var device in player.devices)
        {
            if (device is Gamepad gamepad)
            {
                string layout = gamepad.layout.ToLower();
                string displayName = gamepad.displayName.ToLower();
                string name = gamepad.name.ToLower(); // internal Unity name

                if (layout.Contains("xinput") || displayName.Contains("xbox") || name.Contains("xinput"))
                    return "Xbox";
                else if (
                    layout.Contains("dualshock") || layout.Contains("dualsense") ||
                    displayName.Contains("playstation") || displayName.Contains("dualsense") ||
                    name.Contains("dualsense") || name.Contains("playstation")
                )
                    return "PlayStation";
                else
                    return "Gamepad"; // fallback for other brands
            }
        }

        return "Unknown";
    }

    
}
