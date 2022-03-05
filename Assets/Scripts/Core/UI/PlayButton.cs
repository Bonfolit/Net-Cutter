using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : Button
{
    protected override void Start()
    {
        this.onClick.AddListener(() =>
        {
            GameManager.Instance.StartLevel();
        });

        base.Start();
    }
}
