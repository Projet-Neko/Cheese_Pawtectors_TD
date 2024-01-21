using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    #region Modules
    [SerializeField] private EntitiesMod _entitiesManager;
    [SerializeField] private PlayerMod _playerManager;

    // EntitiesMod
    public CatSO[] Cats => _entitiesManager.Cats;
    public MouseSO[] Mouses => _entitiesManager.Mouses;
    public int MouseLevel => _entitiesManager.MouseLevel;
    #endregion

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
        Debug.Log("Game Manager created.");
    }
}