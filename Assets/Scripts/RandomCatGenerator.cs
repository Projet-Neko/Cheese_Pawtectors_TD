using UnityEngine;
using System.Collections;

public class RandomCatGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _catPrefab; // Prefab du chat � g�n�rer
    [SerializeField] private Transform[] _slots; // Tableau des emplacements des slots

    private float minSpawnTime = 15f; // Temps minimum entre deux apparitions de chats
    private float maxSpawnTime = 20f; // Temps maximum entre deux apparitions de chats

    private void Start()
    {
        // Lance la coroutine pour g�n�rer les chats � intervalles al�atoires
        StartCoroutine(SpawnRandomCat());
    }

    private IEnumerator SpawnRandomCat()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime)); // Attente du d�lai al�atoire

            int randomSlotIndex = Random.Range(0, _slots.Length); // S�lection d'un slot al�atoire
            Transform selectedSlot = _slots[randomSlotIndex];

            if (selectedSlot.childCount == 0) // V�rification si le slot est vide
            {
                // Instanciation du prefab du chat dans le slot s�lectionn�
                Instantiate(_catPrefab, selectedSlot);
            }
        }
    }
}
