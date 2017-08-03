using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
    public static SoundManager instance;

    public AudioClip[] effecSound;
    public AudioClip bgmSound;

    private AudioSource effectAudio;
    [SerializeField]
    private AudioSource bgmAudio;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        effectAudio = this.GetComponent<AudioSource>();
    }

    public void PlayEffectSound(int _num)
    {
        effectAudio.PlayOneShot(effecSound[_num]);
    }
 
    public void PlayBGMSound()
    {
        bgmAudio.PlayOneShot(bgmSound);
    }

    public void StopBGMSound()
    {
        bgmAudio.Stop();
    }

}
