using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button3D : ClickableObject
{
    public float TravelDistance = 0.0f;
    public float DownTime = 0.15f;

    private bool isDown = false;
    private AudioSource clickNoise;

    public void Awake()
    {
        if (clickNoise == null)
        {
            clickNoise = gameObject.AddComponent<AudioSource>();
            clickNoise.clip = Resources.Load("Sounds/Click") as AudioClip;
        }
    }

    public void Start()
    {
        clickNoise.volume = SettingsManager.mySettings.SoundLevel;
    }

    public override void LeftClicked()
    {
        if (isDown)
            return;
        base.LeftClicked();
        StartCoroutine(GetPressed());
        AudioSource s = gameObject.GetComponent<AudioSource>();
        s.Play();
    }

    public IEnumerator GetPressed()
    {
        isDown = true;
        Vector3 oldPos = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y - TravelDistance, transform.position.z);
        yield return new WaitForSecondsRealtime(DownTime);
        transform.position = oldPos;
        isDown = false;
    }

}
