using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptHandler : MonoBehaviour
{
    public static ScriptHandler scriptHandler;
    [SerializeField] private ScriptPlayer scriptPlayer;
    private GameInfo info;

    private void Awake()
    {
        scriptHandler = this;
    }
    private void Start()
    {
        info = GameInfo.gameInfo;
    }
    public void ConditionalScriptPlay(int q_id)
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
    public void EndingScriptPlay(int s, int e)
    {
        PlayScript(s, e);
    }

    public void PlayScript(int s, int e) {
        scriptPlayer.PrepareScriptText(s, e);
        scriptPlayer.ShowNextScript();
        scriptPlayer.isScriptMode = true;
    }
    
    // public int GetToStoryDay()
    // {

    // }
}
