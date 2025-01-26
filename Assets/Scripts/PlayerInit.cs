using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInit: MonoBehaviour
{
    [SerializeField]
    public string playerName;

    void Start()
    {
        gameObject.transform.Find("CubeHead").GetComponent<Renderer>().material.color = Random.ColorHSV();

        //gameObject.transform.Find("NameCanvas/Name").GetComponent<TextMeshProUGUI>().text = playerName;
    }
}
