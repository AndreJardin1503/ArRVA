using UnityEngine;

public class BucketSetup : MonoBehaviour
{
    [Header("Configurações do Balde")]
    public bool autoSetup = true;

    void Start()
    {
        if (autoSetup)
        {
            SetupBucket();
        }
    }

    public void SetupBucket()
    {
        // Garante que o balde tem Rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Configura o Rigidbody do balde para ser kinematic
        // Isto faz com que o balde não seja afetado pela física mas as bolas sim
        rb.isKinematic = true;
        rb.useGravity = false;

        // Verifica se tem colliders
        Collider[] colliders = GetComponentsInChildren<Collider>();
        if (colliders.Length == 0)
        {
            Debug.LogWarning("O balde não tem colliders! Adiciona colliders manualmente para as bolas colidirem.");
        }
        else
        {
            Debug.Log($"Balde configurado com {colliders.Length} collider(s)");
        }
    }
}