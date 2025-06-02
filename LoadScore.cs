using UnityEngine;
using TMPro;

public class LoadScore : MonoBehaviour
{
    void Start()
    {
        TextMeshProUGUI score = GetComponent<TextMeshProUGUI>();
        score.text = "Scored " + PlayerPrefs.GetInt("score", 0) + " with " + PlayerPrefs.GetInt("ignition", 0) + " Ignitions";
    }
}
