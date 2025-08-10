using System.Collections;
using TMPro;
using UnityEngine;

public class PopupUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PopupStart(Color color)
    {
        StartCoroutine(Fade(color));
    }

    private IEnumerator Fade(Color color)
    {
        this.gameObject.GetComponent<TextMeshProUGUI>().color = color;
        yield return new WaitForSeconds(1);
        this.gameObject.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f);
    }
}
