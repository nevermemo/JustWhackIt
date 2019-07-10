using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ClassicGame
{
    public class GameManager : MonoBehaviour
    {
        public static int GameMode = 0;
        public static bool isPaused = true;
        public static int Score = 0;
        public static int Damage = 1;
        public static float TimeRemaining = 60.0f;
        public static float TimeToNextMole = 0.0f;
        public static List<Transform> HoleList = new List<Transform>();
        public static List<int> EmptyHoles = new List<int>();
        public static List<Mole> MoleList = new List<Mole>();

        public GameObject HoleContainer;
        public float[] MoleSpawnInterval;
        
        public Text ScoreText;
        public Text TimeText;
        public Text CountdownText;
        public Image PauseMenuPanel;

        public virtual void Start()
        {
            SceneConfiguration.LoadPrefabs();
            GameMode = 0;
            Reset();

            //Initialize other Lists based on Hole List
            for (int i = 0; i < HoleContainer.transform.childCount; i++)
            {
                Transform placeHolderHole = HoleContainer.transform.GetChild(i);
                Transform newHole = GameObject.Instantiate(SceneConfiguration.Hole).transform;
                newHole.position = new Vector3(placeHolderHole.position.x, newHole.position.y, placeHolderHole.position.z);
                placeHolderHole.gameObject.SetActive(false);

                HoleList.Add( newHole);
                EmptyHoles.Add(i);
                MoleList.Add(null);
            }

            StartCoroutine( StartingCountdown() );
        }

        public virtual void Update()
        {
            if (TimeRemaining < 0.0f)
            {
                TimeRemaining = 0.0f;
                GameOver();
            }

            if (isPaused)
                return;

            TimeToNextMole -= Time.deltaTime;
            TimeRemaining -= Time.deltaTime;

            if (TimeToNextMole <= 0.0f)
            {
                this.GenerateNewMole();
                TimeToNextMole = Random.Range(MoleSpawnInterval[0], MoleSpawnInterval[1]);
            }
            
            TimeText.text = "Time: " + TimeRemaining.ToString("00");
            ScoreText.text = "Score: " + Score.ToString();

        }

        public void Reset()
        {
            Score = 0;
            Damage = 1;
            TimeRemaining = 60.0f;
            isPaused = true;
            HoleList = new List<Transform>();
            EmptyHoles = new List<int>();
            MoleList = new List<Mole>();
    }

        public void PauseGame()
        {
            isPaused = true;
            PauseMenuPanel.gameObject.SetActive(true);
        }

        public void ResumeGame()
        {
            PauseMenuPanel.gameObject.SetActive(false);
            StartCoroutine(StartingCountdown());
        }

        public void GameOver()
        {
            GameObject.Find("BlockingPanel").GetComponent<Image>().raycastTarget = true;
            isPaused = true;
            StartCoroutine(Outro());
        }

        public void ExitGame()
        {
            isPaused = true;
            SceneConfiguration.SwitchSceneTo("MainMenu");
        }

        public virtual void GenerateNewMole()
        {
            if (EmptyHoles.Count == 0)
                return;
            //Select a random hole
            int newMoleHole = EmptyHoles[ Random.Range(0, EmptyHoles.Count) ];
            EmptyHoles.Remove(newMoleHole);

            //Place it into scene
            GameObject MoleObject = GameObject.Instantiate(SceneConfiguration.BasicMole);
            MoleObject.transform.position = new Vector3(HoleList[newMoleHole].position.x, MoleObject.transform.position.y, HoleList[newMoleHole].position.z);
            MoleList[newMoleHole] = MoleObject.GetComponent<Mole>();
            MoleList[newMoleHole].HoleNumber = newMoleHole;
        }

        public static void RemoveMole(Mole moe)
        {
            EmptyHoles.Add(moe.HoleNumber);
            MoleList[moe.HoleNumber] = null;
            Destroy(moe.gameObject);
        }

        public IEnumerator StartingCountdown()
        {
            //Wait for Loading transition
            while (DirtLoader.isLoaded)
                yield return new WaitForEndOfFrame();

            Image blockingPanelImage = GameObject.Find("BlockingPanel").GetComponent<Image>();
            blockingPanelImage.raycastTarget = true;

            CountdownText.gameObject.SetActive(true);
            CountdownText.text = "3";
            yield return new WaitForSecondsRealtime(1);
            CountdownText.text = "2";
            yield return new WaitForSecondsRealtime(1);
            CountdownText.text = "1";
            yield return new WaitForSecondsRealtime(1);
            CountdownText.text = "GO!";
            yield return new WaitForSecondsRealtime(1);
            isPaused = false;
            CountdownText.gameObject.SetActive(false);

            blockingPanelImage.raycastTarget = false;
            yield return new WaitForEndOfFrame();
        }

        //Display Game Over
        public IEnumerator Outro()
        {
            Image blockingPanelImage = GameObject.Find("BlockingPanel").GetComponent<Image>();
            blockingPanelImage.raycastTarget = true;

            CountdownText.gameObject.SetActive(true);
            CountdownText.text = "Game Over";

            yield return new WaitForSecondsRealtime(3);

            SceneConfiguration.SwitchSceneTo("ScoreScreen");
        }




    }
}
