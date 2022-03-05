using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour, IGameStateListener
{
    public static LevelManager Instance;

    public GameConfigSO gameConfig;

    public List<Bucket> buckets;

    public List<Baloon> baloons;

    public Action<Baloon> onBaloonConsume;

    private void Awake()
    {
        Instance = this;

        gameConfig = Resources.Load<GameConfigSO>("GameConfig");
    }

    private bool CheckCompletion()
    {
        foreach (Bucket bucket in buckets)
        {
            if (bucket.requirement > 0)
            {
                return false;
            }
        }

        return true;
    }

    private void OnBaloonConsume(Baloon baloon)
    {
        if (!baloons.Contains(baloon))
        {
            Debug.LogWarning("Trying to remove a baloon that was already removed!");

            return;
        }

        NetManager.Instance.RemoveBaloon(baloon);

        if (CheckCompletion())
        {
            GameManager.Instance.SetState(GameState.Success);
        }
    }

    public void OnGameStateChanged(GameState from, GameState to)
    {
        if (to == GameState.Playing)
        {
            buckets = FindObjectsOfType<Bucket>().ToList();
            baloons = FindObjectsOfType<Baloon>().ToList();

            onBaloonConsume += OnBaloonConsume;
        }
        if (to == GameState.Success)
        {
            onBaloonConsume -= OnBaloonConsume;
        }
        if (to == GameState.Failed)
        {
            onBaloonConsume -= OnBaloonConsume;
        }
    }
}
