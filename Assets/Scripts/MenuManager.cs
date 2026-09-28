using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Nombres de escenas")]
    [SerializeField] private string escenaJuego = "02_JuegoSimpsons";
    [SerializeField] private string escenaTutorial = "01_TutorialSimpsons";

    public void Jugar()
    {
        PlayButtonSound();

        SceneManager.LoadScene(escenaJuego);
    }

    public void Tutorial()
    {
        PlayButtonSound();

        SceneManager.LoadScene(escenaTutorial);
    }

    public void Salir()
    {
        PlayButtonSound();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void PlayButtonSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonSelect();
        }
    }
}