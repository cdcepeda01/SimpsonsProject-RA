using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("Elementos visuales")]

    [SerializeField]
    private GameObject instructionsText;

    [SerializeField]
    private GameObject completedPanel;


    [Header("Escena principal")]

    [SerializeField]
    private string gameSceneName = "02_JuegoSimpsons";

    [Tooltip("Tiempo que permanece visible el mensaje de felicitaciones.")]
    [SerializeField]
    private float delayBeforeGame = 5f;


    [Header("Estado del tutorial")]

    [SerializeField]
    private bool donutsCompleted = false;

    [SerializeField]
    private bool duffCompleted = false;


    private bool tutorialCompleted = false;


    // =====================================================
    // INICIO
    // =====================================================

    private void Start()
    {
        // Instrucciones visibles al comenzar.
        if (instructionsText != null)
        {
            instructionsText.SetActive(true);
        }

        // Felicitaciones ocultas.
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
            "TUTORIAL: Objetivo de donas completado."
        );

        CheckTutorialCompleted();
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
            "TUTORIAL: Objetivo Duff completado."
        );

        CheckTutorialCompleted();
    }


    // =====================================================
    // COMPROBAR TUTORIAL
    // =====================================================

    private void CheckTutorialCompleted()
    {
        if (
            donutsCompleted &&
            duffCompleted &&
            !tutorialCompleted
        )
        {
            tutorialCompleted = true;

            StartCoroutine(
                CompleteTutorialSequence()
            );
        }
    }


    // =====================================================
    // SECUENCIA FINAL
    // =====================================================

    private IEnumerator CompleteTutorialSequence()
    {
        Debug.Log(
            "¡TUTORIAL COMPLETADO!"
        );


        // Ocultamos las instrucciones 3D.
        if (instructionsText != null)
        {
            instructionsText.SetActive(false);
        }


        // Mostramos el mensaje de felicitaciones.
        if (completedPanel != null)
        {
            completedPanel.SetActive(true);
        }


        // Esperamos 5 segundos.
        yield return new WaitForSeconds(
            delayBeforeGame
        );


        // Cargamos el juego principal.
        SceneManager.LoadScene(
            gameSceneName
        );
    }
}