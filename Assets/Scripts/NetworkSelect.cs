using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class NetworkSelect : NetworkBehaviour
{
    [Rpc]
    public static void Rpc_MethodRenderRpc(NetworkRunner runner, string methodName)
    {
        Debug.Log($"Network Method Selected: {methodName}");

        AnalysisReader reader = GameObject.Find("Reader").GetComponent<AnalysisReader>();
        reader.RenderCallsForMethodName(methodName);
    }

    [Rpc]
    public static void Rpc_MethodRenderRpc(NetworkRunner runner)
    {
        AnalysisReader reader = GameObject.Find("Reader").GetComponent<AnalysisReader>();
        reader.RenderCallsForLastSelected();
    }
}
