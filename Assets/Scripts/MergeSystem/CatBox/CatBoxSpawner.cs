using UnityEngine;
using System.Collections;

public class CatBoxSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _catBoxPrefab;
    //[SerializeField] private GameObject _catPrefab;
    [SerializeField] private Transform[] _slots;

    [SerializeField] private float minSpawnTime = 15f;
    [SerializeField] private float maxSpawnTime = 20f;

    private void Start()
    {
        // Lance la coroutine pour générer les chats à intervalles aléatoires
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime)); // Attente du délai aléatoire

            int randomSlotIndex = Random.Range(0, _slots.Length);
            Transform selectedSlot = _slots[randomSlotIndex];

            if (selectedSlot.childCount == 0)
            {
                GameObject go = Instantiate(_catBoxPrefab, selectedSlot);
                go.transform.localScale = new Vector3(50, 50, 50);
                //go.GetComponent<Cat>().SetStorageMode(true); // Permet de cacher le HUD
            }
        }
    }
}