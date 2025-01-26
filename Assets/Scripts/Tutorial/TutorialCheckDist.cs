using UnityEngine;
using UnityEngine.UIElements;

public class TutorialCheckDist : MonoBehaviour
{
    public GameObject goal;
    public GameObject player;
    public GameObject linePrefab;

    GameObject lineObject;

    private void Start()
    {
        lineObject = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineObject.SetActive(false);
        goal.SetActive(false);
    }

    void Update()
    {
        LineRenderer lineRenderer = lineObject.transform.Find("LineComponentRenderer").GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        Vector3[] positions = new Vector3[2] { transform.position, goal.transform.position };
        lineRenderer.SetPositions(positions);
        lineObject.SetActive(true);
        goal.SetActive(true);

        if (Vector3.Distance(goal.transform.position, player.transform.position) < 10)
        {
            gameObject.transform.parent.position = new Vector3(goal.transform.position.x,5.5f, goal.transform.position.z);
            gameObject.transform.parent.transform.eulerAngles = Vector3.zero;
            goal.SetActive(false);
            lineObject.SetActive(false);
            gameObject.GetComponent<TutorialTask>().CompleteTask();
        }
    }
}
