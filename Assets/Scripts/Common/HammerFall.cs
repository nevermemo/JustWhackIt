using ClassicGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerFall : MonoBehaviour
{
    public float FallAngle = -45.0f;
    public float FallTime = 0.5f;
    public int ClickType = 0;           //0 for left click, 1 for right click 
    public int HoleNumber;

    void Start()
    {
        FallAngle = transform.rotation.eulerAngles.x;
        transform.parent.gameObject.GetComponent<AudioSource>().volume = SettingsManager.mySettings.SoundLevel;
        StartCoroutine( AnimationCoR() );
    }

    public IEnumerator AnimationCoR()
    {
        //Launch the particles
        GameManager.HoleList[HoleNumber].gameObject.GetComponent<ParticleSystem>().Play();

        //Fall
        float Speed = FallAngle / FallTime;
        while (transform.localEulerAngles.x > 0 && transform.localEulerAngles.x >= FallAngle)
        {
            if (GameManager.isPaused)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }
            transform.Rotate(Vector3.right, Time.deltaTime * Speed, Space.World);
            yield return new WaitForEndOfFrame();
        }
        transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        yield return new WaitForEndOfFrame();
        
        yield return new WaitForSecondsRealtime(FallTime / 3);
        Speed = FallAngle / (FallTime * (2.0f / 3.0f) );

        //Rise
        while (transform.localEulerAngles.x > FallAngle || transform.localEulerAngles.x == 0.0f)
        {
            if (GameManager.isPaused)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }
            transform.Rotate(Vector3.right, -Time.deltaTime * Speed, Space.World);
            yield return new WaitForEndOfFrame();
        }

        //Get removed
        GameObject.Destroy(transform.parent.gameObject);
        yield return null;
    }


}
