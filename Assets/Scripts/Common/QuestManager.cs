using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class QuestManager : MonoBehaviour
{
    public Button questBtn;       // 의뢰 선택 버튼
    public Button adventureBtn;   // 파모험가 선택 버튼

    public RawImage stateIcons;   // 상태 아이콘

    [SerializeField] public int detachIndex;

    const string ICON_PATH = "Arts/Icon";

    private int targetScore;            // 목표 점수
    private int monsterId;              // 몬스터 Id
    private int monsterStrongSize;      // 몬스터 강점 퍼센트
    private int monsterWeakSize;        // 몬스터 약점 퍼센트
    private int tierScore;              // 모험가 등급 점수
    private int resultScore;            // 결과 점수

    private int frontCount = 0;         // 전위 수
    private int midCount = 0;           // 중위 수
    private int backCount = 0;          // 후위 수
    private int A = 0;                  // 공격 수
    private int B = 0;                  // 방어 수
    private int C = 0;                  // 지원 수
    private int resultMaxRate;          // 어떤 확률이 제일 큰지? / 전멸, 성공, 대성공

    private string monsterStrong;       // 몬스터 강점
    private string monsterWeak;         // 몬스터 약점

    private float leftTime;             // 의뢰 완성까지 남은 시간
    private float samePositionRate;     // 포지션 중복 비율
    private float sameClassRate;        // 클래스 중복 비율
    private float mixPositionRate;      // 포지션 조합 비율
    private float misClassRate;         // 클래스 조합 비율
    private float weakRate;             // 약점 비율
    private float strongRate;           // 강점 비율(마이너스 적용)
    private float dieRate = 0;              // 전멸 확률
    private float bigRate = 0;              // 대성공 확률

    public List<AdventureData> adventureDatas = new List<AdventureData>();

    private void Update()
    {
        if (detachIndex > 10 || (questBtn.interactable == false && adventureBtn.interactable == false))
        {
            if(detachIndex < 10)
            {
                if (PoolManager.Instance.checkUpdate[detachIndex - 1]) return;

                PoolManager.Instance.checkUpdate[detachIndex - 1] = true;
            }
            else
            {
                if (!PoolManager.Instance.ready) return;

                if (PoolManager.Instance.checkUpdate[detachIndex - 6]) return;

                PoolManager.Instance.checkUpdate[detachIndex - 6] = true;
            }
            

            SetQuest(detachIndex);

            SetTier(detachIndex);                 // 등급 점수
            SetSameScore(detachIndex);            // 중복 비율
            SetMixScore(detachIndex);             // 조합 비율
            SetStrongAndWeak(detachIndex);        // 약점 & 강점 비율
            Calculation(detachIndex);             // 점수 계산

            if (detachIndex < 10)
            {
                stateIcons.color = new Color(1, 1, 1, 1);

                if (resultMaxRate == -1)
                    stateIcons.texture = Resources.Load("Arts/Icon/IconFaceHard") as Texture2D;
                else if (resultMaxRate == 0)
                    stateIcons.texture = Resources.Load("Arts/Icon/IconFaceNormal") as Texture2D;
                else
                    stateIcons.texture = Resources.Load("Arts/Icon/IconFaceEasy") as Texture2D;
            }
        }
        else
        {
            if (detachIndex < 10)
                PoolManager.Instance.checkUpdate[detachIndex - 1] = false;
            else
                PoolManager.Instance.checkUpdate[detachIndex - 6] = false;

            stateIcons.texture = null;
            stateIcons.color = new Color(0, 0, 0, 0);
        }
    }

    public void OnClickQusetStart(int index)
    {
        if (DoCheck(index)) return;
    }

    private bool DoCheck(int index)
    {
        if (PoolManager.Instance.questData[index] == null)
        {
            Debug.Log("퀘스트를 다시 선택 해주세요.");
            return true ;
        }
        //else if (adventureDatas[index].Count != 4)
        //{
        //    Debug.Log("모험가를 다시 선택 해주세요.");
        //    return true;
        //}

        return false ;
    }

    private void SetQuest(int index)
    {
        monsterId = PoolManager.Instance.questData[index].questMonsterDescId;    // 몬스터 ID 체크
        
        var monsterData = DataTableManager.Instance.GetMonsterData(monsterId);  // 여기는 정상

        if(monsterData.monsterWeekness != "")
        {
            monsterWeakSize = Convert.ToInt32(monsterData.monsterWeekness.Substring(2, 1));
            monsterWeak = monsterData.monsterWeekness.Substring(0, 2);
        }
        
        if(monsterData.monsterStrength != "")
        {
            monsterStrongSize = Convert.ToInt32(monsterData.monsterStrength.Substring(2, 1));
            monsterStrong = monsterData.monsterStrength.Substring(0, 2);
        }
        
        var questLevel = PoolManager.Instance.questData[index].questLevel;

        switch (questLevel)
        {
            case "브론즈":
                targetScore = 400;
                break;
            case "실버":
                targetScore = 900;
                break;
            case "골드":
                targetScore = 1400;
                break;
            case "플래티넘":
                targetScore = 1900;
                break;
            case "다이아":
                targetScore = 2400;
                break;
        }
    }

    private void SetSameScore(int index)
    {
        frontCount = 0;
        backCount = 0;
        midCount = 0;

        A = 0;
        B = 0;
        C = 0;

        samePositionRate = 0;
        sameClassRate = 0;

        foreach (var item in adventureDatas)
        {
            // 포지션
            if(item.adventurePosition == "전위")
            {
                frontCount++;
            }
            else if(item.adventurePosition == "중위")
            {
                midCount++;
            }
            else if (item.adventurePosition == "후위")
            {
                backCount++;
            }

            // 클래스
            if (item.adventureClass == "공격")
            {
                A++;
            }
            else if (item.adventureClass == "방어")
            {
                B++;
            }
            else if( item.adventureClass == "지원")
            {
                C++;
            }
        }

        var posCount = Mathf.Max(frontCount, Mathf.Max(midCount, backCount));
        
        if(posCount == 2)
        {
            samePositionRate = 0.1f;
        }
        else if(posCount == 3)
        {
            samePositionRate = 0.3f;
        }
        else if (posCount == 4)
        {
            samePositionRate = 0.5f;
        }

        var classCount = Mathf.Max(A, Mathf.Max(B, C));

        if (classCount == 2)
        {
            sameClassRate = 0.1f;
        }
        else if (classCount == 3)
        {
            sameClassRate = 0.3f;
        }
        else if (classCount == 4)
        {
            sameClassRate = 0.5f;
        }
    }

    private void SetMixScore(int index)
    {
        mixPositionRate = 0;
        misClassRate = 0;

        if (frontCount == 1 && backCount == 1) mixPositionRate = 0.1f;
        if(frontCount == 1 && midCount == 1 && backCount == 1) mixPositionRate = 0.3f;
        if (frontCount == 1 && midCount == 1 && backCount == 2) mixPositionRate = 0.5f;

        if (A == 1 && B == 1) misClassRate = 0.1f;
        if (A == 1 && B == 1 && C == 1) misClassRate = 0.3f;
        if(A == 2 && B == 1 && C == 1) misClassRate = 0.5f;
    }

    private void SetStrongAndWeak(int index)
    {
        weakRate = 0;
        strongRate = 0;

        foreach (var item in adventureDatas)
        {
            if (monsterWeak != "")
            {
                if (item.adventureType == monsterWeak)
                {
                    weakRate += ((float)0.1 * monsterWeakSize);
                }
            }

            if (monsterStrong != "")
            {
                if (item.adventureType == monsterStrong)
                {
                    strongRate += ((float)0.1 * monsterStrongSize);
                }
            }
        }
    }

    private void SetTier(int index)
    {
        tierScore = 0;

        foreach (var item in adventureDatas) // adventureDatas가 초기화가 안 되어서 이전 모험가도 같이 계산되나?
        {
            string tier = item.adventureTier;
            switch (tier)   // 모험가 등급 점수
            {
                case "브론즈":
                    tierScore += 100;
                    Debug.Log($"{tier}이라서 +100");
                    break;
                case "실버":
                    tierScore += 200;
                    Debug.Log($"{tier}이라서 +200");
                    break;
                case "골드":
                    tierScore += 300;
                    Debug.Log($"{tier}이라서 +300");
                    break;
                case "플래티넘":
                    tierScore += 400;
                    Debug.Log($"{tier}이라서 +400");
                    break;
                case "다이아":
                    tierScore += 600;
                    Debug.Log($"{tier}이라서 +600");
                    break;
            }
        }
    }

    
    public void Calculation(int index)
    {
        dieRate = 0;
        bigRate = 0;

        float rate = samePositionRate + sameClassRate + mixPositionRate + misClassRate - strongRate + weakRate;
        rate = Mathf.Floor(rate * 10f) / 10f;

        int addScore = (int)(tierScore * rate);

        resultScore = 0;
        resultScore = addScore + tierScore;
        Debug.Log($"기본 점수 : {tierScore}");

        Debug.Log($"계산 비율 : {rate}");
        Debug.Log($"추가 점수 : {addScore}");
        Debug.Log($"합계 점수 : {resultScore}");
        Debug.Log($"목표 점수 : {targetScore}");

        if (targetScore < resultScore)   // 점수 오버
        {
            int tmp = resultScore - targetScore;
            bigRate = (tmp / 10) * 0.5f;
        }
        else if(targetScore > resultScore)  // 점수 언더
        {
            int tmp = targetScore - resultScore;
            dieRate = (tmp / 10) * 1f;
        }

        float nomalRate = 100f - (bigRate + dieRate);

        resultMaxRate = bigRate > dieRate ? (bigRate > nomalRate ? 1 : 0) : (dieRate > nomalRate ? -1 : 0);
        Debug.Log($"전멸 : {dieRate} / 성공 : {nomalRate} / 대 : {bigRate}");


        float randomValue = UnityEngine.Random.Range(0f, 100f);
        randomValue = Mathf.Floor(randomValue * 10f) / 10f;

        float saveTmp = dieRate;

        if (saveTmp > randomValue)
        {
            PoolManager.Instance.resultList[index] = -1;     // 전멸
            return;
        }
        saveTmp += nomalRate;

        if (saveTmp > randomValue)
        {
            PoolManager.Instance.resultList[index] = 0;      // 일반 성공
            return;
        }

        PoolManager.Instance.resultList[index] = 1;          // 대성공
    }

}

