using UnityEngine;
using UnityEngine.UI;

public class ThumbScroller : MonoBehaviour
{
    [SerializeField]
    public float scrollSpeed = 500f;

    // Update is called once per frame
    void Update()
    {
        ScrollRect scrollRect = gameObject.GetComponentInParent<ScrollRect>();
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickDown) && gameObject.GetComponent<Scrollbar>().value > 0)
        {
            //gameObject.GetComponent<Scrollbar>().value -= Time.deltaTime * scrollSpeed;
            float contentHeight = scrollRect.content.sizeDelta.y;
            float contentShift = scrollSpeed * Time.deltaTime;
            gameObject.GetComponent<Scrollbar>().value -= contentShift / contentHeight;
        }
        else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickUp))
        {
            //gameObject.GetComponent<Scrollbar>().value += Time.deltaTime * scrollSpeed;
            float contentHeight = scrollRect.content.sizeDelta.y;
            float contentShift = scrollSpeed *  Time.deltaTime;
            gameObject.GetComponent<Scrollbar>().value += contentShift / contentHeight;
        }

    }
}
