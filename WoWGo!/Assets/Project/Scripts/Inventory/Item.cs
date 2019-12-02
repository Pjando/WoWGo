using UnityEngine;
/*! 
 *  \author    Pavan Jando
 */

/// <summary>
/// Base class for a weapon.
/// </summary>
public abstract class Item {
    /// <summary>
    /// The name of the weapon.
    /// </summary>
    internal string name;
    /// <summary>
    /// The amount of damage a light attack does.
    /// </summary>
    internal int lightDamage;
    /// <summary>
    /// The amount of damage a heavy attack does.
    /// </summary>
    internal int heavyDamage;
    /// <summary>
    /// Whether the weapon is equipped or not.
    /// </summary>
    internal bool isEquipped = false;
    /// <summary>
    /// Identifier for a weapon.
    /// </summary>
    internal int id;
}