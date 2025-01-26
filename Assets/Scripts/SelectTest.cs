using UnityEngine;
using Oculus.Interaction;
using TMPro;
using UnityEngine.UI;

public class SelectTest : MonoBehaviour
{
    [HideInInspector]
    public CubeInfo method;

    [HideInInspector]
    public BugInstance[] bugs;

    private Color copyColor;

    public void HandlePointerEvent(PointerEvent pointerEvent)
    {
        copyColor = gameObject.GetComponent<Renderer>().material.color;
        gameObject.GetComponent<Renderer>().material.color = new Color(0.7f, 0.7f, 0.7f);

        GameObject textGameObject = GameObject.Find("TextContent");
        TextMeshProUGUI tmpObject = textGameObject.GetComponent<TextMeshProUGUI>();
        string textContent = "Full name: " + method.FullName + "\n";
        textContent += "Lines of code: " + method.LOC + "\n";

        if (bugs.Length > 0)
        {
            textContent+=  "Bugs: " + bugs.Length + "\n";
        }
        GameObject cvs = GameObject.Find("CenteredInfoCanvas");
        cvs.GetComponent<Canvas>().enabled = true;
        cvs.transform.Find("Scroll View/Scrollbar Vertical").GetComponent<Scrollbar>().value = 1;

        AnalysisReader reader = GameObject.Find("Reader").GetComponent<AnalysisReader>();
        reader.currentlySeclected = method;

        foreach (BugInstance bug in bugs)
        {
            textContent += "\n<b><u>" + bug.ShortMessage + "</u></b>";
            textContent += "\nPriority: " + bug.Priority;
            textContent += "\n" + bug.LongMessage;

            BugPattern pattern = reader.bugCollection.BugPattern.Find(p => p.Abbrev == bug.Abbrev);
            if (pattern != null)
            {
                textContent += ParseBugDetailsHTML(pattern.Details);
            }
        }
        tmpObject.text = textContent;
    }

    public void HandlePointerRelease(PointerEvent pointerEvent)
    {
        gameObject.GetComponent<Renderer>().material.color = copyColor;
    }

    private string ParseBugDetailsHTML(string details)
    {
        string retStr = details;
        retStr = retStr.Replace("<p>", "").Replace("</p>", "").Replace("<code>", "<i>").Replace("</code>", "</i>").Replace("<br/>", "<br>").Replace("pre>", "i>").Replace("strong>", "b>").Replace("h3>", "b>").Replace("h2>", "b>");
        return retStr;
    }
}
