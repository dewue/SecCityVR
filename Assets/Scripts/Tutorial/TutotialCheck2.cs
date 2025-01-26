using UnityEngine;

public class TutorialSelect2 : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.Two))
        {
            gameObject.GetComponent<TutorialTask>().CompleteTask();
        }
    }
}
