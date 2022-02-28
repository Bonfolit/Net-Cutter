using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2Int coordinate;

    public Vector2
        position,
        prevPosition;

    public GameObject debugObject;

    public float moveability;

    public bool isLocked;

    public bool isHeavy;

    public Vector2 GravityStep => Vector2.up * Physics.gravity.y * Time.deltaTime * Time.deltaTime;
    public Vector2 MoveStep => (position - prevPosition);

    public Node(Vector2 _position, Vector2Int _coordinate, bool _isLocked)
    {
        position = _position;

        prevPosition = _position;

        coordinate = _coordinate;

        isLocked = _isLocked;

        //isHeavy = coordinate.x == 0 && coordinate.y == 0 ? true : false;
    }

    
}