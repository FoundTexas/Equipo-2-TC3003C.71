using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeBlack : MonoBehaviour
{
    public bool fadeToBlack;
    public float fadeSpeed;

    public void DoFade(bool fadeToBlack, float fadeSpeed)
    {
        this.fadeToBlack = fadeToBlack;
        this.fadeSpeed = fadeSpeed;
        gameObject.SetActive(true);
        StartCoroutine(FadeBlackoutSquare());
    }

    public bool IsFaded()   
    {
        Image objectImage = GetComponent<Image>();
        Color objectColor = objectImage.color;
        return (objectColor.a >= 255);
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
            while(GetComponent<Image>().color.a < 1)
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
