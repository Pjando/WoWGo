using UnityEngine;

/*! 
 *  \author    Pavan Jando
 */

/// <summary>
/// Controls the user interface elements in the map scene.
/// </summary>
public class UIController : MonoBehaviour {

    /// <summary>
    /// The parent object for the inventory menu.
    /// </summary>
    public GameObject inventoryUI;
    /// <summary>
    /// The parent object of the inventory slots.
    /// </summary>
    public GameObject parentItems;

    /// <summary>
    /// Calls checkItem()
    /// </summary>
    private void Start() {
        checkItem();
    }

    /// <summary>
    /// Connected to the inventory icon and toggles the inventory menu.
    /// </summary>
    public void OpenInventory() {
        if (inventoryUI.activeSelf)
            inventoryUI.SetActive(false);
        else {
            inventoryUI.SetActive(true);
        }
    }

    /// <summary>
    /// Equips the item that the user has selected.
    /// </summary>
    /// <param name="weapon">String name of the weapon selected.</param>
    public void EquipItem(string weapon) {
        Inventory.instance.Equip(weapon);
        checkItem();
    }

    /// <summary>
    /// Puts a check symbol above the currently equipped item.
    /// </summary>
    void checkItem() {
        foreach (Item temp in Inventory.instance.GetItems()) {
            if (temp.isEquipped) {
                parentItems.transform.GetChild(temp.id).GetChild(0).GetChild(1).gameObject.SetActive(true);
            } else {
                parentItems.transform.GetChild(temp.id).GetChild(0).GetChild(1).gameObject.SetActive(false);
            }
        }
    }
}
