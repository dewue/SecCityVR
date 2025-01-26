using UnityEngine;
using TMPro;

public class IsOnline : MonoBehaviour
{
    void Update()
    {
        GameObject connectedTextGameObject = gameObject.transform.Find("OnlineLabel").gameObject;
        AnalysisReader reader = GameObject.Find("Reader").GetComponent<AnalysisReader>();
        gameObject.transform.Find("GuidedReviewLabel").gameObject.SetActive(reader.guidedReview);
        //connectedTextGameObject.gameObject.SetActive(reader.offlineMode);
        connectedTextGameObject.gameObject.SetActive(false);

        if (NetworkManager.Instance.Runner.IsConnectedToServer)
        {
            TextMeshProUGUI tmpObject = connectedTextGameObject.GetComponent<TextMeshProUGUI>();
            tmpObject.text = NetworkManager.Instance.Runner.SessionInfo.PlayerCount + " Connected";
            connectedTextGameObject.gameObject.SetActive(true);
        }
    }
}
