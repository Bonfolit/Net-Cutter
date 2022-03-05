using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextButton : Button
{
    protected override void Start()
    {
        this.onClick.AddListener(() =>
        {
            GameManager.Instance.NextLevel();
        });

        base.Start();
    }
}
