using UnityEngine;
using UnityEngine.SceneManagement;

public class doorManager : MonoBehaviour
{
    [Header("Scene Transition")]
    [SerializeField] private string targetScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(targetScene);
        }
    }
}
    