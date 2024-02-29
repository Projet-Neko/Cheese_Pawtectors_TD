using UnityEngine;
using System.Collections;

public class RandomCatGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _catPrefab;
    [SerializeField] private Transform[] _slots;

    private float minSpawnTime = 15f;
    private float maxSpawnTime = 20f;

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

            int randomSlotIndex = Random.Range(0, _slots.Length);
            Transform selectedSlot = _slots[randomSlotIndex];

            if (selectedSlot.childCount == 0)
            {
                GameObject go = Instantiate(_catPrefab, selectedSlot);
                go.transform.localScale = new Vector3(100, 100, 100);
                go.GetComponent<Cat>().SetStorageMode(true); // Permet de cacher le HUD
            }
        }
    }
}