using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager inst { get { return instance; } }

    [SerializeField] private AudioClip[] bgm_Audio;
    [SerializeField] private AudioClip[] skill_Audio;
    [SerializeField] private AudioClip[] enemy_Audio;


    private AudioSource audioBgmSource;

    private void Awake()
    {
        instance = this;

        audioBgmSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlaySound(SoundType.Bgm ,(int)BgmSound.Bgm_Start);

        SceneManager.sceneLoaded += LoadSceneEvent;
    }

    private void LoadSceneEvent(Scene scene, LoadSceneMode mode)
    {
        if(SceneManager.GetActiveScene().name != "LoadingScene")
        {
            PlaySound(SoundType.Bgm, (int)BgmSound.Bgm_Stage);
        }
    }

    public void PlaySound(SoundType type, int index)
    {
        switch (type)
        {
            case SoundType.Bgm:
                {
                    audioBgmSource.clip = bgm_Audio[index];
                    audioBgmSource.Play();
                    break;
                }
            case SoundType.Skill:
                {
                    AudioSource.PlayClipAtPoint(skill_Audio[index], Vector3.zero);
                    break;
                }
            case SoundType.Enemy:
                {
                    AudioSource.PlayClipAtPoint(enemy_Audio[index], Vector3.zero);
                    break;
                }
        }

        
    }
}
