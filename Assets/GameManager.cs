using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameState gameState;

    private int level;

    private int levelCount;

    public GameState GameState
    {
        get => gameState;

        private set
        {
            if (gameState != value)
            {
                gameState = value;
            }
        }
    }

    public int Level
    {
        get => level;

        private set
        {
            level = (value % levelCount);
        }
    }

    private void Awake()
    {
        Instance = this;

        levelCount = SceneManager.sceneCountInBuildSettings;
    }

    public void SetState(GameState state)
    {
        GameState = state;
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