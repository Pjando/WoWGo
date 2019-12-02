/*! 
 *  \author    Pavan Jando
 */

/// <summary>
/// Skeleton enemy extends base class enemy.
/// </summary>
class Skeleton : Enemy {
    /// <summary>
    /// Creates empty Skeleton object.
    /// </summary>
    public Skeleton() { }
    /// <summary>
    /// Creates a Skeleton object with default id = 2 and name = skeleton.
    /// </summary>
    /// <param name="lat">Latitude of Skeleton.</param>
    /// <param name="lon">Longitude of Skeleton.</param>
    public Skeleton(float lat, float lon) {
        id = 2;
        name = "skeleton";
        this.lat = lat;
        this.lon = lon;
    }
}

