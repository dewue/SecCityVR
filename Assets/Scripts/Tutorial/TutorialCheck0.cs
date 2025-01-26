using UnityEngine;

public class TutorialCheck0 : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.Start))
        {
            gameObject.GetComponent<TutorialTask>().CompleteTask();
        }
    }
}
