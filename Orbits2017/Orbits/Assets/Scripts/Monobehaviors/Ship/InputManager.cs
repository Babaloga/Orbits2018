using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using XboxCtrlrInput;

public class InputManager : MonoBehaviour {

    public int player;
    public int controller;
    public bool useFull;
    public bool useRight;
    public float rate = 10;

    float turretFacing;
    InputSetter inSet;
    //XboxController useX;

    void Start()
    {
        if (GameObject.Find("InputChoice"))
        {
            inSet = GameObject.Find("InputChoice").GetComponent<InputSetter>();
            if (!inSet.activeDict[player])
            {
                Destroy(gameObject);
            }

            controller = inSet.controllerDict[player];
            useFull = inSet.fullDict[player];
            useRight = inSet.rightDict[player];
            RecursiveLayerAssignment(8 + inSet.teamDict[player], transform);
            //gameObject.layer = 8 + inSet.teamDict[player];
        }

    }

    public float AimDir()
    {
        if(controller == 0)
        {
            if (useFull)
            {
                Vector2 mousePos = Input.mousePosition;
                var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
                Vector2 mousePosRelative = new Vector2(-screenPoint.y + mousePos.y, screenPoint.x - mousePos.x);

                if (mousePosRelative == Vector2.zero) turretFacing += 0;
                else turretFacing = (Mathf.Atan2(mousePosRelative.y, mousePosRelative.x)) * Mathf.Rad2Deg;
                return turretFacing;
            }
            else
            {
                if (useRight)
                {
                    //DIRECTIONAL AIMING DO NOT DELETE

                    //Vector2 n = new Vector2(Input.GetAxis("Vertical 2"), -Input.GetAxis("Horizontal 2"));
                    //if (n == Vector2.zero) turretFacing += 0;
                    //else turretFacing = (Mathf.Atan2(n.y, n.x)) * Mathf.Rad2Deg;
                    //return turretFacing;

                    if (Input.GetKey("right"))
                    {
                        turretFacing -= rate;
                    }
                    if (Input.GetKey("left"))
                    {
                        turretFacing += rate;
                    }
                    return turretFacing;
                }
                else
                {
                    //DIRECTIONAL AIMING DO NOT DELETE

                    //Vector2 n = new Vector2(Input.GetAxis("Vertical"), -Input.GetAxis("Horizontal"));
                    //if (n == Vector2.zero) turretFacing += 0;
                    //else turretFacing = (Mathf.Atan2(n.y, n.x)) * Mathf.Rad2Deg;
                    //return turretFacing;
                    if (Input.GetKey("d"))
                    {
                        turretFacing -= rate;
                    }
                    if (Input.GetKey("a"))
                    {
                        turretFacing += rate;
                    }
                    return turretFacing;
                }
            }
        }
        else
        {
            if (useFull || useRight)
            {
                Vector2 n = new Vector2(Input.GetAxis("Right Stick Vertical P" + controller.ToString()), -Input.GetAxis("Right Stick Horizontal P" + controller.ToString()));

                //Vector2 n = new Vector2(XCI.GetAxis(XboxAxis.RightStickY, IntX(player)), -XCI.GetAxis(XboxAxis.RightStickX, IntX(player)));
                if (n == Vector2.zero) turretFacing += 0;
                else turretFacing = (Mathf.Atan2(n.y, n.x)) * Mathf.Rad2Deg;
                return turretFacing;
            }
            else
            {
                Vector2 n = new Vector2(Input.GetAxis("Left Stick Vertical P" + controller.ToString()), -Input.GetAxis("Left Stick Horizontal P" + controller.ToString()));

                //Vector2 n = new Vector2(XCI.GetAxis(XboxAxis.LeftStickY, IntX(player)), XCI.GetAxis(XboxAxis.LeftStickX, IntX(player)));
                if (n == Vector2.zero) turretFacing += 0;
                else turretFacing = (Mathf.Atan2(n.y, n.x)) * Mathf.Rad2Deg;
                return turretFacing;

            }
        }
    }

    public float AccelAxis()
    {
        if (controller == 0)
        {
            if (useFull)
            {
                return Input.GetAxis("Vertical");
            }
            else
            {
                if (useRight)
                {
                    //return Input.GetAxis("Accel Right");
                    return Input.GetAxis("Vertical 2");

                }
                else
                {
                    //return Input.GetAxis("Accel Left");
                    return Input.GetAxis("Vertical");
                }
            }
        }
        else
        {
            if (useFull)
            {
                if (Mathf.Abs(Input.GetAxis("Left Stick Vertical P" + controller.ToString())) > 0.2f) return Input.GetAxis("Left Stick Vertical P" + controller.ToString());
                else return 0;

                //if (Mathf.Abs(XCI.GetAxis(XboxAxis.LeftStickY, IntX(controller))) > 0.2f) return XCI.GetAxis(XboxAxis.LeftStickY, IntX(controller));
                //else return 0;
            }
            else
            {
                if (useRight)
                {
                    float a = 0;
                    if (Input.GetAxis("R Trigger P" + controller.ToString()) != 0) a -= 1;
                    if (Input.GetButton("R Bumper P" + controller.ToString())) a += 1;

                    //if (XCI.GetAxis(XboxAxis.RightTrigger, IntX(controller)) != 0) a -= 1;
                    //if (XCI.GetButton(XboxButton.RightBumper, IntX(controller))) a += 1;
                    return a;
                }
                else
                {
                    float a = 0;
                    if (Input.GetAxis("L Trigger P" + controller.ToString()) != 0) a -= 1;
                    if (Input.GetButton("L Bumper P" + controller.ToString())) a += 1;
                    //if (XCI.GetAxis(XboxAxis.LeftTrigger, IntX(controller)) != 0) a -= 1;
                    //if (XCI.GetButton(XboxButton.LeftBumper, IntX(controller))) a += 1;

                    return a;
                }
            }
        }
    }

    public bool Fire()
    {
        if (controller == 0)
        {
            if (useFull)
            {
                return Input.GetMouseButton(0);
            }
            else
            {
                if (useRight)
                {
                    return (Input.GetKey("right alt") || Input.GetMouseButton(0));
                }
                else
                {
                    return Input.GetKey("space");
                }
            }
        }
        else
        {
            if (useFull)
            {
                return Input.GetAxis("R Trigger P" + controller.ToString()) != 0;

                //return XCI.GetAxis(XboxAxis.RightTrigger, IntX(player)) > 0.1f;
            }
            else
            {
                if (useRight)
                {
                    return Input.GetButton("B Button P" + controller.ToString());

                    //return XCI.GetButton(XboxButton.B, IntX(controller));
                }
                else
                {
                    return Input.GetAxis("D Pad Vert P" + controller.ToString()) < 0;

                    //return XCI.GetButton(XboxButton.DPadDown, IntX(controller));
                }
            }
        }
    }

    //public XboxController IntX (int n)
    //{
    //    if (n == 1)
    //    {
    //        return XboxController.First;
    //    }
    //    if (n == 2)
    //    {
    //        return XboxController.Second;
    //    }
    //    if (n == 3)
    //    {
    //        return XboxController.Third;
    //    }
    //    else
    //    {
    //        return XboxController.Fourth;
    //    }
    //}

    void RecursiveLayerAssignment(int layer, Transform t)
    {
        t.gameObject.layer = layer;
        foreach(Transform child in t)
        {
            RecursiveLayerAssignment(layer, child);
        }
    }
}
