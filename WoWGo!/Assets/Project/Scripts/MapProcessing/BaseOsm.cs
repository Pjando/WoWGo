using System;
using System.Xml;

/*! 
 *  \author    Sloan Kelly
 */
/// <summary>
/// The base class for each Osm attribute.
/// </summary>
class BaseOsm {
    /// <summary>
    /// Gets the value of an xml tag given the collection of attributes for a node
    /// and converts it to type T.
    /// </summary>
    /// <typeparam name="T">The type of the value to return.</typeparam>
    /// <param name="attrName">The string name of the tag in the Xml attributes collection.</param>
    /// <param name="attributes">The attributes of an XmlNode.</param>
    /// <returns>The value contained within the Xml tag of type T.</returns>
    protected T GetAttribute<T>(string attrName, XmlAttributeCollection attributes) {
        string strValue = attributes[attrName].Value;
        return (T) Convert.ChangeType(strValue, typeof(T));
    }
}
