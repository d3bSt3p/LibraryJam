using UnityEngine;
/// <summary>
/// holds important data about an item, such as its type and sprite, also defines the item type enum.
/// part of the inventory system, which is based on the tutorial from Freedom Coding linked below, but modified to fit our needs
/// https://www.youtube.com/watch?v=HGol5qhqjOE
/// </summary>
public enum ItemType { Net, Jar, Book, MagnifyingGlass }

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class ItemSO : ScriptableObject
{
    [Header("Properties")]
    public float cooldown;
    public ItemType itemType;
    public Sprite itemSprite;
}

