using UnityEngine;

public class TutorialSelect4 : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.Two))
        {
            gameObject.GetComponent<TutorialTask>().CompleteTask();
        }
    }
}
