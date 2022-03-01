using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node: MonoBehaviour
{
    public Vector2Int coordinate;

    public Vector2
        position,
        prevPosition;

    public float moveability;

    public bool isLocked;

    public Baloon baloon;

    public List<Line> connectedLines;

    public List<LineRenderer> lineRenderers;

    public Vector2 GravityStep => Vector2.up * Physics.gravity.y * Time.deltaTime * Time.deltaTime;
    public Vector2 MoveStep => (position - prevPosition);

    public Node(Vector2 _position, Vector2Int _coordinate, bool _isLocked)
    {
        position = _position;

        prevPosition = _position;

        coordinate = _coordinate;

        isLocked = _isLocked;
    }

    public void MoveTowards(Vector2 targetPos)
    {
        if (baloon)
        {
            position = Vector2.Lerp(position, targetPos, moveability);
        }
        else
        {
            position = targetPos;
        }
    }

    public void Initialize(Vector2 _position, Vector2Int _coordinate, bool _isLocked)
    {
        position = _position;

        prevPosition = _position;

        coordinate = _coordinate;

        isLocked = _isLocked;
    }

    private void Update()
    {
        if (baloon)
        {
            //baloon.transform.position = (Vector3)position;
        }
    }
}