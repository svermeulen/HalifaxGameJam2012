using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DolphinPathCreator : MonoBehaviour
{
    public DolphinController controller;
    public Transform dolphin;

    public float sampleStartDistance = 0;

    public float sampleDistance = 0.75f;
    public int maxSampleRange = 8;
    public float coastDistance = 0;

    bool isCreatingPath = false;
    Vector3 lastSamplePoint = Vector3.zero;
    int numPointsCreated = 0;
    bool curveStarted = false;

    Vector3 GetMousePosInWorld()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        mousePos.z = 0;
        return mousePos;
    }

	void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = GetMousePosInWorld();
            var bounds = dolphin.collider.bounds;

            var clickRadius = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);

            //Debugging.Instance.ShowText("dist: " + (mousePos - bounds.center).magnitude);
            if ((mousePos - bounds.center).magnitude < clickRadius)
            {
                controller.OnDolphinPathStarted();
                isCreatingPath = true;
                curveStarted = false;
                numPointsCreated = 0;
            }
        }
        else if (Input.GetMouseButtonUp(0) && isCreatingPath)
        {
            var mousePos = GetMousePosInWorld();

            var newSample = new Vector3(mousePos.x, mousePos.y, 0);
            controller.OnDolphinPathPointAdded(newSample);

            isCreatingPath = false;
        }

        //Debugging.Instance.ShowText("Mouse pos: " + Input.mousePosition);

        if (isCreatingPath)
        {
            var mousePos = GetMousePosInWorld();

            if (curveStarted)
            {
                if (controller.IsUpdatingFromPath)
                {
                    var newSample = new Vector3(mousePos.x, mousePos.y, 0);
                    var cursorDistance = (newSample - lastSamplePoint).magnitude;

                    if (cursorDistance > sampleDistance)
                    {
                        controller.OnDolphinPathPointAdded(newSample);
                        lastSamplePoint = newSample;
                    }
                }
                else
                {
                    curveStarted = false;
                    controller.OnDolphinPathStarted();
                }
            }
            else
            {
                var delta = (mousePos - dolphin.collider.bounds.center);
                var cursorDist = delta.magnitude;
                delta /= cursorDist;

                if (cursorDist > coastDistance)
                {
                    if (cursorDist > sampleStartDistance)
                    {
                        curveStarted = true;

                        var startPos = dolphin.collider.bounds.center;
                        var delta2 = mousePos - startPos;

                        controller.OnDolphinPathPointAdded(startPos);
                        controller.OnDolphinPathPointAdded(startPos + delta2 * 0.25f);
                        controller.OnDolphinPathPointAdded(mousePos);
                        lastSamplePoint = mousePos;
                    }
                    else
                    {
                        var theta = Mathf.Atan2(delta.y, delta.x);
                        var desiredRotation = Quaternion.AngleAxis((Mathf.Rad2Deg * theta) + 90, new Vector3(0, 0, 1));
                        dolphin.transform.rotation = Quaternion.Slerp(dolphin.transform.rotation, desiredRotation, controller.angleMoveSpeed);

                        controller.velocity = delta * controller.moveSpeed;
                    }
                }
            }
        }
	}
}
