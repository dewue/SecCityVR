using Fusion;
using UnityEngine;

public class HideOwnNetworkObject : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
            gameObject.SetActive(!HasStateAuthority);
    }
}
