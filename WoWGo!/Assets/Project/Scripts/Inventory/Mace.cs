/*! 
 *  \author    Pavan Jando
 */

/// <summary>
/// Mace weapon extends base class Item.
/// </summary>
class Mace : Item {
    /// <summary>
    /// Creates a Mace object with preset attribute values.
    /// </summary>
    public Mace() {
        this.name = "mace";
        this.lightDamage = 10;
        this.heavyDamage = 17;
        this.id = 2;
    }
}

