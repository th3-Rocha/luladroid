using UnityEngine;

public class DestroyTerrainsDetails : MonoBehaviour
{
    public Terrain terrain; // Reference to the terrain
    private int[,] originalDetails; // Backup of the original detail layer

    void Start()
    {
        if (terrain != null)
        {
            // Backup the original details
            TerrainData terrainData = terrain.terrainData;
            originalDetails = terrainData.GetDetailLayer(0, 0, terrainData.detailWidth, terrainData.detailHeight, 0);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (terrain == null || originalDetails == null) return;

        TerrainData terrainData = terrain.terrainData;

        // Get the bounds of the object's collider
        Bounds colliderBounds = collision.collider.bounds;

        // Convert bounds from world space to terrain-local space
        Vector3 terrainPositionMin = colliderBounds.min - terrain.transform.position;
        Vector3 terrainPositionMax = colliderBounds.max - terrain.transform.position;

        // Normalize bounds to the range [0, 1]
        float normalizedMinX = Mathf.Clamp01(terrainPositionMin.x / terrainData.size.x);
        float normalizedMinZ = Mathf.Clamp01(terrainPositionMin.z / terrainData.size.z);
        float normalizedMaxX = Mathf.Clamp01(terrainPositionMax.x / terrainData.size.x);
        float normalizedMaxZ = Mathf.Clamp01(terrainPositionMax.z / terrainData.size.z);

        // Convert normalized bounds to detail coordinates
        int detailMinX = Mathf.RoundToInt(normalizedMinX * (terrainData.detailWidth - 1));
        int detailMinZ = Mathf.RoundToInt(normalizedMinZ * (terrainData.detailHeight - 1));
        int detailMaxX = Mathf.RoundToInt(normalizedMaxX * (terrainData.detailWidth - 1));
        int detailMaxZ = Mathf.RoundToInt(normalizedMaxZ * (terrainData.detailHeight - 1));

        // Limit the bounds to avoid processing huge areas
        int maxArea = 1000; // Limit the maximum area processed in one frame
        int totalArea = (detailMaxX - detailMinX + 1) * (detailMaxZ - detailMinZ + 1);
        if (totalArea > maxArea)
        {
            Debug.LogWarning("Object bounds cover too large an area. Skipping processing to prevent freezing.");
            return;
        }

        // Get the affected region of the detail layer
        int width = detailMaxX - detailMinX + 1;
        int height = detailMaxZ - detailMinZ + 1;

        if (width > 0 && height > 0)
        {
            int[,] detailLayer = terrainData.GetDetailLayer(detailMinX, detailMinZ, width, height, 0);

            // Clear details in the region
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    detailLayer[x, z] = 0; // Clear the detail
                }
            }

            // Apply the modified details back to the terrain
            terrainData.SetDetailLayer(detailMinX, detailMinZ, 0, detailLayer);
        }
    }

    void OnApplicationQuit()
    {
        if (terrain != null && originalDetails != null)
        {
            // Restore the original details
            TerrainData terrainData = terrain.terrainData;
            terrainData.SetDetailLayer(0, 0, 0, originalDetails);
        }
    }
}
