using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class SceneConfiguration
{
    public static string HammerPrefix = "";
    public static string HolePrefix = "";
    public static string MolePrefix = "";

    //Stored Prefabs
    public static GameObject Hammer;
    public static GameObject Hole;
    public static GameObject BasicMole;
    public static GameObject StrongMole;
    public static GameObject AngelMole;
    public static GameObject TimeMole;



    public static void LoadPrefabs()
    {
        //Store Prefabs from Assets
        Hammer = Resources.Load("GamePrefabs/" + HammerPrefix + "Hammer") as GameObject;
        Hole = Resources.Load("GamePrefabs/" + HolePrefix + "Hole") as GameObject;
        BasicMole = Resources.Load("GamePrefabs/" + MolePrefix + "BasicMole") as GameObject;
        StrongMole = Resources.Load("GamePrefabs/" + MolePrefix + "StrongMole") as GameObject;
        AngelMole = Resources.Load("GamePrefabs/" + MolePrefix + "AngelMole") as GameObject;
        TimeMole = Resources.Load("GamePrefabs/" + MolePrefix + "TimeMole") as GameObject;

        if (Hammer == null)
            BasicMole = Resources.Load("GamePrefabs/PlaceHolderHammer") as GameObject;
        if (Hole == null)
            Hole = Resources.Load("GamePrefabs/PlaceHolderHole") as GameObject;
        if (BasicMole == null)
            BasicMole = Resources.Load("GamePrefabs/PlaceHolderMole") as GameObject;
    }

    public static void SwitchSceneTo(string sceneName)
    {
        //Some extra steps needed to call a Coroutine from a static class
        //Also we have to make sure it survives into next scene
        //But it is low on resource demand
        ClickableObject temp = new GameObject().AddComponent<ClickableObject>();
        GameObject.DontDestroyOnLoad(temp.gameObject);
        temp.StartCoroutine(SceneTranstion(sceneName, temp));
    }

    public static IEnumerator SceneTranstion(string sceneName, MonoBehaviour temp)
    {
        //Call Canvas animations for exiting scene
        GameObject blockingPanel = GameObject.Find("BlockingPanel");
        blockingPanel.GetComponent<Image>().raycastTarget = true;
        DirtLoader dirtLoader = GameObject.Find("DirtImage").GetComponent<DirtLoader>();
        yield return dirtLoader.Load();

        yield return new WaitForSeconds(0.25f);
        yield return SceneManager.LoadSceneAsync(sceneName);
        Camera.main.gameObject.GetComponent<AudioSource>().volume = SettingsManager.mySettings.MusicLevel;
        //Scene specific adjustments/loads goes here


        //Call Canvas animations for entering scene
        dirtLoader = GameObject.Find("DirtImage").GetComponent<DirtLoader>();
        dirtLoader.InstaLoad();
        yield return new WaitForSeconds(0.25f);
        yield return dirtLoader.Unload();
        blockingPanel = GameObject.Find("BlockingPanel");
        blockingPanel.GetComponent<Image>().raycastTarget = false;

        //Destroy the temporary game object
        GameObject.Destroy(temp.gameObject);
    }
}
