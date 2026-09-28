using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiodeEscena : MonoBehaviour
{
        
    public void CambiarEscena(int numeroEscena)
    {
        SceneManager.LoadScene(numeroEscena);
    }
}
