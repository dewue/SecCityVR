using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasToggLeft : MonoBehaviour
{
    public void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            Canvas go = gameObject.GetComponent<Canvas>();
            go.enabled = !go.enabled;
        }
    }
}
