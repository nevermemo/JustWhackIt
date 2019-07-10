using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MainMenu
{
    public class InputManager : MonoBehaviour
    {
        public float CameraSlideSpeed = 10.0f;

        public Transform MainLocation;
        public Transform SkinsLocation;
        public Transform AdvancedGuideLocation;
        public GameObject OptionsPanel;
        public GameObject HighScoresPanel;
        public GameObject CreditsPanel;

        //CACHED VARIABLES//
        private RaycastHit clickInfo = new RaycastHit();
        private Ray clickRay = new Ray();
        //END CACHED VARIABLES//

        public void Awake()
        {
            SettingsManager.LoadSettings();
        }

        public void Start()
        {
            SceneConfiguration.LoadPrefabs();
            Camera.main.gameObject.GetComponent<AudioSource>().volume = SettingsManager.mySettings.MusicLevel;
            OptionsPanel.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Slider>().value = SettingsManager.mySettings.MusicLevel;
            OptionsPanel.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<Slider>().value = SettingsManager.mySettings.SoundLevel;
            SetSkinArrows();
        }


        void Update()
        {
            //Idle if mouse is idle
            if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
            {
                return;
            }

            //Ignore if we are over UI
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            //Raytrace to mouse
            clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(clickRay, out clickInfo))
            {
                //Return if no object was hit
                return;
            }

            //Check For Object Tag
            switch (clickInfo.transform.gameObject.tag)
            {
                case "Clickable":
                    clickInfo.transform.GetComponent<ClickableObject>().LeftClicked();
                    ExecuteObjectCommand(clickInfo.transform.GetComponent<ClickableObject>().ObjectCommand);
                    break;

                default:
                    break;
            }



        }

        private void ExecuteObjectCommand(string command)
        {
            string[] cmdLines = command.Split('_');

            switch (cmdLines[0])
            {
                //Load Classic Game Scene
                case "ClassicGame":
                    SceneConfiguration.SwitchSceneTo("ClassicGame");
                    break;

                //Load Advanced Game Scene
                case "AdvancedGame":
                    SceneConfiguration.SwitchSceneTo("AdvancedGame");
                    break;

                case "Options":
                    OptionsPanel.SetActive(true);
                    break;

                case "Credits":
                    CreditsPanel.SetActive(true);
                    break;

                //Slide to Skin selection
                case "Skins":
                    StartCoroutine(SlideCamera(SkinsLocation));
                    break;

                //Slide to Advanced Guide 
                case "AdvancedGuide":
                    StartCoroutine(SlideCamera(AdvancedGuideLocation));
                    break;

                //Slide to Main
                case "Main":
                    StartCoroutine(SlideCamera(MainLocation));
                    break;

                //Select a skin
                case "Skin":
                    string prefix = cmdLines[2];
                    if (prefix == "Default")
                        prefix = "";

                    //Disappear arrows to reappear the right one
                    Transform arrowContainer = GameObject.Find(cmdLines[1] + "ArrowsContainer").transform;
                    if (arrowContainer.childCount > 0)
                        foreach (Transform t in arrowContainer)
                        {
                            t.gameObject.SetActive(false);
                            if (t.gameObject.name == prefix + cmdLines[1] + "Arrow")
                                t.gameObject.SetActive(true);
                        }

                    switch (cmdLines[1])
                    {
                        case "Hammer":
                            SceneConfiguration.HammerPrefix = prefix;
                            break;
                        case "Hole":
                            SceneConfiguration.HolePrefix = prefix;
                            break;
                        case "Mole":
                            SceneConfiguration.MolePrefix = prefix;
                            break;

                        default:
                            break;
                    }
                    SettingsManager.SaveSettings();
                    break;

                case "HighScores":
                    HighScoreManager.LoadAllLocalHighScores();
                    Text names = HighScoresPanel.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
                    Text scores = HighScoresPanel.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<Text>();

                    names.text = "";
                    scores.text = "";

                    foreach (HighScoreManager.ScoreEntry e in HighScoreManager.LocalHighscores.Entries)
                    {
                        names.text += e.Name.Substring(0, Mathf.Min(e.Name.Length, 6)) + "\n";
                        scores.text += e.Score + "\n";
                    }

                    Text advancedNames = HighScoresPanel.transform.GetChild(0).GetChild(0).GetChild(2).gameObject.GetComponent<Text>();
                    Text advancedScores = HighScoresPanel.transform.GetChild(0).GetChild(0).GetChild(3).gameObject.GetComponent<Text>();

                    advancedNames.text = "";
                    advancedScores.text = "";

                    foreach (HighScoreManager.ScoreEntry e in HighScoreManager.LocalAdvancedHighscores.Entries)
                    {
                        advancedNames.text += e.Name.Substring(0, Mathf.Min(e.Name.Length, 6)) + "\n";
                        advancedScores.text += e.Score + "\n";
                    }

                    HighScoresPanel.SetActive(true);
                    break;

                case "Exit":
                    Debug.LogWarning("Exiting");
                    Application.Quit(0);
                    break;

                default:
                    break;
            }
        }

        public void SetSkinArrows()
        {
            ExecuteObjectCommand("Skin_Hammer_" + SettingsManager.mySettings.HammerPrefix);
            ExecuteObjectCommand("Skin_Hole_" + SettingsManager.mySettings.HolePrefix);
            ExecuteObjectCommand("Skin_Mole_" + SettingsManager.mySettings.MolePrefix);
        }

        public void ResetHighscores()
        {
            HighScoreManager.ResetAllLocalHighScores();
        }

        public void ResetSettings()
        {
            SettingsManager.ResetSettings();
            Camera.main.gameObject.GetComponent<AudioSource>().volume = SettingsManager.mySettings.MusicLevel;
            OptionsPanel.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Slider>().value = SettingsManager.mySettings.MusicLevel;
            OptionsPanel.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<Slider>().value = SettingsManager.mySettings.SoundLevel;
            SetSkinArrows();
        }

        public void SetMusicLevel(float lvl)
        {
            SettingsManager.mySettings.MusicLevel = lvl;
            Camera.main.gameObject.GetComponent<AudioSource>().volume = SettingsManager.mySettings.MusicLevel;
            SettingsManager.SaveSettings();
        }

        public void SetSoundLevel(float lvl)
        {
            SettingsManager.mySettings.SoundLevel = lvl;
            SettingsManager.SaveSettings();
        }

        private IEnumerator SlideCamera(Transform switchTo)
        {
            Camera cam = Camera.main;
            //Block User Input for the duration;
            Image blockingPanel = GameObject.Find("BlockingPanel").GetComponent<Image>();
            blockingPanel.raycastTarget = true;

            while( (switchTo.position -cam.transform.position).magnitude > 0.001f)
            {
                cam.transform.position = Vector3.MoveTowards(cam.transform.position, switchTo.position, Time.deltaTime * CameraSlideSpeed);
                yield return new WaitForEndOfFrame();
            }
            cam.transform.position = switchTo.position;

            //Unblock Screen
            blockingPanel.raycastTarget = false;

            yield return new WaitForEndOfFrame();
        }

    }
}
