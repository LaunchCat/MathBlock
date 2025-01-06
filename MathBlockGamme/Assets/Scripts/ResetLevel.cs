using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    public void Activate()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
