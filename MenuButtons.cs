using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MenuButtons : MonoBehaviour
{
    public Image panel;
    public float openTime;
    public AudioSource backgroundMusic;
    public AudioSource openingMusic;
    public Toggle reversifierToggle;

    void Start()
    {
        // 0 Means True

        bool revIsOn = PlayerPrefs.GetInt("reversifier", 0) == 0;
        reversifierToggle.isOn = revIsOn;
    }

    public void Open()
    {
        panel.DOFade(1, openTime);
        StartCoroutine(LoadGameScene());

        backgroundMusic.Stop();
        openingMusic.Play();
    }

    public void OnReversifierValueChanged()
    {
        if (reversifierToggle.isOn) PlayerPrefs.SetInt("reversifier", 0);
        else PlayerPrefs.SetInt("reversifier", 1);
    }

    IEnumerator LoadGameScene()
    {
        // Waits for the panel to black out and then loads game

        yield return new WaitForSeconds(openTime);
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();

        // For the Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
