using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*! 
 *  \author    Sloan Kelly
 */
/// <summary>
/// Renders the roads on the map.
/// </summary>
class RoadMaker : InfrastructureBehaviour {
    /// <summary>
    /// The material used for each road.
    /// </summary>
    public Material road;
    /// <summary>
    /// The parent object for all of the roads.
    /// </summary>
    public GameObject parent;
    /// <summary>
    /// Obtains all of the ways and procedurally renders all of the roads.
    /// </summary>
    /// <returns>Returns null when all roads are rendered.</returns>
    private IEnumerator Start() {
        //Waits for all map data to be read.
        while (!map.IsReady) {
            yield return null;
        }
        //Each way in the list of ways that is a road (rather than a boundary).
        foreach (var way in map.ways.FindAll((w) => w.isRoad)) {
            //Creates new gameobject for the road.
            GameObject go = new GameObject(way.id.ToString());
            go.transform.SetParent(parent.transform);
            //Get the centre of way.
            Vector3 localOrigin = GetCentre(way);
            go.transform.position = localOrigin - map.bounds.centre;

            MeshFilter mf = go.AddComponent<MeshFilter>();
            MeshRenderer mr = go.AddComponent<MeshRenderer>();
            //Set material for each wall of a road.
            mr.material = road;

            List<Vector3> vectors = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> indices = new List<int>();
            //For every point in the way
            for (int i = 1; i < way.nodeIDs.Count; i++) {
                //Setting p1 to the previous point.
                OsmNode p1 = map.nodes[way.nodeIDs[i - 1]];
                //Setting p2 to the next point.
                OsmNode p2 = map.nodes[way.nodeIDs[i]];
                //Create vectors using 2 points p1 and p2
                Vector3 s1 = p1 - localOrigin;
                Vector3 s2 = p2 - localOrigin;

                //The vector that maps s1 to s2.
                Vector3 diff = (s2 - s1).normalized;
                //The width of the road
                var cross = Vector3.Cross(diff, Vector3.up) * 3;

                //The 4 corners of the road.
                Vector3 v1 = s1 + cross;
                Vector3 v2 = s1 - cross;
                Vector3 v3 = s2 + cross;
                Vector3 v4 = s2 - cross;
                
                //Add these vectors marking the indicies for the mesh filter.
                vectors.Add(v1);
                vectors.Add(v2);
                vectors.Add(v3);
                vectors.Add(v4);
               
                uvs.Add(new Vector2(0, 0));
                uvs.Add(new Vector2(1, 0));  
                uvs.Add(new Vector3(0, 1));
                uvs.Add(new Vector3(1, 1));
                
                //Add 4 normals for each vector.
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                
                //Each idx represents correspoding vector.
                int idx1, idx2, idx3, idx4;
                idx4 = vectors.Count - 1;
                idx3 = vectors.Count - 2;
                idx2 = vectors.Count - 3;
                idx1 = vectors.Count - 4;

                //Draw first triangle with vectors v1, v3, v2.
                indices.Add(idx1);
                indices.Add(idx3);
                indices.Add(idx2);

                //Draw second triangle with vectors v3, v4, v2.
                indices.Add(idx3);
                indices.Add(idx4);
                indices.Add(idx2);
            }

            mf.mesh.vertices = vectors.ToArray();
            mf.mesh.normals = normals.ToArray();
            mf.mesh.triangles = indices.ToArray();
            mf.mesh.uv = uvs.ToArray();

            yield return null;
        }
    }
}