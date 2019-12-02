using System.Collections.Generic;
using UnityEngine;
/*! 
 *  \author    Pavan Jando
 */

/// <summary>
/// Manages the inventory and which weapon is equipped.
/// </summary>
public class Inventory : MonoBehaviour {
    /// <summary>
    /// Global Singleton instance variable.
    /// </summary>
    public static Inventory instance;
    /// <summary>
    /// Number of items that can be in the inventory.
    /// </summary>
    public const int SIZE = 3;
    /// <summary>
    /// List of type Item containing each item in the inventory.
    /// </summary>
    List<Item> items = new List<Item>(SIZE);
    /// <summary>
    /// An axe object.
    /// </summary>
    Axe axe = new Axe();
    /// <summary>
    /// A mace object.
    /// </summary>
    Mace mace = new Mace();
    /// <summary>
    /// A dagger object.
    /// </summary>
    Dagger dagger = new Dagger();
    /// <summary>
    /// Create Singleton instance and adds axe, mace and dagger to inventory list.
    /// By default equips the axe weapon.
    /// </summary>
    private void Awake() {
        if (!instance) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Inventory.instance.AddItem(axe);
            Inventory.instance.AddItem(mace);
            Inventory.instance.AddItem(dagger);
            items.Find(x => x.name.Contains("axe")).isEquipped = true;
        } else {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Adds an item to the inventory.
    /// </summary>
    /// <param name="item">Item to be added</param>
    public void AddItem(Item item) {
        if (items.Count == 0) {
            item.isEquipped = true;
        }
        items.Add(item);
    }
    /// <summary>
    /// Remove an item from the inventory.
    /// </summary>
    /// <param name="item">Item to be removed.</param>
    public void RemoveItem(Item item) {
        items.Remove(item);
    }
    /// <summary>
    /// Gets the currently equipped item.
    /// </summary>
    /// <returns>Item where Item.isEquipped == true.</returns>
    public Item getEquipped() {
        return items.Find(x => x.isEquipped == true);
    }
    /// <summary>
    /// Equips an item and unequips any other items.
    /// </summary>
    /// <param name="name">The name of the item to be equipped</param>
    public void Equip(string name) {
        items.Find(x => x.isEquipped == true).isEquipped = false;
        items.Find(x => x.name.Contains(name)).isEquipped = true;
    }
    /// <summary>
    /// Returns all the inventory items.
    /// </summary>
    /// <returns>List of type Item containing all items</returns>
    public List<Item> GetItems() {
        return items;
    }
}
