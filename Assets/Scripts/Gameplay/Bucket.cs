using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bucket : MonoBehaviour
{
    public BaloonColor color;

    public int requirement;

    public Text text;

    private Renderer rend;

    public List<Bucket> ToList { get; internal set; }

    private void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
    }

    private void Start()
    {
        rend.material = LevelManager.Instance.gameConfig.colorMaterials.Find(x => x.color == color).material;
    }

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
