using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Interfaz final")]
    [SerializeField]
    private GameObject completedPanel;


    [Header("Escena final")]
    [SerializeField]
    private string finalSceneName = "03_FinalSimpsons";

    [SerializeField]
    private float delayBeforeFinalScene = 5f;


    [Header("Estado del juego - Depuración")]
    [SerializeField]
    private bool donutsCompleted = false;

    [SerializeField]
    private bool duffCompleted = false;


    private bool gameCompleted = false;


    // =====================================================
    // START
    // =====================================================

    private void Start()
    {
        if (completedPanel != null)
        {
            completedPanel.SetActive(false);
        }
    }


    // =====================================================
    // DONAS COMPLETADAS
    // =====================================================

    public void CompleteDonuts()
    {
        if (donutsCompleted)
            return;

        donutsCompleted = true;

        Debug.Log(
            "JUEGO: Todas las donas fueron encontradas."
        );

        CheckGameCompleted();
    }


    // =====================================================
    // DUFF COMPLETADA
    // =====================================================

    public void CompleteDuff()
    {
        if (duffCompleted)
            return;

        duffCompleted = true;

        Debug.Log(
            "JUEGO: Duff colocada correctamente."
        );

        CheckGameCompleted();
    }


    // =====================================================
    // COMPROBAR OBJETIVOS
    // =====================================================

    private void CheckGameCompleted()
    {
        Debug.Log(
            "Estado -> Donas: "
            + donutsCompleted
            + " | Duff: "
            + duffCompleted
        );


        if (
            donutsCompleted &&
            duffCompleted &&
            !gameCompleted
        )
        {
            gameCompleted = true;

            StartCoroutine(
                CompleteGameSequence()
            );
        }
    }


    // =====================================================
    // FINAL DEL JUEGO
    // =====================================================

    private IEnumerator CompleteGameSequence()
    {
        Debug.Log(
            "¡JUEGO COMPLETADO!"
        );


        if (completedPanel != null)
        {
            completedPanel.SetActive(true);
        }


        yield return new WaitForSeconds(
            delayBeforeFinalScene
        );


        SceneManager.LoadScene(
            finalSceneName
        );
    }
}