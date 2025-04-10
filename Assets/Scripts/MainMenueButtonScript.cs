using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenueButtonScript : MonoBehaviour
{
    [SerializeField] private string gameSceneName;
    
    public void StartGamePressed()
    {
        Debug.Log($"{Pressed("New Game")}");
        SceneManager.LoadScene(gameSceneName);
    }
    #region Options
    public void OptionsPressed()
    {
        var text = Pressed("Options");
        Debug.Log(text);
    }
    #endregion

    private string Pressed(string button)
    {
        return button + " Button Pressed";
    }
    public static void QuitPressed()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                        Application.Quit();
        #endif
    }
}
