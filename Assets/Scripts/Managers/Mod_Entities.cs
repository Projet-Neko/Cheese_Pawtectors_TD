using System.Linq;
using UnityEngine;

public class Mod_Entities : Module
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _catPrefab;
    [SerializeField] private GameObject _mousePrefab;
    [SerializeField] private GameObject _blackMousePrefab;
    [SerializeField] private GameObject _mouseRatPrefab;
    [SerializeField] private GameObject _mouseBallPrefab;
    [SerializeField] private GameObject _cheesePrefab;

    public GameObject MousePrefab => _mousePrefab;
    public GameObject BlackMousePrefab => _blackMousePrefab;
    public GameObject MouseRatPrefab => _mouseRatPrefab;
    public GameObject MouseBallPrefab => _mouseBallPrefab;
    public GameObject CheesePrefab => _cheesePrefab;
    public CatSO[] Cats => _cats;
    public MouseSO[] Mouses => _mouses;
    public Cheese Cheese => _cheese;
    public bool CanSpawnAlbino => _canSpawnAlbino;
    public bool CanSpawnBlackMouse => _canSpawnBlackMouse;

    private CatSO[] _cats;
    private MouseSO[] _mouses;
    private Cheese _cheese;
    private bool _canSpawnAlbino = true;
    private bool _canSpawnBlackMouse = false;

    private void Awake()
    {
        //Cat.OnUnlock += Cat_OnUnlock;
        Mod_Waves.OnWaveReload += M_Wave_OnWaveReload;
    }

    private void OnDestroy()
    {
        //Cat.OnUnlock -= Cat_OnUnlock;
        Mod_Waves.OnWaveReload -= M_Wave_OnWaveReload;
    }

    public override void Init(GameManager gm)
    {
        base.Init(gm);
        _cats = Resources.LoadAll<CatSO>("SO/Cats");
        _mouses = Resources.LoadAll<MouseSO>("SO/Mouses");

        _cats.OrderBy(x => x.name);
        _mouses.OrderBy(x => x.name);

        Debug.Log($"Loaded {_cats.Length} cats SO !");

        InitComplete();
    }

    //private void Cat_OnUnlock(int catIndex)
    //{
    //    Debug.Log($"<color=lime>{_cats[catIndex].Name} is unlocked !</color>");
    //    _cats[catIndex].State = CatState.Unlock;
    //}

    private void M_Wave_OnWaveReload()
    {
        _canSpawnAlbino = true;
        if(_gm.Data.WaveNumber > 20)
        {
            _canSpawnBlackMouse = true;
        }
    }

    public void AlbinoHasSpawned()
    {
        _canSpawnAlbino = false;
    }

    public void BlackMouseHasSpawned()
    {
        _canSpawnBlackMouse = false;
    }

    public void SpawnCheese(Transform room)
    {
        GameObject cheese = Instantiate(CheesePrefab, room.position, Quaternion.identity, room);
        cheese.transform.localEulerAngles = new(0, 0, 0);
        _cheese = cheese.GetComponent<Cheese>();
    }
}