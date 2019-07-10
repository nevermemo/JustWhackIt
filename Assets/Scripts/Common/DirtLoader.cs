using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtLoader : MonoBehaviour
{
    public static bool isLoaded = false;
    public float AnimationTime = 1.0f;

    public IEnumerator LoadDirt()
    {
        isLoaded = true;
        float Speed = (Screen.height + 200.0f) / AnimationTime;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 maxOffset = rectTransform.offsetMax;
        Vector2 minOffset = rectTransform.offsetMin;
        float slideOffset = 0.0f;

        while (minOffset.y <0 )
        {
            slideOffset = Time.deltaTime * Speed;
            maxOffset.y += slideOffset;
            minOffset.y += slideOffset;
            yield return new WaitForEndOfFrame();

            rectTransform.offsetMax = maxOffset;
            rectTransform.offsetMin = minOffset;
        }

        maxOffset.y = 120.0f;
        rectTransform.offsetMax = maxOffset;
        minOffset.y = 0.0f;
        rectTransform.offsetMin = minOffset;
        yield return null;
    }

    public IEnumerator UnloadDirt()
    {
        float Speed = (Screen.height + 200.0f) / AnimationTime;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 maxOffset = rectTransform.offsetMax;
        Vector2 minOffset = rectTransform.offsetMin;
        float slideOffset = 0.0f;

        while (minOffset.y > -Screen.height-200)
        {
            slideOffset = Time.deltaTime * Speed;
            maxOffset.y -= slideOffset;
            minOffset.y -= slideOffset;
            yield return new WaitForEndOfFrame();

            rectTransform.offsetMax = maxOffset;
            rectTransform.offsetMin = minOffset;
        }

        maxOffset.y = -Screen.height - 80;
        rectTransform.offsetMax = maxOffset;
        minOffset.y = -Screen.height - 200;
        rectTransform.offsetMin = minOffset;
        isLoaded = false;
        yield return null;
    }

    public Coroutine Load()
    {
        return StartCoroutine(LoadDirt());
    }

    public Coroutine Unload()
    {
        return StartCoroutine(UnloadDirt());
    }

    public void InstaLoad()
    {
        gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 120.0f);
        gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
        isLoaded = true;
    }

    public void InstaUnload()
    {
        gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, -Screen.height - 80);
        gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, -Screen.height - 200);
        isLoaded = false;
    }

}
