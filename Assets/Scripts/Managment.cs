using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SelectionState
{
    UnitsSelected,
    Frame,
    Other
}
public class Managment : MonoBehaviour
{
    public Camera Camera;
    public SelectebleObject Hovered;
    public List<SelectebleObject> ListOfSelected = new List<SelectebleObject>();

    public Image FrameImage;
    Vector2 _frameStart;
    Vector2 _frameEnd;
    public SelectionState currentSelectionState;
    private bool isFrameStarted = false;
    void Update()
    {
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponent<SelectebleCollider>())
            {
                SelectebleObject hitSelecteble = hit.collider.GetComponent<SelectebleCollider>().selectebleObject;
                if(Hovered)
                {
                    if(Hovered != hitSelecteble)
                    {
                        Hovered.OnUnHover();
                        Hovered = hitSelecteble;
                        Hovered.OnHover();
                    }
                }
                else
                {
                    Hovered = hitSelecteble;
                    Hovered.OnHover();
                }
            }
            else
            {
                UnhoverCurrent();
            }
        }
        else
        {
            UnhoverCurrent();
        }
        if(Input.GetMouseButtonUp(0))
        {
            if (Hovered)
            {
                if(!Input.GetKey(KeyCode.LeftControl))
                {
                    UnselectAll();
                }
                currentSelectionState = SelectionState.UnitsSelected;
                Select(Hovered);
            }
            if (currentSelectionState == SelectionState.UnitsSelected)
            {
                if (Input.GetMouseButtonUp(0))
                {
                   
                    if (hit.collider.tag == "Ground")
                    {
                        int rowNumber = Mathf.CeilToInt(Mathf.Sqrt(ListOfSelected.Count));
                        for (int i = 0; i < ListOfSelected.Count; i++)
                        {
                            int row = i / rowNumber;
                            int column = i % rowNumber;
                            Vector3 point = hit.point + new Vector3(column, 0f, row);
                            ListOfSelected[i].WhenClickOnGround(point);
                        }
                    }
                }
            }
           
        }
        if (Input.GetMouseButtonUp(1))
        {
            UnselectAll();
        }
        //Frame selection
        if (Input.GetMouseButtonDown(0))
        {
            _frameStart = Input.mousePosition;
            isFrameStarted = true;
        }
        if (Input.GetMouseButton(0) && isFrameStarted)
        {
            _frameEnd = Input.mousePosition;

            Vector2 min = Vector2.Min(_frameStart, _frameEnd);
            Vector2 max = Vector2.Max(_frameStart, _frameEnd);

            Vector2 sise = max - min;
            if (sise.magnitude > 10)
            {
                
                FrameImage.enabled = true;
                FrameImage.rectTransform.anchoredPosition = min;
                FrameImage.rectTransform.sizeDelta = sise;


                UnselectAll();
                Rect rect = new Rect(min, sise);
                Unit[] allUnits = FindObjectsOfType<Unit>();
                for (int i = 0; i < allUnits.Length; i++)
                {
                    Vector2 screenPoints = Camera.WorldToScreenPoint(allUnits[i].transform.position);
                    if (rect.Contains(screenPoints))
                    {
                        Select(allUnits[i]);
                    }
                }
                currentSelectionState = SelectionState.Frame;
            }
        }
        if (Input.GetMouseButtonUp(0)){
            FrameImage.enabled = false;
            isFrameStarted = false;
            if(ListOfSelected.Count > 0)
            {
                currentSelectionState = SelectionState.UnitsSelected;
            }
            else
            {
                currentSelectionState = SelectionState.Other;
            }
        }
    }
    private void OnDisable()
    {
        if(FrameImage)
        {
            FrameImage.enabled = false;
        }
        isFrameStarted = false;
    }
    void UnhoverCurrent()
    {
        if (Hovered)
        {
            Hovered.OnUnHover();
            Hovered = null;
        }
    }
    public void Unselect(SelectebleObject selectebleObject)
    {
        if (ListOfSelected.Contains(selectebleObject))
        {
            ListOfSelected.Remove(selectebleObject);
        }
    }
    void UnselectAll()
    {
        for (int i = 0; i < ListOfSelected.Count; i++)
        {
            ListOfSelected[i].UnSelect();
        }
        ListOfSelected.Clear();
        currentSelectionState = SelectionState.Other;
    }
    private void Select(SelectebleObject selectebleObject)
    {
        if (!ListOfSelected.Contains(selectebleObject))
        {
            ListOfSelected.Add(selectebleObject);
            selectebleObject.Select();
        }
    }
}
