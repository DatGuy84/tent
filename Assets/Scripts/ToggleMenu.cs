using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleMenu : MonoBehaviour
{
    public GameObject menu;
    public InputActionProperty toggleAction;

    void Update()
    {
        if (toggleAction.action.WasPressedThisFrame())
        {
            menu.SetActive(!menu.activeSelf);
        }
    }
}