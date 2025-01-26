using System;
using System.Collections;
using UnityEngine;


public enum FishType
{
    [Prefab("Fish/Swordfish")]
    Swordfish,

    [Prefab("Fish/Sawfish")]
    Sawfish,

    [Prefab("Fish/Hammerfish")]
    Hammerfish,

    [Prefab("Fish/Dartfish")]
    Dartfish,

    [Prefab("Fish/Blowfish")]
    Blowfish,

    [Prefab("Fish/SeaUrchin")]
    SeaUrchin
}

public enum FishHorizontalDirection
{
    Left,
    Right
}



public class FishSpawnPoint : MonoBehaviour
{
    public FishType FishType;
    public FishHorizontalDirection HorizontalDirection;

    public GameObject spawnedFish;

    public ParticleSystem SpawnEffectSystem;

    public float MaxHorizontalPosition = 90;
    public float FadeInDuration = 1.5f;

    void Start()
    {
        SpawnFish();
        SpawnEffectSystem = GetComponentInChildren<ParticleSystem>();
    }

    /// <summary>
    /// Spawn a fish by instantiating a prefab and setting its position
    /// </summary>
    public void SpawnFish()
    {
        GameObject fishPrefab = GetFishPrefab();
        if (fishPrefab == null)
        {
            Debug.LogError("Fish prefab not found");
            return;
        }

        spawnedFish = Instantiate(fishPrefab, transform.position, Quaternion.identity);
        // if position is negative, flip the fish to face left
        HorizontalDirection = transform.position.x < 0 ? FishHorizontalDirection.Right : FishHorizontalDirection.Left;
        if (HorizontalDirection == FishHorizontalDirection.Left)
        {
            spawnedFish.transform.localScale = new Vector3(-1, 1, 1);
            spawnedFish.GetComponentInChildren<Fish>().FlipToLeft();
        }
        spawnedFish.transform.SetParent(transform);
        SpawnEffectSystem.Play();
    }

    public GameObject GetFishPrefab()
    {
        FishType fishType = FishType;
        System.Type type = fishType.GetType();

        // get the prefab attribute for the fish type
        PrefabAttribute prefabAttribute = (PrefabAttribute)Attribute.GetCustomAttribute(type.GetField(fishType.ToString()), typeof(PrefabAttribute));
        if (prefabAttribute == null)
        {
            Debug.LogError("Prefab attribute not found for fish type: " + fishType);
            return null;
        }

        string prefabPath = prefabAttribute.PrefabPath;
        GameObject fishPrefab = Resources.Load<GameObject>(prefabPath);
        return fishPrefab;
    }

    void Update()
    {
        if (spawnedFish == null)
        {
            return;
        }

        if (spawnedFish.transform.position.x < -MaxHorizontalPosition || spawnedFish.transform.position.x > MaxHorizontalPosition)
        {
            if (spawnedFish.activeSelf == false)
            {
                return;
            }
            // deactivate the fish if it goes out of bounds
            spawnedFish.SetActive(false);
            // move the fish back to the spawn point
            spawnedFish.transform.position = transform.position;

            // play a spawn particle effect
            SpawnEffect();

        }
    }

    void SpawnEffect()
    {
        // play a spawn particle effect
        if (SpawnEffectSystem != null)
        {
            SpawnEffectSystem.Play();
        

            // wait for the duration of the particle effect
            StartCoroutine(DeactivateFish(1f));
        } else {
            Debug.LogError("Spawn effect not found");
        }

    }

    IEnumerator DeactivateFish(float duration)
    {
        yield return new WaitForSeconds(duration);
        spawnedFish.SetActive(true);
    }


    

}


// Use a custom attribute to define the prefab path for each fish type
public class PrefabAttribute : System.Attribute
{
    public string PrefabPath;

    public PrefabAttribute(string prefabPath)
    {
        PrefabPath = prefabPath;
    }
}