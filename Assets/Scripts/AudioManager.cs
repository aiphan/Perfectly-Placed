using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;  
    public AudioClip backgroundMusic, toolSound, snapSound, levelCompleteSound;
    private AudioSource musicSource, sfxSource;
    private float originalMusicVolume = 1f, lowVolume = 0.2f;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); return; }

        var sources = GetComponents<AudioSource>();
        musicSource = sources[0];
        sfxSource = sources[1];
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
            originalMusicVolume = musicSource.volume;
        }
    }

    public void PlaySFX(AudioClip clip) { if (clip != null) sfxSource.PlayOneShot(clip); }
    public void PlayToolSound() => PlaySFX(toolSound);
    public void PlaySnapSound() => PlaySFX(snapSound);
    public void PlayLevelCompleteSound() => PlaySFX(levelCompleteSound);


    public void FadeOutBackgroundMusic(float fadeDuration) => StartCoroutine(FadeOutMusicCoroutine(fadeDuration));
    private IEnumerator FadeOutMusicCoroutine(float fadeDuration)
    {
        float startVolume = musicSource.volume;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, lowVolume, t / fadeDuration);
            yield return null;
        }
        musicSource.volume = lowVolume;
    }

    public void RestoreMusicVolume() => musicSource.volume = originalMusicVolume;
}
