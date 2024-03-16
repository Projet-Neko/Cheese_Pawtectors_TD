using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpawnPowerUps : MonoBehaviour
{
    [SerializeField] private GameObject _PowerUpsPrefab;
    [SerializeField] private Transform _PowerUpsLayout;
    [SerializeField] private float minSpawnInterval = 60f;
    [SerializeField] private float maxSpawnInterval = 180f;
    [SerializeField] private int maxPrefabs = 4;

    void Start()
    {
        StartCoroutine(SpawnPrefabRoutine());
    }

    IEnumerator SpawnPrefabRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

            if (_PowerUpsLayout.childCount < maxPrefabs)
            {
                GameObject newPrefab = Instantiate(_PowerUpsPrefab, _PowerUpsLayout);
                LayoutRebuilder.ForceRebuildLayoutImmediate(_PowerUpsLayout.GetComponent<RectTransform>());
            }
        }
    }
}
