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

    [Header("Catch Display")]
    [Tooltip("The image shown on screen when this bug is caught.")]
    public Sprite catchImage;
}

