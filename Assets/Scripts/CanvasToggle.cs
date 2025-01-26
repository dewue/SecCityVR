using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CanvasToggle : MonoBehaviour
{
    public void Update()
    {
        Canvas go = gameObject.GetComponent<Canvas>();
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            go.enabled = !go.enabled;
        }

        // A Button press to render calls
        if (OVRInput.GetDown(OVRInput.Button.One) && go.enabled)
        {
            AnalysisReader reader = GameObject.Find("Reader").GetComponent<AnalysisReader>();
            if (NetworkManager.Instance.Runner.IsConnectedToServer)
            {
                NetworkSelect.Rpc_MethodRenderRpc(NetworkManager.Instance.Runner, reader.currentlySeclected.FullName);
            }
            else
            {
                reader.RenderCallsForLastSelected();
            }
        }

    }
}
