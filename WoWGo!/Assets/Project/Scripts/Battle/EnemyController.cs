using UnityEngine;
using TMPro;

/*! 
 *  \author    Pavan Jando
 */

/// <summary>
/// Controls the enemy's actions during the battle phase.
/// </summary>
public class EnemyController : MonoBehaviour {

    /// <summary>
    /// Name of the enemy.
    /// </summary>
    public TMP_Text name;
    /// <summary>
    /// The prefab used to spawn the wizard model.
    /// </summary>
    public GameObject wizard;
    /// <summary>
    /// The prefab used to spawn the skeleton model.
    /// </summary>
    public GameObject skeleton;
    /// <summary>
    /// Holds the current enemy that needs to be spawned.
    /// </summary>
    internal GameObject currentEnemy;
    /// <summary>
    /// Contains the Enemy Manager component used to get the current enemy.
    /// </summary>
    EnemyManager em;
    /// <summary>
    /// The animator for the given model.
    /// </summary>
    Animator animator;

    /// <summary>
    /// Checks to see which is the enemy the user clicked on and spawns it
    /// in idle animation for the main battle phase.
    /// </summary>
    void Awake() {
        em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();

        Vector3 pos = new Vector3(0.8f, 3.26f, 14.7f);

        if (em.currentEnemy == "wizard(Clone)") {
            currentEnemy = Instantiate(wizard, pos, transform.rotation);
            currentEnemy.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            animator = currentEnemy.GetComponent<Animator>();
            name.text = "Dark Sorcerer";

        } else if (em.currentEnemy == "skeleton(Clone)") {
            currentEnemy = Instantiate(skeleton, pos, transform.rotation);
            currentEnemy.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            animator = currentEnemy.GetComponent<Animator>();
            name.text = "Skelebob";
        }

        animator.SetBool("idle_combat", true);
        MoveToLayer(currentEnemy.transform, 9);
    }

    /// <summary>
    /// Recursive function that sets the layer for a root transform and all child transforms
    /// Layer used to render the model before the background.
    /// </summary>
    /// <param name="root">The root transform</param>
    /// <param name="layer">The layer to apply to the root and children transforms</param>
    void MoveToLayer(Transform root, int layer) {
        root.gameObject.layer = layer;
        foreach (Transform child in root)
            MoveToLayer(child, layer);
    }
}
