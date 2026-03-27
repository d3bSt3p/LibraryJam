using System.Collections.Generic;
using UnityEngine;

public class bookManager : MonoBehaviour
{
    // ── Inspector references ─────────────────────────────────────────────────
    [Header("Book Pages")]
    [SerializeField] private List<GameObject> pages = new List<GameObject>();

    // ── Page state ───────────────────────────────────────────────────────────
    private int _currentPageIndex = 0;

    // ────────────────────────────────────────────────────────────────────────
    void Start()
    {
        ShowPage(_currentPageIndex);
    }

    // ── Page navigation ──────────────────────────────────────────────────────
    public void NextPage()
    {
        if (_currentPageIndex < pages.Count - 1)
            ShowPage(_currentPageIndex + 1);
    }

    public void PreviousPage()
    {
        if (_currentPageIndex > 0)
            ShowPage(_currentPageIndex - 1);
    }

    private void ShowPage(int index)
    {
        for (int i = 0; i < pages.Count; i++)
        {
            if (pages[i] != null)
                pages[i].SetActive(i == index);
        }

        _currentPageIndex = index;
    }
}
