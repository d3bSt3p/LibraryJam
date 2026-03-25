using UnityEngine;
using UnityEngine.UI;

public class JarController : MonoBehaviour
{
    [Header("Butterfly Settings")]
    [SerializeField] GameObject jarButterflyPrefab;
    [SerializeField] int jarCapacity = 10;

    [Header("Spawn Area")]
    [SerializeField] RectTransform spawnArea;

    private int _currentCount = 0;

    public bool IsFull => _currentCount >= jarCapacity;

    public bool AddButterfly(BugDataSO bugData)
    {
        if (IsFull)
        {
            Debug.LogWarning("Jar is full!");
            return false;
        }

        Vector2 spawnPos = GetRandomPositionInSpawnArea();

        GameObject butterfly = Instantiate(jarButterflyPrefab, spawnArea);
        butterfly.GetComponent<RectTransform>().anchoredPosition = spawnPos;
        butterfly.GetComponent<JarButterfly>()?.Initialise(bugData, spawnArea);

        _currentCount++;
        return true;
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