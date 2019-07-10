using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClassicGame;

namespace AdvancedGame
{
    public class AdvancedGameManager : GameManager
    {
        public float BasicMoleWeight = 1.0f;
        public float StrongMoleWeight = 1.0f;
        public float AngelMoleWeight = 1.0f;
        public float TimeMoleWeight = 1.0f;

        public override void Start()
        {
            base.Start();
            GameMode = 1;
        }

        public override void GenerateNewMole()
        {
            if (EmptyHoles.Count == 0)
                return;

            //Select a random hole
            int newMoleHole = EmptyHoles[Random.Range(0, EmptyHoles.Count)];
            EmptyHoles.Remove(newMoleHole);

            //Select a random mole
            GameObject MoleObject;
            if (RandomWeighted(BasicMoleWeight, BasicMoleWeight + StrongMoleWeight + AngelMoleWeight + TimeMoleWeight))
            {
                MoleObject = GameObject.Instantiate(SceneConfiguration.BasicMole);
            }
            else if (RandomWeighted(StrongMoleWeight, StrongMoleWeight + AngelMoleWeight + TimeMoleWeight))
            {
                MoleObject = GameObject.Instantiate(SceneConfiguration.StrongMole);
            }
            else if (RandomWeighted(AngelMoleWeight, AngelMoleWeight + TimeMoleWeight))
            {
                MoleObject = GameObject.Instantiate(SceneConfiguration.AngelMole);
            }
            else
            {
                MoleObject = GameObject.Instantiate(SceneConfiguration.TimeMole);
            }

            //Place it into scene
            MoleObject.transform.position = new Vector3(HoleList[newMoleHole].position.x, MoleObject.transform.position.y, HoleList[newMoleHole].position.z);
            MoleList[newMoleHole] = MoleObject.GetComponent<Mole>();
            MoleList[newMoleHole].HoleNumber = newMoleHole;
        }

        public bool RandomWeighted(float weight, float total)
        {
            return (weight >= Random.Range(0, total));
        }
    }
}
