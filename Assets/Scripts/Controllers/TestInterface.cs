using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TestInterface : MonoBehaviour, IDragAndDropHandler
{
    public DragAndDropVisualMode dragAndDropVisualMode => throw new System.NotImplementedException();

    public bool AcceptsDragAndDrop()
    {
        throw new System.NotImplementedException();
    }

    public void DrawDragAndDropPreview()
    {
        throw new System.NotImplementedException();
    }

    public void ExitDragAndDrop()
    {
        throw new System.NotImplementedException();
    }

    public void PerformDragAndDrop()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateDragAndDrop()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
