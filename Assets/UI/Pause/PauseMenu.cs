public class PauseMenu : Menu
{
    public void Resume()
    {
        PauseManager.Instance.Unpause();
        Destroy(transform.root.gameObject);
    }
}
