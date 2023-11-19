using MoreMountains.Tools;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MMSingleton<GameManager>
{
    [SerializeField] private UnityEvent onRestart;

    public void SaveGameSettings()
    {
        GameSettingsWrapper gameSettings = new GameSettingsWrapper();
        AudioManager.Instance.GetSettings(gameSettings);
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            SaveUserPrefs(gameSettings);
        }
        else
        {
            string json = JsonUtility.ToJson(gameSettings, true);
            File.WriteAllText(_settingsFile, json);
        }
    }

    private readonly string _settingsFile = "settings.txt";

    protected override void Awake()
    {
        base.Awake();
        LoadGameSettings();
    }

    public void LoadGameSettings()
    {
        GameSettingsWrapper gameSettings;
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            gameSettings = LoadUserPrefs();
        }
        else
        {
            gameSettings = LoadSettingsFile();
        }
        AudioManager.Instance.SetSettings(gameSettings);
    }

    private GameSettingsWrapper DefaultSettings()
    {
        GameSettingsWrapper settings = new GameSettingsWrapper();
        settings.MasterVolume = 1.0f;
        settings.MusicVolume = 1.0f;
        return settings;
    }

    private GameSettingsWrapper LoadSettingsFile()
    {
        GameSettingsWrapper settings;
        string file;
        try
        {
            file = File.ReadAllText(_settingsFile);
        }
        catch (Exception e) when (e is IOException ||
                                    e is FileNotFoundException)
        {
            return DefaultSettings();
        }

        try
        {
            settings = JsonUtility.FromJson<GameSettingsWrapper>(file);
        }
        catch
        {
            return DefaultSettings();
        }

        return settings;
    }

    private GameSettingsWrapper LoadUserPrefs()
    {
        GameSettingsWrapper gameSettings = new GameSettingsWrapper();
        gameSettings.MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        gameSettings.MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        return gameSettings;
    }

    private void SaveUserPrefs(GameSettingsWrapper gameSettings)
    {
        PlayerPrefs.SetFloat("MasterVolume", gameSettings.MasterVolume);
        PlayerPrefs.SetFloat("MusicVolume", gameSettings.MusicVolume);
        PlayerPrefs.Save();
    }

    public void LoseGame()
    {
        UIManager.Instance.ShowLose();
    }

    public void WinGame()
    {
        UIManager.Instance.ShowWin();
    }

    public void RestartGame()
    {
        onRestart.Invoke();
    }
}
