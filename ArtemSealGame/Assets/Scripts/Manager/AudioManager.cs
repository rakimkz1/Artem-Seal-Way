using Cysharp.Threading.Tasks;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource sfxSource;
    private AudioSource bgmSource;

    public void Initialize()
    {
        // SFX
        var sfxObj = new GameObject("SFX Source");
        sfxObj.transform.parent = transform;
        sfxSource = sfxObj.AddComponent<AudioSource>();

        // BGM
        var bgmObj = new GameObject("BGM Source");
        bgmObj.transform.parent = transform;
        bgmSource = bgmObj.AddComponent<AudioSource>();
        bgmSource.loop = true;
    }

    //  SFX

    public void PlaySfx(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlaySfxLoop(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        sfxSource.clip = clip;
        sfxSource.volume = volume;
        sfxSource.loop = true;
        sfxSource.Play();
    }

    public void StopSfxLoop()
    {
        sfxSource.Stop();
        sfxSource.clip = null;
        sfxSource.loop = false;
    }

    //  BGM

    public void PlayBgm(AudioClip clip, float volume = 0.5f)
    {
        if (clip == null) return;
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;

        bgmSource.clip = clip;
        bgmSource.volume = volume;
        bgmSource.Play();
    }

    public void StopBgm()
    {
        bgmSource.Stop();
        bgmSource.clip = null;
    }
    

    public void SetSfxMute(bool isMuted) => sfxSource.mute = isMuted;
    public void SetBgmMute(bool isMuted) => bgmSource.mute = isMuted;
}