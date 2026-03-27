using System;
using UnityEngine;

/// <summary>
/// Stores data about a specific type of bug/butterfly.
/// Assign one of these to each bug prefab via the BugIdentity component.
/// </summary>
[CreateAssetMenu(fileName = "BugData", menuName = "Scriptable Objects/Bug Data")]
public class BugDataSO : ScriptableObject
{
    [Header("Bug Info")]
    public string bugName;

    [Tooltip("The scientific/latin name of this bug.")]
    public string bugScientificName;

    [TextArea(2, 4)]
    [Tooltip("A fun fact displayed when this bug is caught or inspected.")]
    public string bugFact;

    [Header("Catch Stats")]
    [Tooltip("How many of this bug the player has caught. Saved and loaded across sessions.")]
    [SerializeField] private int amountCaught;

    [Tooltip("The target number to catch for the achievement to unlock.")]
    public int catchGoal = 10;

    [Header("Catch Display")]
    [Tooltip("The image shown on screen when this bug is caught.")]
    public Sprite catchImage;

    [Header("Inspect View")]
    [Tooltip("The 3D prefab spawned when inspecting this bug.")]
    public GameObject inspectPrefab;

    /// <summary>How many of this bug have been caught.</summary>
    public int AmountCaught => amountCaught;

    /// <summary>Returns true when the catch goal has been reached.</summary>
    public bool GoalReached => amountCaught >= catchGoal;

    /// <summary>Increments the caught counter by one.</summary>
    public void RegisterCatch() => amountCaught++;

    /// <summary>Directly sets the caught count, e.g. when loading a save.</summary>
    public void SetAmountCaught(int value) => amountCaught = Mathf.Max(0, value);
}

