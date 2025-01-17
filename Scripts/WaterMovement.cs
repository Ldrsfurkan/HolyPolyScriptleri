using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovement : MonoBehaviour
{
    // Dalga hızı - Dalgaların ne kadar hızlı hareket edeceğini belirler
    private float waveSpeed = 1f;

    // Dalga yüksekliği - Dalgaların yukarı ve aşağı ne kadar hareket edeceğini belirler
    private float waveHeight = 0.02f;

    // Dalga frekansı - Dalgaların birbirine ne kadar yakın olduğunu belirler
    private float waveFrequency = 0.01f;

    // Texture kaydırma hızı (daha yavaş olacak şekilde ayarlandı)
    public Vector2 textureWaveSpeed = new Vector2(0.002f, 0.006f);

    // Material referansı
    private Material waterMaterial;

    // Texture offset'leri tutmak için vektör
    private Vector2 textureOffset = Vector2.zero;

    private Vector3[] baseVertices; // Orijinal vertex pozisyonlarını saklamak için.

    void Start()
    {
        // Düzlemin mesh'ini al.
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        // Orijinal vertex pozisyonlarını sakla.
        baseVertices = mesh.vertices;

        // Renderer bileşeninden su materyalini al.
        waterMaterial = GetComponent<Renderer>().material;
    }

    void Update()
    {
        // Mesh dalgalanması
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = new Vector3[baseVertices.Length];

        // Zaman faktörünü kullanarak her vertex'in yüksekliğini ve pozisyonunu değiştir.
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = baseVertices[i];

            // X ve Z pozisyonlarını kullanarak sinüs dalgasını uygula.
            float wave = Mathf.Sin((vertex.x + Time.time * waveSpeed) * waveFrequency)
                       + Mathf.Sin((vertex.z + Time.time * waveSpeed) * waveFrequency);

            // Dalganın yüksekliğini ayarla.
            vertex.y = wave * waveHeight;

            vertices[i] = vertex;
        }

        // Yeni vertex pozisyonlarını güncelle.
        mesh.vertices = vertices;
        mesh.RecalculateNormals(); // Normal'ları tekrar hesaplayarak ışıklandırmayı doğru hale getir.

        // Texture kaydırması (daha yavaş ilerlemesi için ayarlandı)
        textureOffset += textureWaveSpeed * Time.deltaTime;

        // Malzemenin mainTexture'ine offset uygula.
        waterMaterial.mainTextureOffset = textureOffset;
    }
}
