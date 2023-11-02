using UnityEngine;

public class PlayerController : MonoBehaviour
{
    protected Rigidbody rb;

    public float BaseMoveForce;

    [ReadOnlyInspector]
    public float CurrentMoveForce;

    protected PlayerInputActions inputActions;

    protected Player player;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();

        player = GetComponent<Player>();

        inputActions.Player.Pause.performed += Pause_performed;

        inputActions.Player.TriggerActiveAbility.performed += TriggerActiveAbility_performed;
        inputActions.Player.ToggleAbilityLeft.performed += ToggleAbilityLeft_performed;
        inputActions.Player.ToggleAbilityRight.performed += ToggleAbilityRight_performed;

        CurrentMoveForce = BaseMoveForce;
    }

    protected virtual void ToggleAbilityRight_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!GameStateData.IsGameOver) GameEventSystem.Player_OnToggleAbilityRight(player);
    }

    protected virtual void ToggleAbilityLeft_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!GameStateData.IsGameOver) GameEventSystem.Player_OnToggleAbilityLeft(player);
    }

    protected virtual void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {    
        if (!GameStateData.IsGameOver) GameEventSystem.Player_OnPaused(player);
    }

    protected virtual void TriggerActiveAbility_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!GameStateData.IsGameOver) GameEventSystem.Player_OnTriggerActiveAbility(player);
    }

    protected virtual void FixedUpdate()
    {
        if (GameStateData.IsPaused)
        {
            rb.velocity = Vector3.zero;
        }

        if (inputActions != null && !GameStateData.IsPaused)
        {
            var input = inputActions.Player.Walk.ReadValue<Vector2>().normalized;
            if (input == Vector2.zero)
            {
                rb.velocity = Vector3.zero;
            }
            else
            {
                rb.AddForce(new Vector3(input.x, 0, input.y) * CurrentMoveForce, ForceMode.Acceleration);
            }
        }
    }
}
