using System.Collections.Generic;
using UnityEngine;

public class SkeletonSpawner : MonoBehaviour
{
    public static SkeletonSpawner Instance { get; set;}
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    [Header("Spawner Settings")]
    public Terrain terrain; // Terrain referansı
    public GameObject skeletonPrefab; // Instantiate edilecek prefab
    public GameObject skeletonsParent; // Skeletonların parent objesi
    public int maxSkeletons = 100; // Maksimum skeleton sayısı

    private List<GameObject> spawnedSkeletons = new List<GameObject>(); // Spawn edilen skeletonları tutar

    // Skeletonları rastgele yarat
    public void SpawnSkeletons()
    {
        if (terrain == null || skeletonPrefab == null || skeletonsParent == null)
        {
            Debug.LogError("Spawner için gerekli referanslar eksik!");
            return;
        }

        int skeletonCount = Random.Range(1, maxSkeletons + 1);

        for (int i = 0; i < skeletonCount; i++)
        {
            // Terrain üzerinde rastgele bir pozisyon oluştur
            float randomX = Random.Range(terrain.transform.position.x, terrain.transform.position.x + terrain.terrainData.size.x);
            float randomZ = Random.Range(terrain.transform.position.z, terrain.transform.position.z + terrain.terrainData.size.z);
            float yPos = terrain.SampleHeight(new Vector3(randomX, 0, randomZ)) + terrain.transform.position.y;

            Vector3 spawnPosition = new Vector3(randomX, yPos, randomZ);

            // Skeleton prefabını instantiate et ve parent olarak skeletonsParent'ı ayarla
            GameObject skeleton = Instantiate(skeletonPrefab, spawnPosition, Quaternion.identity, skeletonsParent.transform);
            spawnedSkeletons.Add(skeleton);
        }

    }

    // Tüm skeletonları kaldır
    public void RemoveAllSkeletons()
    {
        foreach (GameObject skeleton in spawnedSkeletons)
        {
            if (skeleton != null)
            {
                Destroy(skeleton);
            }
        }
        spawnedSkeletons.Clear();
    }
}
