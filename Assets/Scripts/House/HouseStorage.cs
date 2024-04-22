using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseStorage : MonoBehaviour
{
    [SerializeField] private HorizontalLayoutGroup _container;
    [SerializeField] private GameObject _slotPrefab;

    private List<Transform> _slots;

    private void Start()
    {
        _slots = new();

        // Temporary inventory without tags
        foreach (var room in House.Instance.RoomsStorage)
        {
            if (room.Key.Item1 is RoomPattern.VoidRoom) continue;
            CreateSlot();
        }
    }

    public void CreateSlot()
    {
        GameObject go = Instantiate(_slotPrefab, _container.transform);
        _slots.Add(go.transform);

        // TODO -> create room as slot child
    }
}