using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image FadeImg;
    public float fadeSpeed = 1.5f;
    public bool sceneStarting = true;

    void Update()
    {
        if (sceneStarting)
            StartScene();
    }

    void FadeToClear()
    {
        FadeImg.color = Color.Lerp(FadeImg.color, Color.clear, fadeSpeed * Time.deltaTime);
    }

    void FadeToBlack()
    {
        FadeImg.color = Color.Lerp(FadeImg.color, Color.black, fadeSpeed * Time.deltaTime);
    }

    void StartScene()
    {
        FadeToClear();
        
        if (FadeImg.color.a <= 0.05f)
        {
            FadeImg.color = Color.clear;
            FadeImg.gameObject.SetActive(false);
            sceneStarting = false;
        }
    }

    public void EndScene()
    {
        FadeImg.gameObject.SetActive(true);
        FadeToBlack();
    }
}

