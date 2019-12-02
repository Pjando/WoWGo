using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

/*! 
 *  \author    Pavan Jando
 */

/// <summary>
/// Controls the health bar of the enemy and when the battle scene is finished.
/// </summary>
public class EnemyHealth : MonoBehaviour {
    /// <summary>
    /// The amount of health the player starts the game with.
    /// </summary>
    public int startingHealth = 100;
    /// <summary>
    /// The current health the player has.
    /// </summary>
    public int currentHealth;
    /// <summary>
    /// Reference to the UI's health bar.
    /// </summary>
    public Slider healthSlider;                                
    /// <summary>
    /// The animator for the enemy model.
    /// </summary>
    Animator anim;
    /// <summary>
    /// Whether the enemy is alive.
    /// </summary>
    bool isAlive = true;
    /// <summary>
    /// True when the player gets damaged.
    /// </summary>
    bool damaged;                                              
    /// <summary>
    /// Instantiate the animator and set the current health to starting health.
    /// </summary>
    void Start() {
        EnemyController enemy = GameObject.Find("Battle").GetComponent<EnemyController>();
        anim = enemy.currentEnemy.GetComponent<Animator>();
        currentHealth = startingHealth;
    }
    /// <summary>
    /// Reset damaged boolean to false after damage was taken.
    /// </summary>
    void Update() {
        damaged = false;
    }
    /// <summary>
    /// Decreases the enemy health by a given amount.
    /// Also triggers damage animation.
    /// </summary>
    /// <param name="amount">The amount of damage to subtract</param>
    public void TakeDamage(int amount) {
        // Set the damaged flag so the screen will flash.
        damaged = true;

        // Reduce the current health by the damage amount.
        currentHealth -= amount;

        // Set the health bar's value to the current health.
        healthSlider.value = currentHealth;

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (currentHealth <= 0) {
            // ... it should die.
            StartCoroutine(Death());
        } else if (currentHealth >= 0 && isAlive) {
            anim.SetTrigger("damage_001");
        }
    }
    /// <summary>
    /// Waits for the death animation to complete before changing to map scene.
    /// </summary>
    /// <returns>Waits for "dead" animation to play, then waits for the animation to finish.</returns>
    IEnumerator Death() {
        // Set the death flag so this function won't be called again.
        isAlive = false;
        // Tell the animator that the player is dead.
        anim.SetTrigger("dead");
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("dead"));
        
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadSceneAsync("Map Reader Test");
        yield break;
    }
}