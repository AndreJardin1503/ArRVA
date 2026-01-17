using UnityEngine;
using UnityEngine.InputSystem;

public class BallSpawner : MonoBehaviour
{
    public GameObject[] ballPrefabs;
    public Transform bucketTransform;
    public float spawnHeight = 0.05f;
    public float spawnRadius = 0.01f;
    public float ballScale = 0.005f;

    private bool spawned = false;
    private int frameCount = 0;

    void Start()
    {
        Debug.Log("===== BALLSPAWNER START =====");
        Debug.Log("Prefabs: " + (ballPrefabs != null ? ballPrefabs.Length : 0));
        Debug.Log("Balde: " + (bucketTransform != null ? "OK" : "NULL"));
    }

    void Update()
    {
        frameCount++;

        // Log a cada 60 frames para confirmar que está a executar
        if (frameCount % 60 == 0)
        {
            Debug.Log("BallSpawner ATIVO - Frame: " + frameCount);
        }

        // Detecta clique/toque usando Pointer (funciona em ambos os sistemas)
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame && !spawned)
        {
            Debug.Log("POINTER PRESS DETECTADO!");
            SpawnBalls();
        }
    }

    void SpawnBalls()
    {
        Debug.Log("===== SPAWNING BOLAS =====");

        if (bucketTransform == null)
        {
            Debug.LogError("Balde NULL!");
            return;
        }

        if (ballPrefabs == null || ballPrefabs.Length == 0)
        {
            Debug.LogError("Prefabs NULL!");
            return;
        }

        Vector3 pos = bucketTransform.position + Vector3.up * spawnHeight;
        Debug.Log("Spawn em: " + pos);
        Debug.Log("Balde está em: " + bucketTransform.position);

        int count = 0;

        for (int i = 0; i < ballPrefabs.Length; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                Vector3 offset = new Vector3(
                    Random.Range(-spawnRadius, spawnRadius), 0,
                    Random.Range(-spawnRadius, spawnRadius)
                );

                GameObject ball = Instantiate(ballPrefabs[i], pos + offset, Quaternion.identity);
                ball.transform.localScale = Vector3.one * ballScale;

                Rigidbody rb = ball.GetComponent<Rigidbody>();
                if (!rb) rb = ball.AddComponent<Rigidbody>();
                rb.mass = 0.001f;
                rb.useGravity = true;

                if (!ball.GetComponent<Collider>())
                    ball.AddComponent<SphereCollider>();

                count++;
            }
        }

        spawned = true;
        Debug.Log($"===== {count} BOLAS CRIADAS =====");
    }
}