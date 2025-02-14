using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureExportUI : BaseUI
{
    public override void Init(Transform anchor)
    {
        base.Init(anchor);
    }
    public void OnClickExportBtn()
    {
        // PlayerPrefs에 해당 모험가 제거
        // 인피니티 스크롤 업데이트
        Debug.Log("Export!!!");
    }
}
