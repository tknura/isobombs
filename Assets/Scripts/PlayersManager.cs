using UnityEngine;
using UnityEngine.InputSystem;


public class PlayersManager : MonoBehaviour
{
    [HideInInspector] public PlayersManager instance;
    [SerializeField] private GameObject playerPrefab;

    private void Awake() {
        if (!instance) {
            instance = this;
        }
        var p1 = PlayerInput.Instantiate(playerPrefab, 1, controlScheme: "Keyboard Left", -1, Keyboard.current);
        var p2 = PlayerInput.Instantiate(playerPrefab, 2, controlScheme: "Keyboard Right", -1, Keyboard.current);
    }


}
