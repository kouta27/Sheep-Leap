using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[DefaultExecutionOrder(-100)]
public class AudioManager : MonoBehaviour
{
    // ===== Singleton =====
    public static AudioManager Instance { get; private set; }

    // ===== Data mapping (enum -> AudioClip) =====
    [Serializable]
    public struct SoundEntry
    {
        public SoundId id;
        public AudioClip clip;
    }

    [Header("Clip Mapping (enum -> AudioClip)")]
    [Tooltip("Register enum-to-clip mapping here.")]
    public SoundEntry[] entries;

    // Built dictionary for fast lookup
    private Dictionary<SoundId, AudioClip> _clips;

    // ===== Music (BGM) =====
    [Header("Music")]
    [Tooltip("Optional: Route to a mixer group.")]
    public AudioMixerGroup musicMixerGroup;
    [Range(0f, 1f)] public float musicVolume = 1.0f;

    private AudioSource _musicSource;
    private Coroutine _musicFadeCo;

    // ===== SFX (one-shot with pool) =====
    [Header("SFX")]
    [Tooltip("Optional: Route to a mixer group.")]
    public AudioMixerGroup sfxMixerGroup;
    [Range(0f, 1f)] public float sfxVolume = 1.0f;
    [Min(1)] public int sfxPoolSize = 12;

    private AudioSource[] _sfxPool;
    private int _sfxIndex;

    // ===== Lifetime =====
    private void Awake()
    {
        // Singleton boot
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        BuildClipDictionary();
        BuildMusicSource();
        BuildSfxPool();
    }

    private void BuildClipDictionary()
    {
        _clips = new Dictionary<SoundId, AudioClip>(entries.Length);
        foreach (var e in entries)
        {
            if (e.id == SoundId.None || e.clip == null) continue;
            if (_clips.ContainsKey(e.id)) continue; // ignore duplicated ids
            _clips.Add(e.id, e.clip);
        }
    }

    private void BuildMusicSource()
    {
        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.loop = true;
        _musicSource.playOnAwake = false;
        _musicSource.outputAudioMixerGroup = musicMixerGroup;
        _musicSource.volume = musicVolume;
    }

    private void BuildSfxPool()
    {
        _sfxPool = new AudioSource[sfxPoolSize];
        for (int i = 0; i < sfxPoolSize; i++)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.loop = false;
            src.outputAudioMixerGroup = sfxMixerGroup;
            src.volume = sfxVolume;
            _sfxPool[i] = src;
        }
    }

    // ===== Public API =====

    /// <summary>
    /// Play a one-shot SFX by enum id.
    /// </summary>
    public void PlaySFX(SoundId id, float volume = 1f, float pitch = 1f)
    {
        if (!_clips.TryGetValue(id, out var clip) || clip == null) return;

        var src = _sfxPool[_sfxIndex];
        _sfxIndex = (_sfxIndex + 1) % _sfxPool.Length;

        src.Stop();
        src.clip = clip;
        src.pitch = pitch;
        src.volume = sfxVolume * Mathf.Clamp01(volume);
        src.Play();
    }

    /// <summary>
    /// Play music (BGM) by enum id. Optional fade (seconds).
    /// </summary>
    public void PlayMusic(SoundId id, bool loop = true, float fadeSeconds = 0f)
    {
        if (!_clips.TryGetValue(id, out var clip) || clip == null) return;

        _musicSource.loop = loop;

        if (fadeSeconds > 0f)
        {
            if (_musicFadeCo != null) StopCoroutine(_musicFadeCo);
            _musicFadeCo = StartCoroutine(FadeToMusic(clip, fadeSeconds));
        }
        else
        {
            _musicSource.clip = clip;
            _musicSource.volume = musicVolume;
            _musicSource.Play();
        }
    }

    /// <summary>
    /// Stop music (optional fade-out).
    /// </summary>
    public void StopMusic(float fadeSeconds = 0f)
    {
        if (!_musicSource.isPlaying) return;

        if (fadeSeconds > 0f)
        {
            if (_musicFadeCo != null) StopCoroutine(_musicFadeCo);
            _musicFadeCo = StartCoroutine(FadeOutMusic(fadeSeconds));
        }
        else
        {
            _musicSource.Stop();
            _musicSource.clip = null;
        }
    }

    /// <summary>
    /// Change global volumes at runtime.
    /// </summary>
    public void SetVolumes(float newMusicVolume, float newSfxVolume)
    {
        musicVolume = Mathf.Clamp01(newMusicVolume);
        sfxVolume = Mathf.Clamp01(newSfxVolume);

        if (_musicSource != null)
            _musicSource.volume = musicVolume;
        if (_sfxPool != null)
            foreach (var s in _sfxPool) s.volume = sfxVolume;
    }

    // ===== Coroutines =====
    private System.Collections.IEnumerator FadeToMusic(AudioClip next, float seconds)
    {
        float t = 0f;
        float startVol = _musicSource.isPlaying ? _musicSource.volume : 0f;

        // Fade out current
        while (t < seconds)
        {
            t += Time.unscaledDeltaTime;
            _musicSource.volume = Mathf.Lerp(startVol, 0f, t / seconds);
            yield return null;
        }

        _musicSource.Stop();
        _musicSource.clip = next;
        _musicSource.Play();

        // Fade in
        t = 0f;
        while (t < seconds)
        {
            t += Time.unscaledDeltaTime;
            _musicSource.volume = Mathf.Lerp(0f, musicVolume, t / seconds);
            yield return null;
        }
        _musicSource.volume = musicVolume;
        _musicFadeCo = null;
    }

    private System.Collections.IEnumerator FadeOutMusic(float seconds)
    {
        float t = 0f;
        float startVol = _musicSource.volume;
        while (t < seconds)
        {
            t += Time.unscaledDeltaTime;
            _musicSource.volume = Mathf.Lerp(startVol, 0f, t / seconds);
            yield return null;
        }
        _musicSource.Stop();
        _musicSource.clip = null;
        _musicFadeCo = null;
    }
}

public enum SoundId
{
    None = 0,

    // UI
    UI_Click,
    UI_Open,
    UI_Close,

    // Gameplay SFX
    Player_Jump,
    Enemy_Hit,
    Coin_Get,

    // Music
    BGM_Title,
    BGM_Stage1,
}
