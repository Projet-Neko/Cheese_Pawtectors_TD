using UnityEngine;

public class BoxInteraction : MonoBehaviour
{
    [SerializeField] private GameObject catPrefab;

    private void OnMouseDown()
    {
        Destroy(gameObject);

        SpawnCat();
    }

    private void SpawnCat()
    {
        Transform slot = transform.parent;

        // Instancier le prefab du chat dans le même emplacement que la boîte
        GameObject go = Instantiate(catPrefab, slot.position, Quaternion.identity, slot);
        go.transform.localScale = new Vector3(10, 10, 10); // Modifie la scale du chat
        go.GetComponent<Cat>().SetStorageMode(true); // Permet de cacher le HUD
    }
}
