using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeBlack : MonoBehaviour
{
    public bool fadeToBlack;
    public float fadeSpeed;
    private Image objectImage;
    private Color objectColor;

    public void DoFade(bool fadeToBlack, float fadeSpeed)
    {
        this.fadeToBlack = fadeToBlack;
        this.fadeSpeed = fadeSpeed;
        StartCoroutine(FadeBlackoutSquare());
    }

    public bool IsFaded()
    {
        objectImage = GetComponent<Image>();
        objectColor = objectImage.color;
        if(objectColor.a >= 1)
            return true;
        return false;
    }
    public IEnumerator FadeBlackoutSquare()
    {
        Image objectImage = GetComponent<Image>();
        Color objectColor = objectImage.color;
        float fadeAmount;

        if(fadeToBlack)
        {
            while(GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
                objectColor.a = fadeAmount;
                objectImage.color = objectColor;
                yield return null;
            }
        }
        else
        {
            while(GetComponent<Image>().color.a > 1)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
                objectColor.a = fadeAmount;
                objectImage.color = objectColor;
                yield return null;
            }
        }

        yield return new WaitForEndOfFrame();
    }

}
