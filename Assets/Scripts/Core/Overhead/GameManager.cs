using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameState state;

    private int level;

    private int levelCount;

    public Action<GameState, GameState> onStateChanged;

    public GameState State
    {
        get => state;

        private set
        {
            if (state != value)
            {
                state = value;
            }
        }
    }

    public int Level
    {
        get => level;

        private set
        {
            level = (value % levelCount);

            PlayerPrefs.SetInt("level", level);
        }
    }

    private void Awake()
    {
        Instance = this;

        levelCount = SceneManager.sceneCountInBuildSettings;
    }

    private void Start()
    {
        SetState(GameState.Initial);

        level = PlayerPrefs.GetInt("level", 0);
    }

    public void SetState(GameState state)
    {
        if (State == state)
        {
            return;
        }

        GameState temp = State;

        State = state;

        onStateChanged?.Invoke(temp, State);

        var listeners = FindObjectsOfType<MonoBehaviour>().OfType<IGameStateListener>().ToArray();

        for (int i = 0; i < listeners.Length; i++)
        {
            listeners[i].OnGameStateChanged(temp, state);
        }
    }

    public void StartLevel()
    {
        SetState(GameState.Playing);
    }

    public void NextLevel()
    {
        Level++;

        SceneManager.LoadScene(Level);
    }
}

public enum GameState
{
    Null,
    Initial,
    Playing,
    Success,
    Failed
}