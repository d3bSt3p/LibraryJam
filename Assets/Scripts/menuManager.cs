using UnityEngine;

public class menuManager : MonoBehaviour
{
    // Refrence to the Jar menu game object, to show and hide it when needed.
    [SerializeField] private GameObject jarMenu;
    [SerializeField] private GameObject bookMenu;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ShowJar()
    {
        // show the jar menu
        jarMenu.SetActive(true);
    }
    public void HideJar()
    {
        // hide the jar menu
        jarMenu.SetActive(false);
    }
    public void ShowBook()
    {
        // show the book menu
        bookMenu.SetActive(true);
    }
    public void HideBook()
    {
        // hide the book menu
        bookMenu.SetActive(false);
    }
}