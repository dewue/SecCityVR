using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTask : MonoBehaviour
{
    public bool IsCompleted { get; private set; } = false;

    public void CompleteTask()
    {
        IsCompleted = true;
    }
}
