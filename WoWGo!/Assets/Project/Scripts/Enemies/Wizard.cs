/*! 
 *  \author    Pavan Jando
 */

/// <summary>
/// Wizard enemy extends base class enemy.
/// </summary>
class Wizard : Enemy {
    /// <summary>
    /// Creates empty Wizard object.
    /// </summary>
    public Wizard() { }
    /// <summary>
    /// Creates a Wizard object with default id = 2 and name = wizard.
    /// </summary>
    /// <param name="lat">Latitude of Wizard.</param>
    /// <param name="lon">Longitude of Wizard.</param>
    public Wizard(float lat, float lon) {
        id = 1;
        name = "wizard";
        this.lat = lat;
        this.lon = lon;
    }
}

