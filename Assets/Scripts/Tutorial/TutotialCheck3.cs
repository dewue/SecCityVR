using UnityEngine;

public class TutorialSelect3 : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.One))
        {
            gameObject.GetComponent<TutorialTask>().CompleteTask();
        }
    }
}
