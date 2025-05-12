using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerDeviceAssigner : MonoBehaviour
{
    public PlayerInput player1;
    public PlayerInput player2;

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
        }
        else if (gamepads.Count == 1)
        {
            // One gamepad and one keyboard
            PairDevice(player1, keyboard);
            PairDevice(player2, gamepads[0]);
        }
        else
        {
            // // Two players share one keyboard
            // PairDevice(player1, keyboard);
            // PairDevice(player2, keyboard);
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
}
