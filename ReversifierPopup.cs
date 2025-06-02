using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class ReversifierPopup : MonoBehaviour
{
    public TextMeshProUGUI popupText;
    public RectTransform textRect;
    public Vector2 endPosition;
    public float fadeInTime;
    public float fadeDragTime;
    public float fadeOutTime;
    public float waitDuration;

    public void ShowPopup(string text)
    {
        StartCoroutine(ShowPopupCoroutine(popupText, textRect, text));
    }

    IEnumerator ShowPopupCoroutine(TextMeshProUGUI text, RectTransform textRect, string popupData)
    {
        Vector2 inititalPosition = textRect.transform.position;

        text.text = popupData;

        text.DOFade(1, fadeInTime);
        textRect.DOAnchorPos(endPosition, fadeDragTime).SetEase(Ease.OutQuint);
        yield return new WaitForSeconds(waitDuration);

        text.DOFade(0, fadeOutTime);
        yield return new WaitForSeconds(fadeOutTime + 0.1f);

        textRect.position = inititalPosition;
    }
}
