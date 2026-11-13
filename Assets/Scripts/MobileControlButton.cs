using UnityEngine;
using UnityEngine.EventSystems;

public class MobileControlButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public enum Control { Left, Right, Jump, Attack }
    public Control control;

    private void Awake()
    {
        if (!Application.isMobilePlatform) gameObject.SetActive(false);
    }

    private static bool leftHeld, rightHeld;
    private static bool jumpPressed, attackPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (control)
        {
            case Control.Left: leftHeld = true; break;
            case Control.Right: rightHeld = true; break;
            case Control.Jump: jumpPressed = true; break;
            case Control.Attack: attackPressed = true; break;
        }
    }

    public void OnPointerUp(PointerEventData eventData) => Release();
    public void OnPointerExit(PointerEventData eventData) => Release();

    private void OnDisable() => Release();

    private void Release()
    {
        switch (control)
        {
            case Control.Left: leftHeld = false; break;
            case Control.Right: rightHeld = false; break;
            case Control.Attack: attackPressed = false; break;
        }
    }

    public static float Horizontal => (rightHeld ? 1f : 0f) - (leftHeld ? 1f : 0f);

    public static bool ConsumeJump()
    {
        bool value = jumpPressed;
        jumpPressed = false;
        return value;
    }

    public static bool ConsumeAttack()
    {
        bool value = attackPressed;
        attackPressed = false;
        return value;
    }
}
