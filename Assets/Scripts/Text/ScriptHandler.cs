using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptHandler : MonoBehaviour
{
    public static ScriptHandler scriptHandler;
    public Button endingTrigger;
    [SerializeField] private ScriptPlayer scriptPlayer;
    private List<int> toStoryDay = new List<int> { 1, 5, 10, 15 };
    private int storyDayIndex = 0;

    private void Awake()
    {
        scriptHandler = this;
    }
    public bool ScriptPlayDay(int day)
    {
        switch (day)
        {
            case 5:
                PlayScript(100101, 100121);
                return true;
            case 10:
                PlayScript(100201, 100219);
                return true;
            case 15:
                PlayScript(100301, 100315);
                return true;
        }
        return false;
    }
    public void ScriptPlayQuestID(int q_id)
    {
        switch (q_id)
        {
            case 132901: // 1챕터
                PlayScript(100151, 100171);
                break;
            case 133902: // 2챕터
                PlayScript(100251, 100260);
                break;
            case 139999: // 3챕터
                PlayScript(100351, 100377);
                break;
        }
    }
    public void PlayScript(int s, int e)
    {
        scriptPlayer.PrepareScriptText(s, e);
        scriptPlayer.ShowNextScript();
        scriptPlayer.isScriptMode = true;
    }
    public void AfterEndTheScript(int id)
    {
        switch (id)
        {
            case 100017:
                storyDayIndex++;
                AudioManager.Instance.PlayBGM(BGM.Main6);
                PlayScript(100018, 100036);
                break;
            case 100036:
                // 튜토리얼 실행
                // PoolManager.Instance.isNotTutorialTouch = true;
                // 튜토리얼 실행 함수
                AudioManager.Instance.PlayBGM(BGM.Main6);
                break;
            case 100121: // 슬라임 홍수 시작
                var slimeUiData = new EmergencyQuestUIData(11);
                UIManager.Instance.OpenUI<EmergencyQuestUI>(slimeUiData);
                AudioManager.Instance.PlayBGM(BGM.EmergencyQuest);
                break;
            case 100171: // 슬라임 홍수 성공
                storyDayIndex++;
                AudioManager.Instance.PlayBGM(BGM.Main6);
                break;
            case 100219: // 몬스터 웨이브 시작
                var monsterWaveUiData = new EmergencyQuestUIData(12);
                UIManager.Instance.OpenUI<EmergencyQuestUI>(monsterWaveUiData);
                AudioManager.Instance.PlayBGM(BGM.EmergencyQuest);
                break;
            case 100260:
                storyDayIndex++;
                AudioManager.Instance.PlayBGM(BGM.Main6);
                break;
            case 100315: // 모험가 구함 시작
                var uiData = new EmergencyQuestUIData(13);
                UIManager.Instance.OpenUI<EmergencyQuestUI>(uiData);
                AudioManager.Instance.PlayBGM(BGM.EmergencyQuest);
                break;
            case 100377: // 엔딩 스크립트 실행
                PlayScript(100379, 100379);
                PlayScript(109101, 109124);
                break;
            case 109124: // 엔딩 실행
                Fade.Instance.DoFade(Color.black, 0f, 1f, 1f, 0f, false);
                ShowEndingIll();
                break;
        }
    }
    public int GetToStoryDay()
    {
        return toStoryDay[storyDayIndex];
    }
    public void PlayIntroScript()
    {
        PlayScript(100001, 100017);
    }


    public void ShowEndingIll()
    {
        scriptPlayer.SetIllWithoutScript(Resources.Load<Sprite>($"Arts/Illustration/ED1"));
        Fade.Instance.DoFade(Color.black, 1f, 0f, 1f, 0f, true, () =>
        {
            endingTrigger.gameObject.SetActive(true);
        });
    }
}
