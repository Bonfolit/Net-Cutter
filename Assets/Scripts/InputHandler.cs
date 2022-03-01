using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputHandler : MonoBehaviour
{
    private Plane gamePlane;

    private Vector3 mousePos;

    public float minDistance;

    public Action<Vector3, Vector3> onMouseSwipe;

    public Action<Vector3> onMouseHold;

    private void Awake()
    {
        gamePlane = new Plane(Vector3.back, Vector3.zero);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            mousePos = Input.mousePosition;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 newPos = Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(newPos);

            if (gamePlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);

                onMouseHold?.Invoke(hitPoint);
            }

            //if (Vector3.Distance(mousePos, newPos) > minDistance)
            //{
            //    onMouseSwipe?.Invoke(mousePos, newPos);
            //}

            mousePos = newPos;
        }
    }
}
