using UnityEngine;

/*! 
 *  \author    Pavan Jando
 */

/// <summary>
/// Controls the player animation and equipping the correct weapon.
/// </summary>
public class PlayerController : MonoBehaviour {
    /// <summary>
    /// The animator for the player model.
    /// </summary>
    public Animator player;
    /// <summary>
    /// Reference to the enemy health script.
    /// </summary>
    public EnemyHealth enemyHealth;
    /// <summary>
    /// The game object that is parent of all of the different weapons.
    /// </summary>
    public GameObject melee;

    /// <summary>
    /// Finds the currently equipped weapon and makes it visible.
    /// </summary>
    void Start() {
        foreach (Item weapon in Inventory.instance.GetItems()) {
            if (weapon.isEquipped) {
                melee.transform.GetChild(weapon.id).gameObject.SetActive(true);
            } else {
                melee.transform.GetChild(weapon.id).gameObject.SetActive(false);
            }
        }
        enemyHealth = GameObject.Find("Battle").GetComponent<EnemyHealth>();
    }

    /// <summary>
    /// Detects the users swipe and plays the subsequent animation.
    /// Deals damage to enemy based on the weapon equipped.
    /// </summary>
    void Update() {
        if (player.GetCurrentAnimatorStateInfo(0).IsName("IdleWithWeapon")) {
            if (SwipeManager.IsSwipingLeft()) {
                player.SetTrigger("Hit01");
                enemyHealth.TakeDamage(Inventory.instance.getEquipped().lightDamage);
            } else if (SwipeManager.IsSwipingRight()) {
                player.SetTrigger("Hit02");
                enemyHealth.TakeDamage(Inventory.instance.getEquipped().heavyDamage);
            }
        }
    }
}
