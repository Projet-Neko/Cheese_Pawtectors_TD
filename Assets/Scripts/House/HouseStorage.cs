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
        /*foreach (var set in Enum.GetValues(typeof(RoomSet)))
        {
            foreach (var design in Enum.GetValues(typeof(RoomDesign)))
            {
                GameObject go = Instantiate(_slotPrefab, _container.transform);
                _slots.Add(go.transform);
            }
        }*/

        // Temporary inventory without tags
        /*for (int i = 0; i < House.Instance.MaxRooms; i++)
        {
            for (int j = 0; j < House.Instance.MaxRooms; j++)
            {

                Room room = House.Instance.RoomsGrid[i, j];
                if (room == null || room is VoidRoom) continue;
                Debug.Log(room.name);
                GameObject go = Instantiate(_slotPrefab, _container.transform);
                _slots.Add(go.transform);
            }

        }*/
    }
}