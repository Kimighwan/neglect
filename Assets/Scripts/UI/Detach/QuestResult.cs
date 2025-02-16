using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestResult : BaseUI
{
    public TextMeshProUGUI txt;
    public GameObject receiptBtn;

    public Dictionary<int, int> list = new Dictionary<int, int>();  // 파견 Index에 따른 결과 저장


    private void Start()
    {
        receiptBtn.SetActive(false);
        StartCoroutine(UpdateResultCo());
    }

    public override void Init(Transform anchor)
    {
        base.Init(anchor);

        var rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(800f, 450f);
    }

    public void OnClickReceiptBtn()
    {
        // 골드 추가하고
        // 모험가 다시 풀기

        UIManager.Instance.CloseUI(this);
    }

    private IEnumerator UpdateResultCo()
    {
        txt.text = "의뢰 확인 중...";

        yield return new WaitForSeconds(2f);

        txt.text = "의뢰 성공";
        receiptBtn.SetActive(true);
    }
}
