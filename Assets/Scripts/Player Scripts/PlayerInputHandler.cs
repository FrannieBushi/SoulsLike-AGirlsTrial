using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public float moveInput;
    public bool jumpPressed;
    public bool jumpHeld;
    public bool dodgePressed;
    public bool usePotionPressed;
    public bool attackPressed;
    public bool specialAttackPressed;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<float>();
        inputActions.Player.Move.canceled += ctx => moveInput = 0f;

        inputActions.Player.Jump.performed += ctx =>
        {
            jumpPressed = true;
            jumpHeld = true;
        };

        inputActions.Player.Jump.canceled += ctx =>
        {
            jumpHeld = false;
        };

        inputActions.Player.Dodge.performed += ctx => dodgePressed = true;

        inputActions.Player.UsePotion.performed += ctx => usePotionPressed = true;

        inputActions.Player.Attack.performed += ctx => attackPressed = true;
        inputActions.Player.SpecialAttack.performed += ctx => specialAttackPressed = true;
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    public void ResetInputs()
    {
        jumpPressed = false;
        dodgePressed = false;
        usePotionPressed = false;
        attackPressed = false;
        specialAttackPressed = false;
    }
    
}
