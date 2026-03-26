using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the player's item inventory.
/// Supports picking up items, switching with number keys 1-9,
/// and throwing the currently selected item.
/// Script should be attached to the player GameObject.
/// based on tutorial from Freedom Coding linked below, but modified to fit our needs
/// https://www.youtube.com/watch?v=HGol5qhqjOE
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    // ── Inventory state ─────────────────────────────────────────────────────
    public List<ItemType> inventoryList = new List<ItemType>();
    public int selectedItem;

    // ── Inspector references ─────────────────────────────────────────────────
    [Header("Player Settings")]
    [SerializeField] private Camera cam;
    [SerializeField] private int playerReach = 3;

    [Header("Input")]
    [SerializeField] private KeyCode pickUpItemKey = KeyCode.E;
    [SerializeField] private KeyCode throwItemKey = KeyCode.Q;
    [SerializeField] private bool canDropItems = true;

    [Header("UI")]
    [SerializeField] private GameObject pressToPickupUI;
    [SerializeField] private Image[] inventorySlotImages = new Image[9];
    [SerializeField] private Image[] inventoryBackgroundImages = new Image[9];
    [SerializeField] private Sprite emptySlotSprite;

    [Header("Throw Spawn Point")]
    [SerializeField] private Transform throwSpawnPoint;

    [Header("Weapon GameObjects (held by player)")]
    [SerializeField] private GameObject NetItem;
    [SerializeField] private GameObject JarItem;
    [SerializeField] private GameObject BookItem;
    [SerializeField] private GameObject HandItem;

    [Header("Weapon Prefabs (spawned when thrown)")]
    [SerializeField] private GameObject NetPrefab;
    [SerializeField] private GameObject JarPrefab;
    [SerializeField] private GameObject BookPrefab;
    [SerializeField] private GameObject HandPrefab;
    
    [Header("Components")]
    [SerializeField] private BugCatcher bugCatcher;

    // ── Private helpers ──────────────────────────────────────────────────────
    private Dictionary<ItemType, GameObject> _heldItemObjects;
    private Dictionary<ItemType, GameObject> _itemPrefabs;

    private static readonly Color32 SelectedColor   = new Color32(145, 255, 126, 255);
    private static readonly Color32 UnselectedColor = new Color32(219, 219, 219, 255);

    // ────────────────────────────────────────────────────────────────────────
    void Start()
    {
        _heldItemObjects = new Dictionary<ItemType, GameObject>
        {
            { ItemType.Net,  NetItem  },
            { ItemType.Jar, JarItem },
            { ItemType.Book,    BookItem    },
            { ItemType.Hand,    HandItem    },
        };

        _itemPrefabs = new Dictionary<ItemType, GameObject>
        {
            { ItemType.Net,  NetPrefab  },
            { ItemType.Jar, JarPrefab },
            { ItemType.Book,    BookPrefab    },
            { ItemType.Hand,    HandPrefab    },
        };

        RefreshHeldItem();
    }

    // ────────────────────────────────────────────────────────────────────────
    void Update()
    {
        HandleNumberKeySelection();
        HandleThrow();
        HandlePickup();
        RefreshUI();
    }

    // ── Number key selection (1–9) ───────────────────────────────────────────
    private void HandleNumberKeySelection()
    {
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) && i < inventoryList.Count)
            {
                selectedItem = i;
                RefreshHeldItem();
                break;
            }
        }
    }

    // ── Throw ────────────────────────────────────────────────────────────────
    private void HandleThrow()
    {
        if (!canDropItems) return;
        if (!Input.GetKeyDown(throwItemKey)) return;
        if (inventoryList.Count <= 1) return; // keep at least one item

        Vector3 spawnPos = throwSpawnPoint != null
            ? throwSpawnPoint.position
            : transform.position;

        Instantiate(_itemPrefabs[inventoryList[selectedItem]], spawnPos, Quaternion.identity);

        inventoryList.RemoveAt(selectedItem);

        // Clamp selectedItem so it stays valid
        if (selectedItem >= inventoryList.Count)
            selectedItem = inventoryList.Count - 1;

        RefreshHeldItem();
    }

    // ── Pickup ───────────────────────────────────────────────────────────────
    private void HandlePickup()
    {
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        bool showUI = false;

        if (Physics.Raycast(ray, out RaycastHit hit, playerReach))
        {
            IPickable pickable = hit.collider.GetComponent<IPickable>();
            if (pickable != null)
            {
                showUI = true;
                if (Input.GetKeyDown(pickUpItemKey))
                {
                    ItemPickable ip = hit.collider.GetComponent<ItemPickable>();
                    if (ip != null && ip.weaponScriptableObject != null)
                        inventoryList.Add(ip.weaponScriptableObject.itemType);

                    pickable.PickItem();
                    showUI = false;
                }
            }
        }

        if (pressToPickupUI != null)
            pressToPickupUI.SetActive(showUI);
    }

    // ── UI refresh ───────────────────────────────────────────────────────────
    private void RefreshUI()
    {
        // Slot sprites
        for (int i = 0; i < inventorySlotImages.Length; i++)
        {
            if (inventorySlotImages[i] == null) continue;

            if (i < inventoryList.Count)
            {
                Item heldItem = _heldItemObjects[inventoryList[i]].GetComponent<Item>();
                inventorySlotImages[i].sprite = heldItem != null && heldItem.itemScriptableObject != null
                    ? heldItem.itemScriptableObject.itemSprite
                    : emptySlotSprite;
            }
            else
            {
                inventorySlotImages[i].sprite = emptySlotSprite;
            }
        }

        // Slot background highlight
        for (int i = 0; i < inventoryBackgroundImages.Length; i++)
        {
            if (inventoryBackgroundImages[i] == null) continue;
            inventoryBackgroundImages[i].color = (i == selectedItem) ? SelectedColor : UnselectedColor;
        }
    }

    // ── Switch held weapon ───────────────────────────────────────────────────
    private void RefreshHeldItem()
    {
        // Hide all
        foreach (var kvp in _heldItemObjects)
            if (kvp.Value != null) kvp.Value.SetActive(false);

        // Show selected
        if (inventoryList.Count > 0)
        {
            ItemType currentItem = inventoryList[selectedItem];

            GameObject go = _heldItemObjects[currentItem];
            if (go != null) go.SetActive(true);

            // Enable BugCatcher only when Net is equipped
            if (bugCatcher != null)
                bugCatcher.enabled = currentItem == ItemType.Net;
        }
        else
        {
            // No items in inventory, disable BugCatcher
            if (bugCatcher != null)
                bugCatcher.enabled = false;
        }
    }
}

