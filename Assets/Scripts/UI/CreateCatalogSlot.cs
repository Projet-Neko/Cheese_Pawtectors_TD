using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCatalogSlot : MonoBehaviour
{
    [SerializeField] private GameObject _slotPrefab;

    private void Awake()
    {
        //GameManager.Instance.Cats
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var cat in GameManager.Instance.Cats)
        {
            Instantiate(_slotPrefab, transform).GetComponent<CatalogSlot>().Init(cat);
        }
    }
}
