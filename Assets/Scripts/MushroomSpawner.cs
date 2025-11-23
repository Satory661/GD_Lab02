using UnityEngine;

public class MushroomSpawner : MonoBehaviour
{
    public GameObject mushroomPrefab; 
    public Transform player;           
    public float spawnRadius = 5f;     
    public float pickupDistance = 1.5f; 

    private GameObject currentMushroom;

    void Start()
    {
        SpawnNewMushroom();
    }

    void SpawnNewMushroom()
    {
        if (mushroomPrefab == null || player == null)
        {
            Debug.LogWarning("Не назначен игрок или префаб гриба!");
            return;
        }

        // Случайная точка вокруг игрока
        Vector3 spawnPos = player.position + Random.insideUnitSphere * spawnRadius;
        spawnPos.y = player.position.y;

        currentMushroom = Instantiate(mushroomPrefab, spawnPos, Quaternion.identity);
        currentMushroom.transform.localScale = Vector3.one * 1.5f;

        
        Collider col = currentMushroom.GetComponent<Collider>();
        if (col == null)
            col = currentMushroom.AddComponent<SphereCollider>();
        col.isTrigger = true;
    }

    void Update()
    {
        if (currentMushroom == null) return;

        // Проверка расстояния
        float dist = Vector3.Distance(player.position, currentMushroom.transform.position);
        if (dist < pickupDistance)
        {
            Debug.Log("🍄 Гриб подобран!");
            Destroy(currentMushroom);
            currentMushroom = null; 
            SpawnNewMushroom();
        }
    }
}
