using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class CatBoxSpawner : MonoBehaviour
{
    public static event Action<int> OnBoxSpawn;

    [Header("Dependencies")]
    [SerializeField] private GameObject _catBoxPrefab;
    [SerializeField] private Transform[] _slots;

    [Header("Spawn Time")]
    [SerializeField] private float minSpawnTime = 15f;
    [SerializeField] private float maxSpawnTime = 20f;

    private List<int> _availableSlots;

    private void Awake()
    {
        Storage.OnBoxInit += SpawnBox;

        StartCoroutine(Spawn()); // Lance la coroutine pour générer les chats à intervalles aléatoires
    }

    private void OnDestroy()
    {
        Storage.OnBoxInit -= SpawnBox;
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnTime, maxSpawnTime)); // Attente du délai aléatoire
            if (!CanSpawn()) continue;

            int randomSlotIndex = UnityEngine.Random.Range(0, _slots.Length);
            
            while (!_availableSlots.Contains(randomSlotIndex))
            {
                randomSlotIndex = UnityEngine.Random.Range(0, _slots.Length);
            }

            Transform selectedSlot = _slots[randomSlotIndex];
            if (selectedSlot.childCount == 0) SpawnBox(selectedSlot);
        }
    }

    private bool CanSpawn()
    {
        _availableSlots = new();

        foreach (var slot in _slots)
        {
            if (slot.childCount == 0) _availableSlots.Add(int.Parse(slot.name.Split('_')[1]));
        }

        return _availableSlots.Count > 2;
    }

    private void SpawnBox(Transform slot)
    {
        GameObject go = Instantiate(_catBoxPrefab, slot);
        go.transform.localScale = new Vector3(50, 50, 50);
        OnBoxSpawn?.Invoke(int.Parse(slot.name.Split('_')[1]));
    }
}