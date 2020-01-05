/*! 
 *  \author    Pavan Jando
 */

/// <summary>
/// Base class for the enemies in the game.
/// </summary>
public abstract class Enemy {
    /// <summary>
    /// Identifier for an enemy.
    /// </summary>
    internal int id;
    /// <summary>
    /// The name of an enemy.
    /// </summary>
    internal string name;
    /// <summary>
    /// The latitude for an enemy.
    /// </summary>
    internal float lat;
    /// <summary>
    /// The longitude for an enemy.
    /// </summary>
    internal float lon;

    /// <summary>
    /// Uses the ByteBuffer class to serialise the enemy attributes into a byte array.
    /// </summary>
    /// <returns>Byte array containing enemy data.</returns>
    public byte[] Serialise() {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger(id);
        buffer.WriteString(name);
        buffer.WriteFloat(lat);
        buffer.WriteFloat(lon);

        return buffer.ToArray();
    }

    /// <summary>
    /// Outlines all of the enemies attributes.
    /// </summary>
    /// <returns>String of all the attributes of the enemy</returns>
    public override string ToString() {
        return "{ ID:" + id + ", Name:" + name + ", Lat:" + lat + ", Lon:" + lon + "}";
    }
}