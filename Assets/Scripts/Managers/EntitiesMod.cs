using System.Linq;
using UnityEngine;

public class EntitiesMod : MonoBehaviour
{
    [SerializeField] private GameObject _catPrefab, _mousePrefab;

    public CatSO[] Cats => _cats;
    public MouseSO[] Mouses => _mouses;
    public int MouseLevel => _mouseLevel;

    private CatSO[] _cats;
    private MouseSO[] _mouses;
    private int _mouseLevel = 1;

    private void Start()
    {
        _cats = Resources.LoadAll<CatSO>("SO/Cats");
        _mouses = Resources.LoadAll<MouseSO>("SO/Mouses");

        _cats.OrderBy(x => x.name);
        _mouses.OrderBy(x => x.name);

        // Testing only
        Cat c = Instantiate(_catPrefab).GetComponent<Cat>();
        c.Init(1);

        c = Instantiate(_catPrefab).GetComponent<Cat>();
        c.Init(2);
    }
}