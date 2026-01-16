using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    private ARTrackedImageManager trackedImages;

    public GameObject[] ArPrefabs;

    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();

    private void Awake()
    {
        trackedImages = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        trackedImages.trackedImagesChanged += OnImageChanged;
        Debug.Log("ImageTracker ENABLED");
    }

    private void OnDisable()
    {
        trackedImages.trackedImagesChanged -= OnImageChanged;
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        // Trata added e updated da mesma forma para cria��o
        foreach (var trackedImage in args.added)
        {
            TryCreatePrefab(trackedImage);
        }

        foreach (var trackedImage in args.updated)
        {
            TryCreatePrefab(trackedImage);

            if (spawnedObjects.TryGetValue(trackedImage.referenceImage.name, out GameObject go))
            {
                go.SetActive(trackedImage.trackingState == TrackingState.Tracking);
            }
        }

        foreach (var trackedImage in args.removed)
        {
            if (spawnedObjects.TryGetValue(trackedImage.referenceImage.name, out GameObject go))
            {
                go.SetActive(false);
            }
        }
    }

    private void TryCreatePrefab(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        if (spawnedObjects.ContainsKey(imageName))
            return;

        foreach (var prefab in ArPrefabs)
        {
            if (prefab.name == imageName)
            {
                GameObject go = Instantiate(prefab);
                go.name = prefab.name;

                // Parenting correto
                go.transform.SetParent(trackedImage.transform);

                // Reset transform (CR�TICO)
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;

                // Escala baseada no tamanho f�sico da imagem
                float scale = trackedImage.size.x;
                go.transform.localScale = Vector3.one * scale;

                spawnedObjects.Add(imageName, go);

                Debug.Log($"Criado {go.name} | scale: {go.transform.localScale} | pos: {go.transform.localPosition}");

            
                break;
            }
        }

    }

}
