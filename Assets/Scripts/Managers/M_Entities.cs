using System.Linq;
using UnityEngine;

public class M_Entities : MonoBehaviour
{
    [SerializeField] private GameObject _catPrefab, _mousePrefab;

    public CatSO[] Cats => _cats;
    public MouseSO[] Mouses => _mouses;
    public Cheese Cheese => _cheese;
    public int MouseLevel => _mouseLevel;
    public bool CanSpawnAlbino => _canSpawnAlbino;

    private CatSO[] _cats;
    private MouseSO[] _mouses;
    private Cheese _cheese;
    private int _mouseLevel = 1;
    private bool _canSpawnAlbino = true;

    private void OnDestroy()
    {
        Cheese.OnInit -= Cheese_OnInit;
        M_Wave.OnWaveReload -= M_Wave_OnWaveReload;
    }

    public void Init()
    {
        _cats = Resources.LoadAll<CatSO>("SO/Cats");
        _mouses = Resources.LoadAll<MouseSO>("SO/Mouses");

        _cats.OrderBy(x => x.name);
        _mouses.OrderBy(x => x.name);

        Cheese.OnInit += Cheese_OnInit;
        M_Wave.OnWaveReload += M_Wave_OnWaveReload;
    }

    private void Cheese_OnInit(Cheese cheese)
    {
        Debug.Log("Cheese found.");
        _cheese = cheese;
    }

    private void M_Wave_OnWaveReload()
    {
        _mouseLevel++;
        _canSpawnAlbino = true;
    }

    public void AlbinoHasSpawned()
    {
        _canSpawnAlbino = false;
    }
}