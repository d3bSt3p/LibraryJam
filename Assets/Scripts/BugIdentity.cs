using UnityEngine;

/// <summary>
/// Attach this to a bug prefab to identify which type of bug it is.
/// </summary>
public class BugIdentity : MonoBehaviour
{
    [Tooltip("The scriptable object that holds this bug's data.")]
    public BugDataSO bugData;
}

