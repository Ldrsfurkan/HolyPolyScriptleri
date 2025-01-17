using System.Collections.Generic;
using UnityEngine;

public class TreeChopper : MonoBehaviour
{
    public LayerMask terrainLayer; // Terrain katmanını seçmek için bir Layer Mask
    public float maxRaycastDistance = 50f;

    private Terrain terrain;
    private TerrainData terrainData;

    private void Start()
    {
        terrain = Terrain.activeTerrain;
        if (terrain != null)
        {
            terrainData = terrain.terrainData;
        }
        else
        {
            Debug.LogError("Terrain bulunamadı!");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Sol tık
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, maxRaycastDistance, terrainLayer))
            {
                Vector3 worldPosition = hit.point;
                TryChopTree(worldPosition);
            }
        }
    }

    private void TryChopTree(Vector3 worldPosition)
    {
        if (terrain == null || terrainData == null) return;

        // Dünya pozisyonunu Terrain koordinatlarına çevir
        Vector3 terrainPosition = worldPosition - terrain.transform.position;
        Vector3 normalizedPosition = new Vector3(
            terrainPosition.x / terrainData.size.x,
            terrainPosition.y / terrainData.size.y,
            terrainPosition.z / terrainData.size.z
        );

        // Ağaçların konumlarına bak ve en yakın ağacı bul
        TreeInstance[] trees = terrainData.treeInstances;
        for (int i = 0; i < trees.Length; i++)
        {
            Vector3 treeWorldPosition = Vector3.Scale(trees[i].position, terrainData.size) + terrain.transform.position;
            float distance = Vector3.Distance(treeWorldPosition, worldPosition);

            if (distance < 2f) // Kesim mesafesi
            {
                ChopTree(i);
                return;
            }
        }
    }

    private void ChopTree(int treeIndex)
    {
        TreeInstance[] trees = terrainData.treeInstances;

        // Yeni ağaç listesi oluştur
        List<TreeInstance> updatedTrees = new List<TreeInstance>(trees);

        // Belirtilen ağacı kaldır
        updatedTrees.RemoveAt(treeIndex);

        // Güncellenmiş listeyi Terrain'e geri aktar
        terrainData.treeInstances = updatedTrees.ToArray();

        // Envantere odun ekle
        InventorySystem.Instance.AddToInventory("Wood");
    }
}
