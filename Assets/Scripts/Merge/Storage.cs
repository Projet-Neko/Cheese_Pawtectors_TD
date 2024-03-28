using System;
using UnityEngine;
using UnityEngine.UI;

public class Storage : MonoBehaviour
{
    public static event Action<int, int> OnStorageCheck; // slot index, cat level
    public static event Action<int, int, bool> OnCatSpawn; // slot index, cat level, is free
    public static event Action<Transform> OnBoxInit; // slot transform

    [SerializeField] private GridLayoutGroup _slots;
    [SerializeField] private GameObject _catPrefab;

    private Transform _freeSlot;

    private void Awake()
    {
        AdoptButton.OnAdoptButtonClick += AdoptButton_OnAdoptButtonClick;
        Mod_Economy.OnAdoptCheck += Mod_Economy_OnAdoptCheck;
        CatBoxOpening.OnBoxOpen += CatBoxOpening_OnBoxOpen;

        int index = 0;

        foreach (Transform slot in _slots.transform)
        {
            Data_Storage ds = GameManager.Instance.Data.Storage[index];
            if (ds.CatIndex == -2) OnBoxInit?.Invoke(slot);
            else if (ds.CatIndex != -1) slot.GetComponent<StorageSlot>().InitSlot(SpawnCat(ds.CatIndex + 1, slot, true));
            index++;
        }
    }

    private void OnDestroy()
    {
        AdoptButton.OnAdoptButtonClick -= AdoptButton_OnAdoptButtonClick;
        Mod_Economy.OnAdoptCheck -= Mod_Economy_OnAdoptCheck;
        CatBoxOpening.OnBoxOpen -= CatBoxOpening_OnBoxOpen;
    }

    private void AdoptButton_OnAdoptButtonClick(int catLevel)
    {
        int slotIndex = -1;

        // Check if storage has free slot
        foreach (Transform slot in _slots.transform)
        {
            if (slot.childCount == 0)
            {
                _freeSlot = slot;
                break;
            }
        }

        if (_freeSlot != null) slotIndex = int.Parse(_freeSlot.name.Split('_')[1]);
        OnStorageCheck?.Invoke(slotIndex, catLevel);
    }

    private void Mod_Economy_OnAdoptCheck(bool canAdopt, int catLevel)
    {
        if (!canAdopt) return;
        SpawnCat(catLevel, _freeSlot); // Spawn cat in storage
    }

    private Cat SpawnCat(int catLevel, Transform slot, bool free = false)
    {
        GameObject go = Instantiate(_catPrefab, slot);
        go.transform.localScale = new Vector3(10, 10, 10);

        Cat cat = go.GetComponent<Cat>();
        cat.Init(catLevel);
        cat.SetStorageMode(true); // Permet de cacher le HUD
        OnCatSpawn?.Invoke(int.Parse(slot.name.Split('_')[1]), catLevel, free);
        return cat;
    }

    private void CatBoxOpening_OnBoxOpen(Transform slot)
    {
        int level = UnityEngine.Random.Range(1, GameManager.Instance.Data.LastCatUnlockedIndex + 2);
        SpawnCat(level, slot, true);
    }
}