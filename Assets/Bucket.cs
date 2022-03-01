using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bucket : MonoBehaviour
{
    public BaloonColor color;

    public int requirement;

    public Text text;

    public bool TryConsume(Baloon baloon)
    {
        if (baloon.color == color)
        {
            requirement--;

            return true;
        }

        return false;
    }
}
