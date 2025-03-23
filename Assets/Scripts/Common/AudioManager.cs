using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGM
{
    Start,          // 타이틀(Start씬)
    Main6,          // 길드 내부1
    Main7,          // 길드 내부2
    ScriptDefault,  // 일반 스크립트 상황
    ScriptIntro,    // 0_1 스크립트
    Script0_2,      // 0_2 스크립트
    TypeWriter,     // 타자기 소리
    ED1,            // ED1
    COUNT,
}

public enum SFX
{
    GameStart,      // 게임 시작 버튼
    Denied,         // 골드 부족 같은 거 삐삑!
    LevelUp,        // 레벨 업
    Mark,           // 느낌표 활성화
    QuestFail,      // 의뢰 실패(전멸)
    QuestSuccess,   // 의뢰 완료
    Stamp,          // 정산 보고서 도장 소리
    OpenBook,       // 도감 Open
    CloseBook,      // 도감 Close
    BookFlip3,      // 페이지 넘기는 소리1
    BookFlip4,      // 페이지 넘기는 소리2
    BookFlip5,      // 페이지 넘기는 소리3
    BookFlip7,      // 페이지 넘기는 소리4
    Click1,         // 클릭음1
    Click2,         // 클릭음2
    CoinDrop1,      // 정산1
    CoinDrop2,      // 정산2
    Alarm,          // 작은 알람소리
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

    private float masterVol;
    public float MasterVol { get { return masterVol; } set { masterVol = value; } }
    private float bgmVol;
    public float BgmVol { get { return bgmVol; } set { bgmVol = value; } }
    private float sfxVol;
    public float SfxVol { get { return sfxVol; } set { sfxVol = value; } }

    public float bgmTmp;
    public float sfxTmp;

    protected override void Init()
    {
        base.Init();
        LoadBGMPlayer();
        LoadSFXPlayer();
        masterVol = 0.5f;
        bgmVol = 0.5f;
        sfxVol = 0.5f;
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
            newAudioSource.spatialBlend = 0f;
            newAudioSource.volume = 1f;
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
            newAudioSource.spatialBlend = 0f;
            newAudioSource.volume = 1f;
            newGO.transform.parent = SFXTrs;

            m_SFXPlayer[(SFX)i] = newAudioSource;
        }
    }

    // 기존의 PlayBGM 메서드를 수정하여 BGM 전환 시 fade out 효과를 추가함.
    public void PlayBGM(BGM bgm)
    {
        StartCoroutine(SwitchBGM(bgm, 0.5f));
    }

    // 현재 재생 중인 BGM을 fade out한 후 새로운 BGM을 재생하는 코루틴
    private IEnumerator SwitchBGM(BGM bgm, float fadeTime)
    {
        if (m_CurrBGMSource != null && m_CurrBGMSource.isPlaying)
        {
            yield return StartCoroutine(FadeOut(m_CurrBGMSource, fadeTime));
            m_CurrBGMSource.Stop();
        }

        if (!m_BGMPlayer.ContainsKey(bgm))
        {
            yield break;
        }

        m_CurrBGMSource = m_BGMPlayer[bgm];
        m_CurrBGMSource.Play();
        StartCoroutine(FadeIn(m_CurrBGMSource, masterVol * bgmVol));
    }

    // 지정된 AudioSource의 볼륨을 fadeTime 동안 서서히 줄이는 코루틴
    private IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;
        float elapsed = 0f;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeTime);
            yield return null;
        }
        audioSource.volume = 0f;
    }
    // 지정된 AudioSource의 볼륨을 fadeTime 동안 서서히 키우는 코루틴
    private IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
    {
        float endVolume = audioSource.volume;
        float elapsed = 0f;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, endVolume, elapsed / fadeTime);
            yield return null;
        }
        audioSource.volume = endVolume;
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
        if (m_CurrBGMSource) StartCoroutine(StopBGMFadeOut(0.5f));
    }

    private IEnumerator StopBGMFadeOut(float fadeTime) {
        yield return StartCoroutine(FadeOut(m_CurrBGMSource, fadeTime));
        m_CurrBGMSource.Stop();
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

    public void UpdateVolume()
    {
        float a = masterVol * bgmVol;
        float b = masterVol * sfxVol;
        foreach (var audioSourceItem in m_BGMPlayer)
        {
            audioSourceItem.Value.volume = a;
        }
        foreach (var audioSourceItem in m_SFXPlayer)
        {
            audioSourceItem.Value.volume = b;
        }
    }
}
