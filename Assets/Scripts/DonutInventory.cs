using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DonutInventory : MonoBehaviour
{
    public static DonutInventory Instance;

    [Header("Donas")]
    [SerializeField]
    private int totalDonuts = 4;

    [Header("Interfaz")]
    [SerializeField]
    private TMP_Text counterText;

    [Header("Donas visibles sobre la mesa")]
    [Tooltip("Donas decorativas que aparecerán a medida que el jugador recoja las donas.")]
    [SerializeField]
    private GameObject[] displayedDonuts;

    [Header("Eventos")]
    public UnityEvent OnAllDonutsCollected;


    public int CollectedDonuts { get; private set; }


    public int TotalDonuts
    {
        get
        {
            return totalDonuts;
        }
    }


    public bool HasAllDonuts
    {
        get
        {
            return CollectedDonuts >= totalDonuts;
        }
    }


    // =====================================================
    // AWAKE
    // =====================================================

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    // =====================================================
    // START
    // =====================================================

    private void Start()
    {
        CollectedDonuts = 0;

        // Ocultamos inicialmente todas las donas de la mesa.
        HideDisplayedDonuts();

        UpdateCounter();
    }


    // =====================================================
    // ON DESTROY
    // =====================================================

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }


    // =====================================================
    // RECOGER UNA DONA
    // =====================================================

    public void CollectDonut()
    {
        if (CollectedDonuts >= totalDonuts)
            return;


        // El índice de la siguiente dona que debe aparecer.
        int donutIndex = CollectedDonuts;


        // Sumamos la dona al inventario.
        CollectedDonuts++;


        // Mostramos una dona sobre la mesa.
        ShowDisplayedDonut(donutIndex);


        // Actualizamos la interfaz.
        UpdateCounter();


        Debug.Log(
            "Dona recogida: " +
            CollectedDonuts +
            " / " +
            totalDonuts
        );


        // Si ya recogimos todas:
        if (HasAllDonuts)
        {
            Debug.Log(
                "¡Todas las donas fueron recogidas!"
            );

            OnAllDonutsCollected?.Invoke();
        }
    }


    // =====================================================
    // MOSTRAR DONA SOBRE LA MESA
    // =====================================================

    private void ShowDisplayedDonut(int index)
    {
        if (displayedDonuts == null)
            return;

        if (
            index < 0 ||
            index >= displayedDonuts.Length
        )
        {
            return;
        }

        if (displayedDonuts[index] != null)
        {
            displayedDonuts[index].SetActive(true);
        }
    }


    // =====================================================
    // OCULTAR TODAS AL INICIO
    // =====================================================

    private void HideDisplayedDonuts()
    {
        if (displayedDonuts == null)
            return;

        foreach (GameObject donut in displayedDonuts)
        {
            if (donut != null)
            {
                donut.SetActive(false);
            }
        }
    }


    // =====================================================
    // LIMPIAR INVENTARIO
    // =====================================================

    public void ClearInventory()
    {
        CollectedDonuts = 0;

        HideDisplayedDonuts();

        UpdateCounter();
    }


    // =====================================================
    // ACTUALIZAR HUD
    // =====================================================

    private void UpdateCounter()
    {
        if (counterText != null)
        {
            counterText.text =
                CollectedDonuts +
                " / " +
                totalDonuts;
        }
    }
}