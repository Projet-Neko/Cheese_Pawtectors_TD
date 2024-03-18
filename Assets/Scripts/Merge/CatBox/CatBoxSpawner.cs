using UnityEngine;
using System.Collections;
using System;

public class CatBoxSpawner : MonoBehaviour
{
    public static event Action<int> OnBoxSpawn;

    [Header("Dependencies")]
    [SerializeField] private GameObject _catBoxPrefab;
    [SerializeField] private Transform[] _slots;

    [Header("Spawn Time")]
    [SerializeField] private float minSpawnTime = 15f;
    [SerializeField] private float maxSpawnTime = 20f;

    private void Awake()
    {
        Storage.OnBoxInit += Storage_OnBoxInit;

        StartCoroutine(Spawn()); // Lance la coroutine pour générer les chats à intervalles aléatoires
    }

    private void OnDestroy()
    {
        Storage.OnBoxInit -= Storage_OnBoxInit;
    }

    private void Storage_OnBoxInit(Transform slot)
    {
        SpawnBox(slot);
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnTime, maxSpawnTime)); // Attente du délai aléatoire
            Debug.Log("spawning");

            int randomSlotIndex = UnityEngine.Random.Range(0, _slots.Length);
            Transform selectedSlot = _slots[randomSlotIndex];

            if (selectedSlot.childCount == 0) SpawnBox(selectedSlot);
        }
    }

    private void SpawnBox(Transform slot)
    {
        GameObject go = Instantiate(_catBoxPrefab, slot);
        go.transform.localScale = new Vector3(50, 50, 50);
        OnBoxSpawn?.Invoke(int.Parse(slot.name.Split('_')[1]));
    }
}