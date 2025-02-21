using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGM
{
    Start,
    Main6,
    Main7,
    COUNT
}

public enum SFX
{
    LevelUp,
    Mark,           // 느낌표 활성화
    QuestFail,
    QuestSuccess,
    Stamp,          // 정산 보고서 도장 소리
    OpenBook,       // 도감 Open
    CloseBook,      // 도감 Close
    BookFlip3,
    BookFlip4,
    BookFlip5,
    BookFlip7,
    COUNT
}

public class AudioManager : SingletonBehaviour<AudioManager>
{
    public Transform BGMTrs;    // BGM AudioSource의 위치
    public Transform SFXTrs;    // SFX AudioSource의 위치

    private const string AUDIO_PATH = "Audio";  // Rescources 폴더 하위 경로

    // BGM AudioSource 컨테이너
    private Dictionary<BGM, AudioSource> m_BGMPlayer = new Dictionary<BGM, AudioSource>();
    private AudioSource m_CurrBGMSource;    // 현재 BGM AudioSource

    // SFX AudioSource 컨테이너
    private Dictionary<SFX, AudioSource> m_SFXPlayer = new Dictionary<SFX, AudioSource>();

    protected override void Init()
    {
        base.Init();

        LoadBGMPlayer();
        LoadSFXPlayer();
    }

    private void LoadBGMPlayer()    // 모든 BGM 불러와서 컨테이너에 저장
    {
        for (int i = 0; i < (int)BGM.COUNT; i++)
        {
            var audioName = ((BGM)i).ToString();
            var pathStr = $"{AUDIO_PATH}/{audioName}";
            var audioClip = Resources.Load(pathStr, typeof(AudioClip)) as AudioClip;
            if (!audioClip)
            {
                continue;
            }

            var newGO = new GameObject(audioName);
            var newAudioSource = newGO.AddComponent<AudioSource>();
            newAudioSource.clip = audioClip;
            newAudioSource.loop = true;
            newAudioSource.playOnAwake = false;
            newGO.transform.parent = BGMTrs;

            m_BGMPlayer[(BGM)i] = newAudioSource;
        }
    }

    private void LoadSFXPlayer()    // 모든 SFX 불러와서 컨테이너에 저장
    {
        for (int i = 0; i < (int)SFX.COUNT; i++)
        {
            var audioName = ((SFX)i).ToString();
            var pathStr = $"{AUDIO_PATH}/{audioName}";
            var audioClip = Resources.Load(pathStr, typeof(AudioClip)) as AudioClip;
            if (!audioClip)
            {
                continue;
            }

            var newGO = new GameObject(audioName);
            var newAudioSource = newGO.AddComponent<AudioSource>();
            newAudioSource.clip = audioClip;
            newAudioSource.loop = false;
            newAudioSource.playOnAwake = false;
            newGO.transform.parent = SFXTrs;

            m_SFXPlayer[(SFX)i] = newAudioSource;
        }
    }

    public void PlayBGM(BGM bgm)
    {
        if (m_CurrBGMSource)
        {
            m_CurrBGMSource.Stop();
            m_CurrBGMSource = null;
        }

        if (!m_BGMPlayer.ContainsKey(bgm))
        {
            return;
        }

        m_CurrBGMSource = m_BGMPlayer[bgm];
        m_CurrBGMSource.Play();
    }

    public void PauseBGM()  // 일시정지
    {
        if (m_CurrBGMSource) m_CurrBGMSource.Pause();
    }

    public void ResumeBGM() // 일시정지 해제
    {
        if (m_CurrBGMSource) m_CurrBGMSource.UnPause();
    }

    public void StopBGM()
    {
        if (m_CurrBGMSource) m_CurrBGMSource.Stop();
    }

    public void PlaySFX(SFX sfx)
    {
        if (!m_SFXPlayer.ContainsKey(sfx))
        {
            return;
        }

        m_SFXPlayer[sfx].Play();
    }

    public void Mute()  // 소리 끄기
    {
        foreach (var audioSourceItem in m_BGMPlayer)
        {
            audioSourceItem.Value.volume = 0f;
        }

        foreach (var audioSourceItem in m_SFXPlayer)
        {
            audioSourceItem.Value.volume = 0f;
        }
    }

    public void UnMute()    // 소리 켜기
    {
        foreach (var audioSourceItem in m_BGMPlayer)
        {
            audioSourceItem.Value.volume = 1f;
        }

        foreach (var audioSourceItem in m_SFXPlayer)
        {
            audioSourceItem.Value.volume = 1f;
        }
    }
}
