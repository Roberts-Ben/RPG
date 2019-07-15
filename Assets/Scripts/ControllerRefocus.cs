using UnityEngine;
using UnityEngine.EventSystems;

public class ControllerRefocus : MonoBehaviour
{
    private GameObject lastselect;

    void Start()
    {
        lastselect = new GameObject();
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastselect);
        }
        else
        {
            lastselect = EventSystem.current.currentSelectedGameObject;
        }
    }

}