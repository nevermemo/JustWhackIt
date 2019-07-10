using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ClassicGame
{
    public class InputManager : MonoBehaviour
    {
        public float Cooldown = 1.0f;
        private float MouseCooldown = 0.0f;

        //CACHED VARIABLES//
        private RaycastHit clickInfo = new RaycastHit();
        private Ray clickRay = new Ray();
        //END CACHED VARIABLES//

        public void Update()
        {
            //Lower Mouse Cooldown
            if (!GameManager.isPaused)
            {
                MouseCooldown -= Time.deltaTime;
                MouseCooldown = Mathf.Max(0.0f, MouseCooldown);
            }

            //Idle if mouse is idle
            if ( (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1)) || MouseCooldown > 0.0f)
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
            if(!Physics.Raycast(clickRay, out clickInfo))
            {
                //Return if no object was hit
                return;
            }

            //Check For Object Tag
            switch(clickInfo.transform.gameObject.tag)
            {
                case "Button":
                    clickInfo.transform.GetComponent<ClickableObject>().LeftClicked();
                    ExecuteCommand(clickInfo.transform.GetComponent<ClickableObject>().ObjectCommand);
                    break;

                case "Clickable":
                    if (Input.GetMouseButtonDown(0))
                    {
                        clickInfo.transform.gameObject.GetComponent<ClickableObject>().LeftClicked();
                    }
                    else
                    {
                        clickInfo.transform.gameObject.GetComponent<ClickableObject>().RightClicked();
                        MouseCooldown = Cooldown;
                    }
                    break;

                default:
                    break;
            }

        }

        private void ExecuteCommand(string command)
        {
            string[] cmdLines = command.Split('_');

            switch (cmdLines[0])
            {
                case "PauseButton":
                    gameObject.GetComponent<GameManager>().PauseGame();
                    break;

                default:
                    break;
            }

        }
    }
}