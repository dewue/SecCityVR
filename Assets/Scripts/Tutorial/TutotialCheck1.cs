using UnityEngine;

public class TutorialSelect1 : MonoBehaviour
{
    
    void Update()
    {
        if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0f)
        {
            gameObject.GetComponent<TutorialTask>().CompleteTask();
        }
    }
}
