using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_MethodPart : MonoBehaviour
{
    public GameObject[] dialogs;
    private int currentDialogIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        dialogs[0].SetActive(true);
        for (int i = 1; i < dialogs.Length; i++)
        {
            dialogs[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDialogIndex < dialogs.Length && dialogs[currentDialogIndex].GetComponent<TutorialTask>().IsCompleted)
        {
            dialogs[currentDialogIndex].gameObject.SetActive(false);

            currentDialogIndex++;
            if (currentDialogIndex < dialogs.Length)
            {
                dialogs[currentDialogIndex].gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
