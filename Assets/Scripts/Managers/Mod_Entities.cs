using System.Linq;
using UnityEngine;

public class Mod_Entities : Module
{
    [SerializeField] private GameObject _catPrefab, _mousePrefab;

    public CatSO[] Cats => _cats;
    public MouseSO[] Mouses => _mouses;
    public Cheese Cheese => _cheese;
    public bool CanSpawnAlbino => _canSpawnAlbino;

    private CatSO[] _cats;
    private MouseSO[] _mouses;
    private Cheese _cheese;
    private bool _canSpawnAlbino = true;

    private void Awake()
    {
        Cheese.OnInit += Cheese_OnInit;
        Mod_Waves.OnWaveReload += M_Wave_OnWaveReload;
    }

    private void OnDestroy()
    {
        Cheese.OnInit -= Cheese_OnInit;
        Mod_Waves.OnWaveReload -= M_Wave_OnWaveReload;
    }

    public override void Init(GameManager gm)
    {
        base.Init(gm);
        _cats = Resources.LoadAll<CatSO>("SO/Cats");
        _mouses = Resources.LoadAll<MouseSO>("SO/Mouses");

        _cats.OrderBy(x => x.name);
        _mouses.OrderBy(x => x.name);

        InitComplete();
    }

    private void Cheese_OnInit(Cheese cheese)
    {
        _cheese = cheese;
    }

    private void M_Wave_OnWaveReload()
    {
        _canSpawnAlbino = true;
    }

    public void AlbinoHasSpawned()
    {
        _canSpawnAlbino = false;
    }
}