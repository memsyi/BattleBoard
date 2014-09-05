using UnityEngine;
using System.Collections;

public class MouseSelectionBehavior : MonoBehaviour
{
    [SerializeField]
    GUISkin _mouseDragSkin = null;

    private Rect _mouseSelectionArea;
    private Vector2 _mouseStartSelection;

    public bool IsDragging { get; private set; }

    // Update is called once per frame
    void Update()
    {
        HandleMouseSelection();
    }

    void OnGUI()
    {
        if(!IsDragging)
        {
            return;
        }

        GUI.Box(_mouseSelectionArea, "", _mouseDragSkin.box);
    }

    private void HandleMouseSelection()
    {
        if(Input.GetMouseButtonDown(0))
        {
            MouseEnter();
        }
        else if(IsDragging)
        {
            MouseDrag();
        }

        if(Input.GetMouseButtonUp(0))
        {
            MouseExit();
        }
    }

    private void MouseEnter()
    {
        IsDragging = true;

        _mouseStartSelection.x = Input.mousePosition.x;
        _mouseStartSelection.y = Screen.height - Input.mousePosition.y;

        _mouseSelectionArea = new Rect(_mouseStartSelection.x, _mouseStartSelection.y, 0, 0);
    }

    private void MouseDrag()
    {
        Vector2 currentMousePosition;
        currentMousePosition.x = Input.mousePosition.x;
        currentMousePosition.y = Screen.height - Input.mousePosition.y;

        _mouseSelectionArea.width = Mathf.Abs(currentMousePosition.x - _mouseStartSelection.x);

        if (currentMousePosition.x < _mouseStartSelection.x)
        {
            _mouseSelectionArea.x = currentMousePosition.x;
        }
        else
        {
            _mouseSelectionArea.x = _mouseStartSelection.x;
        }

        _mouseSelectionArea.height = Mathf.Abs(currentMousePosition.y - _mouseStartSelection.y);
        
        if (currentMousePosition.y < _mouseStartSelection.y)
        {
            _mouseSelectionArea.y = currentMousePosition.y;
        }
        else
        {
            _mouseSelectionArea.y = _mouseStartSelection.y;
        }
    }

    private void MouseExit()
    {
        IsDragging = false;
    }
}
