using UnityEngine;
using UnityEngine.UI;

public class JarController : MonoBehaviour
{
    [Header("Butterfly Settings")]
    [SerializeField] GameObject jarButterflyPrefab;

    [Header("Spawn Area")]
    [SerializeField] RectTransform spawnArea;

    // ── Lifecycle ────────────────────────────────────────────────────────────
    private void Start()
    {
        if (BugJar.Instance == null) return;

        foreach (BugDataSO bugData in BugJar.Instance.StoredBugs)
            SpawnButterflyVisual(bugData);
        //Debug.Log($"JarController: Spawned visuals for {BugJar.Instance.Count} stored bugs.");
    }

    // ── Public API ───────────────────────────────────────────────────────────

    /// <summary>Spawns a visual butterfly in the jar UI. Call after BugJar.TryAddBug succeeds.</summary>
    public void SpawnButterflyVisual(BugDataSO bugData)
    {
        Vector2 spawnPos = GetRandomPositionInSpawnArea();

        GameObject butterfly = Instantiate(jarButterflyPrefab, spawnArea);
        butterfly.GetComponent<RectTransform>().anchoredPosition = spawnPos;
        butterfly.GetComponent<JarButterfly>()?.Initialise(bugData, spawnArea);
    }

    /// <summary>Destroys all butterfly visuals in the jar and clears the BugJar data.</summary>
    public void ClearJar()
    {
        foreach (Transform child in spawnArea)
            Destroy(child.gameObject);

        BugJar.Instance?.ClearJar();
    }

    public Vector2 GetRandomPositionInSpawnArea()
    {
        Vector3[] corners = new Vector3[4];
        spawnArea.GetLocalCorners(corners);
        // corners[0] = bottom-left, corners[2] = top-right (local space)
        return new Vector2(
            Random.Range(corners[0].x, corners[2].x),
            Random.Range(corners[0].y, corners[2].y)
        );
    }
}