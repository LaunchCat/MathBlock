using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    public void Activate()
    {
       LevelManager.instance.ResetLevel();
    }
}
