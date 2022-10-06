using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicControllerVisuals : MonoBehaviour
{
    [SerializeField]
    VisualsItem[] _variants;

    void Start()
    {
        ControlsManager controlManager = FindObjectOfType<ControlsManager>();

        if (controlManager != null)
        {
            ControllerChanged(controlManager.CurrentControllerType);
        }
    }

    public void ControllerChanged(CurrentControllerType controlType)
    {
        foreach (VisualsItem itm in _variants)
        {
            if (itm.obj == null)
                continue;

            itm.obj.gameObject.SetActive(controlType == itm.controlType);
        }
    }

    [System.Serializable]
    public class VisualsItem
    {
        public Transform obj;
        public CurrentControllerType controlType;
    }
}
