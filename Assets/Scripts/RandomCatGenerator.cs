using UnityEngine;
using System.Collections;

public class RandomCatGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _catPrefab; // Prefab du chat à générer
    [SerializeField] private Transform[] _slots; // Tableau des emplacements des slots

    private float minSpawnTime = 15f; // Temps minimum entre deux apparitions de chats
    private float maxSpawnTime = 20f; // Temps maximum entre deux apparitions de chats

    private void Start()
    {
        // Lance la coroutine pour générer les chats à intervalles aléatoires
        StartCoroutine(SpawnRandomCat());
    }

    private IEnumerator SpawnRandomCat()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime)); // Attente du délai aléatoire

            int randomSlotIndex = Random.Range(0, _slots.Length); // Sélection d'un slot aléatoire
            Transform selectedSlot = _slots[randomSlotIndex];

            if (selectedSlot.childCount == 0) // Vérification si le slot est vide
            {
                // Instanciation du prefab du chat dans le slot sélectionné
                Instantiate(_catPrefab, selectedSlot);
            }
        }
    }
}
