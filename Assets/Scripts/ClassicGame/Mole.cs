using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClassicGame
{

    public class Mole : ClickableObject
    {
        public int HoleNumber;
        public float WaitTime = 1.0f;
        public float RiseTime = 0.5f;
        public float RiseDistance = 1.0f;
        public int Health = 1;
        public int Point = 10;

        private Coroutine myCoR = null;

        public void Start()
        {
            ObjectCommand = "Mole";
            RiseDistance = -transform.position.y;
            myCoR = StartCoroutine( LifeCoR() );
        }

        public virtual void GetHit(int damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                Die();
            }
        }

        public virtual void Die()
        {
            GameManager.Score += Point;
            StopCoroutine( myCoR );
            StartCoroutine( DeathCoR() );
        }

        public override void LeftClicked()
        {
            SummonHammer(0);
            GetHit(GameManager.Damage);
        }

        public override void RightClicked()
        {
            SummonHammer(1);
            GetHit(GameManager.Damage*10);
        }

        public void SummonHammer(int c)
        {
            GameObject hammer = GameObject.Instantiate(SceneConfiguration.Hammer);
            hammer.transform.position = GameManager.HoleList[HoleNumber].position;
            hammer.transform.GetChild(0).GetComponent<HammerFall>().ClickType = c;
            hammer.transform.GetChild(0).GetComponent<HammerFall>().HoleNumber = this.HoleNumber;
        }

        public virtual IEnumerator LifeCoR()
        {
            float Speed = RiseDistance / RiseTime;
            //Rise
            while (transform.position.y < 0)
            {
                if (GameManager.isPaused)
                {
                    yield return new WaitForEndOfFrame();
                    continue;
                }
                transform.position += Vector3.up * Time.deltaTime * Speed;
                yield return new WaitForEndOfFrame();
            }
            transform.position.Set( transform.position.x, 0.0f , transform.position.z );

            //Wait
            yield return new WaitForSecondsRealtime( WaitTime );

            //Fall
            while (transform.position.y > -RiseDistance)
            {
                if (GameManager.isPaused)
                {
                    yield return new WaitForEndOfFrame();
                    continue;
                }
                transform.position -= Vector3.up * Time.deltaTime * Speed;
                yield return new WaitForEndOfFrame();
            }

            //Get removed
            GameManager.RemoveMole(this);
        }

        public virtual IEnumerator DeathCoR()
        {
            float Speed = RiseDistance / RiseTime;
            //Make the mole non-raycast target
            gameObject.layer = 2;

            //Fall
            while (transform.position.y > -RiseDistance)
            {
                if (GameManager.isPaused)
                {
                    yield return new WaitForEndOfFrame();
                    continue;
                }
                transform.position -= Vector3.up * Time.deltaTime * Speed * 2.0f;
                yield return new WaitForEndOfFrame();
            }

            //Get removed
            GameManager.RemoveMole(this);
        }

    }
}
