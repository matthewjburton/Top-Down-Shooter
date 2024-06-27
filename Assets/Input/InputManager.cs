using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public static PlayerInput PlayerInput { get; private set; }

    public Vector2 MoveInput { get; private set; }
    public bool AttackJustPressed { get; private set; }
    public bool AttackPressed { get; private set; }
    public Vector2 MouseInput { get; private set; }
    public bool PauseInput { get; private set; }
    public bool UnpauseInput { get; private set; }

    [Header("Input Action Maps")]
    InputActionMap gameMap;
    InputActionMap uiMap;

    [Header("Input Actions")]
    InputAction moveAction;
    InputAction attackAction;
    InputAction mouseAction;
    InputAction pauseAction;
    InputAction unpauseAction;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        // Get reference to player input
        PlayerInput = GetComponent<PlayerInput>();
        if (PlayerInput == null) Debug.Log("Player Input not found!");

        // Get references to action maps
        gameMap = GetActionMap("Game");
        uiMap = GetActionMap("UI");

        InitializeInputActions();
    }

    void InitializeInputActions()
    {
        // Game actions
        moveAction = GetActionFromMap("Move", gameMap);
        attackAction = GetActionFromMap("Attack", gameMap);
        mouseAction = GetActionFromMap("Mouse Position", gameMap);
        pauseAction = GetActionFromMap("Pause", gameMap);

        // UI actions
        unpauseAction = GetActionFromMap("Unpause", uiMap);
    }

    void Update()
    {
        UpdateInputs();
    }

    void UpdateInputs()
    {
        // Game inputs
        MoveInput = moveAction.ReadValue<Vector2>();
        AttackJustPressed = attackAction.WasPressedThisFrame();
        AttackPressed = attackAction.IsPressed();
        MouseInput = mouseAction.ReadValue<Vector2>();
        PauseInput = pauseAction.WasPressedThisFrame();

        // UI inputs
        UnpauseInput = unpauseAction.WasPressedThisFrame();
    }

    InputActionMap GetActionMap(string mapName)
    {
        return PlayerInput.actions.FindActionMap(mapName, true);
    }

    InputAction GetActionFromMap(string actionName, InputActionMap mapName)
    {
        return mapName.FindAction(actionName, true);
    }
}
