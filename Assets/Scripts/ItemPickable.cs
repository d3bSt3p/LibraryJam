using UnityEngine;

/// <summary>
/// Attach this to weapon pickups in the world.
/// part of the inventory system, which is based on the tutorial from Freedom Coding linked below, but modified to fit our needs
/// https://www.youtube.com/watch?v=HGol5qhqjOE
/// </summary>
public class ItemPickable : MonoBehaviour, IPickable
{
    public ItemSO weaponScriptableObject;

    public void PickItem()
    {
        Destroy(gameObject);
    }
}

