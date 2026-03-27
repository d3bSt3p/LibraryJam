using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Persistent singleton that tracks all bugs stored in the jar across scenes.
/// Attach to a dedicated GameObject; it will survive scene loads automatically.
/// </summary>
public class BugJar : MonoBehaviour
{
    public static BugJar Instance { get; private set; }

    [Header("Jar Settings")]
    [SerializeField] private int jarCapacity = 10;

    private readonly List<BugDataSO> _storedBugs = new List<BugDataSO>();

    public IReadOnlyList<BugDataSO> StoredBugs => _storedBugs;
    public int Count => _storedBugs.Count;
    public bool IsFull => _storedBugs.Count >= jarCapacity;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>Attempts to add a bug to the jar. Returns true on success.</summary>
    public bool TryAddBug(BugDataSO bugData)
    {
        if (IsFull)
        {
            Debug.LogWarning("BugJar is full!");
            return false;
        }

        _storedBugs.Add(bugData);
        Debug.Log($"BugJar: added '{bugData.bugName}' ({Count}/{jarCapacity})");
        return true;
    }

    /// <summary>Removes all bugs from the jar.</summary>
    public void ClearJar()
    {
        Debug.LogWarning("Clearing BugJar!");
        _storedBugs.Clear();
    }
    
}