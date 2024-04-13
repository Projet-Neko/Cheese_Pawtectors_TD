using UnityEngine;

public class DropCat : DragAndDropHandler
{
    [SerializeField] private Cat _currentCat;
    private int _previousSlotIndex;

    public override void HandleDragAndDrop(Cat cat, Vector3 initialPosition)
    {
        if (transform.childCount == 0) _currentCat = null;
        //_previousSlotIndex = int.Parse(cat.transform.parent.name.Split('_')[1]);
        // TODO -> vérifier si le chat faisait partie d'une room plutôt d'un slot

        _currentCat = cat;
        _currentCat.transform.SetParent(transform);
        _currentCat.transform.position = transform.position;
        //cat.transform.position = Camera.main.ScreenToWorldPoint(cat.transform.position);
    }
}