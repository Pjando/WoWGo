/*! 
 *  \author    Pavan Jando
 */

/// <summary>
/// Dagger weapon extends base class Item.
/// </summary>
class Dagger : Item {
    /// <summary>
    /// Creates a Dagger object with preset attribute values.
    /// </summary>
    public Dagger() {
        this.name = "dagger";
        this.lightDamage = 8;
        this.heavyDamage = 13;
        this.id = 1;
    }
}

