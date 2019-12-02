/*! 
 *  \author    Pavan Jando
 */

/// <summary>
/// Axe weapon extends base class Item.
/// </summary>
class Axe : Item {
    /// <summary>
    /// Creates an Axe object with preset attribute values.
    /// </summary>
    public Axe() {
        this.name = "axe";
        this.lightDamage = 12;
        this.heavyDamage = 16;
        this.id = 0;
    }
}

