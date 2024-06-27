using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    [SerializeField] GameObject pauseMenu;
    GameObject menuInstance;

    public bool IsPaused { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (InputManager.Instance.PauseInput || InputManager.Instance.UnpauseInput)
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        if (IsPaused)
        {
            Unpause();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0;
        InputManager.PlayerInput.SwitchCurrentActionMap("UI");
        menuInstance = Instantiate(pauseMenu, transform.position, Quaternion.identity);
    }

    public void Unpause()
    {
        IsPaused = false;
        Time.timeScale = 1;
        InputManager.PlayerInput.SwitchCurrentActionMap("Game");

        if (menuInstance)
            Destroy(menuInstance);
    }
}
