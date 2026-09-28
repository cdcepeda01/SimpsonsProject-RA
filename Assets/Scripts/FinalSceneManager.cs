using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalSceneManager : MonoBehaviour
{
    [Header("Configuración")]

    [SerializeField]
    private string menuSceneName =
        "00_MenuSimpsons";

    [SerializeField]
    private float timeBeforeMenu =
        8f;


    private void Start()
    {
        StartCoroutine(
            ReturnToMenu()
        );
    }


    private IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(
            timeBeforeMenu
        );

        SceneManager.LoadScene(
            menuSceneName
        );
    }
}