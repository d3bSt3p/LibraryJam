using UnityEngine;
/// <summary>
/// this is a script for handling special pause funcitons for the ASU 2025 Library Game Jam
/// </summary>
public class PauseFunctions : MonoBehaviour
{
    // grab a refrence to the hand game object (atached to the player controller)
    [SerializeField] GameObject hand;

    // function to toggle the hand on and off when the game is paused and unpaused
    public void ToggleHand()
    {
        if (hand.activeSelf)
        {
            hand.SetActive(false);
        }
        else
        {
            hand.SetActive(true);
        }
    }
}
