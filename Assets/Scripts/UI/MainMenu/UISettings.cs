using System;
using System.IO;
using UnityEngine;

[Serializable]
public class SettingsStorage
{
    public int displayMode;
    public int resolution;

    public float masterVolume;
    public float BGMVolume;
    public float SFXVolume;

    public int quality;
    public int maxFPS;
}

public class UISettings : MonoBehaviour
{
    [SerializeField] private SOSettings defaultSettings;
    [SerializeField] private SOSettings userSettings;

    private static UISettings _instance;

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadSettingsFromJson()
    {
        try
        {
            var userSettingsJson = File.ReadAllText(Application.persistentDataPath + "/UserSettings.json");
            var userSettingsObject = JsonUtility.FromJson<SettingsStorage>(userSettingsJson);

            // Video Settings
            userSettings.displayMode = userSettingsObject.displayMode;
            userSettings.resolution = userSettingsObject.resolution;

            // Audio Settings
            userSettings.masterVolume = userSettingsObject.masterVolume;
            userSettings.BGMVolume = userSettingsObject.BGMVolume;
            userSettings.SFXVolume = userSettingsObject.SFXVolume;

            // Graphics Settings
            userSettings.quality = userSettingsObject.quality;
            userSettings.maxFPS = userSettingsObject.maxFPS;
        }
        catch
        {
            userSettings = defaultSettings;
            File.WriteAllText(Application.persistentDataPath + "/UserSettings.json", JsonUtility.ToJson(userSettings));
        }
    }
}
