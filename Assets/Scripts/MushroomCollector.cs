using UnityEngine;

public class MushroomCollector : MonoBehaviour
{
    public int mushroomCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mushroom"))
        {
            mushroomCount++;
            Destroy(other.gameObject);
            Debug.Log("Подобрано грибов: " + mushroomCount);
        }
    }
}
