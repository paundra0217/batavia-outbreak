using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

enum AudioType
{
    BGM,
    SFX,
    VCL
}

[Serializable]
class AudioObject
{
    [Header("Audio Information")]
    [Tooltip("Audio name that later will be played using PlayBGM(), PlaySFX(), or PlayVoiceLine() function, depend on the audio type")]
    public string AudioName;

    [Tooltip("The audio clip itself, audio file")]
    public AudioClip AudioClip;

    [Header("Audio Manipulation")]
    [Tooltip("The pitch of the audio")]
    [Range(-3f, 3f)]
    public float AudioPitch = 1f;

    [Tooltip("The volume of the audio")]
    [Range(0f, 1f)]
    public float AudioVolume = 1f;

    [Header("Audio Status")]
    [Tooltip("If enabled, the audio will loop until it stopped manually")]
    public bool Looping;

    [Tooltip("If enabled, the audio will be played when the scene is loaded")]
    public bool PlayOnAwake;

    [HideInInspector] public AudioSource Source;
}

[Serializable]
class SceneBGMObject
{
    public string sceneName;
    public string BGMName;
}

public class AudioController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Mixer for applied effects, currently pause")]
    private AudioMixer mixer;

    [SerializeField]
    [Tooltip("Target group for BGM to Audio Mixer")]
    private AudioMixerGroup BGMGroup;

    [SerializeField]
    [Tooltip("Target group for SFX to Audio Mixer")]
    private AudioMixerGroup SFXGroup;

    [SerializeField]
    [Tooltip("Target group for voice lines to Audio Mixer")]
    private AudioMixerGroup VCLGroup;

    [SerializeField]
    [Tooltip("List of BGM that will be played")]
    private AudioObject[] audioBGM;

    [SerializeField]
    [Tooltip("List of SFX that will be played")]
    private AudioObject[] audioSFX;

    [SerializeField]
    [Tooltip("List of voice lines that will be played")]
    private AudioObject[] audioVCL;

    [SerializeField]
    [Tooltip("List of scene and BGM name where when a specific scene loaded, specific BGM played.")]
    private SceneBGMObject[] sceneBGMList;

    private AudioSource currentBGMPlaying;

    private Queue<AudioSource> currentSFXesPlaying = new Queue<AudioSource>();
    private Queue<AudioSource> currentVCLsPlaying = new Queue<AudioSource>();

    private static AudioController _instance;
    public static AudioController Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogWarning("Audio Controller is null");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        DontDestroyOnLoad(gameObject);

        SceneManager.activeSceneChanged += PlayBGMOnSceneChanged;

        InitializeAudios();
    }

    private void InitializeAudios()
    {
        foreach (var a in audioBGM)
        {
            AudioSource aSource = gameObject.AddComponent<AudioSource>();

            aSource.clip = a.AudioClip;
            aSource.loop = a.Looping;
            aSource.pitch = a.AudioPitch;
            aSource.volume = a.AudioVolume;
            aSource.playOnAwake = a.PlayOnAwake;
            aSource.outputAudioMixerGroup = BGMGroup;

            a.Source = aSource;

            if (aSource.playOnAwake)
                aSource.Play();
            else
                aSource.Stop();
        }

        foreach (var a in audioSFX)
        {
            AudioSource aSource = gameObject.AddComponent<AudioSource>();

            aSource.clip = a.AudioClip;
            aSource.loop = a.Looping;
            aSource.pitch = a.AudioPitch;
            aSource.volume = a.AudioVolume;
            aSource.playOnAwake = a.PlayOnAwake;
            aSource.outputAudioMixerGroup = SFXGroup;

            a.Source = aSource;
        }

        foreach (var a in audioVCL)
        {
            AudioSource aSource = gameObject.AddComponent<AudioSource>();

            aSource.clip = a.AudioClip;
            aSource.loop = a.Looping;
            aSource.pitch = a.AudioPitch;
            aSource.volume = a.AudioVolume;
            aSource.playOnAwake = a.PlayOnAwake;
            aSource.outputAudioMixerGroup = VCLGroup;

            a.Source = aSource;
        }
    }

    private AudioObject SearchAudio(string audioName, AudioType type)
    {
        AudioObject audio = null;

        switch (type)
        {
            case AudioType.BGM:
                audio = audioBGM.FirstOrDefault(s => s.AudioName == audioName);
                break;

            case AudioType.SFX:
                audio = audioSFX.FirstOrDefault(s => s.AudioName == audioName);
                break;

            case AudioType.VCL:
                audio = audioVCL.FirstOrDefault(s => s.AudioName == audioName);
                break;
        }

        if (audio == null)
        {
            Debug.LogError("Audio is not available or you're searching in the wrong type.");
            return null;
        }

        return audio;
    }

    private void PlayBGMOnSceneChanged(Scene current, Scene next)
    {
        try
        {
            var audioName = sceneBGMList.FirstOrDefault(s => s.sceneName == next.name).BGMName;
            PlayBGM(audioName);
        }
        catch
        {
            Debug.LogWarning("Audio cannot be played because either Scene does not exist on the variable.");
        }
    }

    // - Description: Plays a background music, when a background music is currently playing and this function called
    //                the currently playing background music will be stopped and the new one will be played
    // - Params:
    //      - string audioName: The BGM that will be played
    // - Returns: none
    public void PlayBGM(string audioName)
    {
        var audio = SearchAudio(audioName, AudioType.BGM);
        if (audio == null)
            return;

        foreach (var a in audioBGM)
        {
            a.Source.Stop();
        }

        currentBGMPlaying = audio.Source;
        audio.Source.Play();
    }

    // - Description: Plays an SFX once or loop, depend on the audio settings in the inspector,
    //                can play multiple SFXes at once.
    // - Params:
    //      - string audioName: The SFX that will be played
    // - Returns: none
    public void PlaySFX(string audioName)
    {
        var audio = SearchAudio(audioName, AudioType.SFX);
        if (audio == null)
            return;

        // Kenapa ada ini??S
        // StopBGM();
        // currentBGMPlaying = audio.Source;

        if (audio.Looping)
            audio.Source.Play();
        else
            audio.Source.PlayOneShot(audio.AudioClip);
    }

    // - Description: Plays a voice line, when a voice line is playing and this function called, the currently playing
    //                voice line will be stopped and the new one will be played.
    // - Params:
    //      - string voiceLineName: The voice line that will be played
    // - Returns: none
    public void PlayVoiceLine(string voiceLineName)
    {
        var audio = SearchAudio(voiceLineName, AudioType.VCL);
        if (audio == null)
            return;

        StopVoiceLines();

        audio.Source.Play();
    }

    // - Description: Stop the currently playing BGM.
    // - Params: none
    // - Returns: none
    public void StopBGM()
    {
        foreach (var a in audioBGM)
        {
            a.Source.Stop();
        }
    }

    // - Description: Stops a specified SFX.
    // - Params:
    //      - string audioName: The SFX that will be stopped playing
    // - Returns: none
    public void StopSFX(string audioName)
    {
        var audio = SearchAudio(audioName, AudioType.SFX);
        if (audio == null)
            return;

        audio.Source.Stop();
    }

    // - Description: Stop all SFX.
    // - Params: none
    // - Returns: none
    public void StopAllSFX()
    {
        foreach (var a in audioSFX)
        {
            a.Source.Stop();
        }
    }

    // - Description: Stop a currently playing voice line.
    // - Params: none
    // - Returns: none
    public void StopVoiceLines()
    {
        foreach (var a in audioVCL)
        {
            a.Source.Stop();
        }
    }

    public void PauseAllSound()
    {
        currentBGMPlaying.Pause();

        foreach (var a in audioSFX)
        {
            if (a.Source.isPlaying)
            {
                a.Source.Pause();
                currentSFXesPlaying.Append(a.Source);
            }
        }

        foreach (var a in audioVCL)
        {
            if (a.Source.isPlaying)
            {
                a.Source.Pause();
                currentVCLsPlaying.Append(a.Source);
            }
        }
    }

    public void ResumeAllSound()
    {
        currentBGMPlaying.UnPause();

        while (currentSFXesPlaying.Count > 0)
        {
            currentSFXesPlaying.Dequeue().UnPause();
        }

        while (currentVCLsPlaying.Count > 0)
        {
            currentVCLsPlaying.Dequeue().UnPause();
        }
    }
}
