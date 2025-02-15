using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using static UnityEditor.Progress;


public class QuestManager : MonoBehaviour
{
    public List<AdventureData> adventureDatas = new List<AdventureData>(); // 모험가들
    public QuestData questData;     // 의뢰


    private float leftTime;             // 의뢰 완성까지 남은 시간

    private int monsterId;              // 몬스터 Id
    private int monsterStrongSize;      // 몬스터 강점 퍼센트
    private int monsterWeakSize;        // 몬스터 약점 퍼센트
    private int tierScore;              // 모험가 등급 점수

    private string monsterStrong;       // 몬스터 강점
    private string monsterWeak;         // 몬스터 약점

    private float samePositionRate;     // 포지션 중복 비율
    private float sameClassRate;        // 클래스 중복 비율
    private float mixPositionRate;      // 포지션 조합 비율
    private float misClassRate;         // 클래스 조합 비율
    private float weakRate;             // 약점 비율(마이너스 적용)
    private float strongRate;           // 강점 비율
    public void OnClickQusetStart()
    {
        if (DoCheck()) return;

        SetQuest();


        SetSameScore();         // 중복 비율
        SetMixScore();          // 조합 비율
        SetStrongAndWeak();     // 약점 & 강점 비율
        SetTier();              // 등급 점수
    }

    private bool DoCheck()
    {
        if (questData == null || adventureDatas.Count != 4)
        {
            Debug.Log("퀘스트 또는 모험가를 다시 선택 해주세요.");
            return true ;
        }

        return false ;
    }

    private void SetQuest()
    {
        monsterId = questData.questMonsterDescId;    // 몬스터 ID 체크
        
        var monsterData = DataTableManager.Instance.GetMonsterDescData(monsterId);

        monsterWeakSize = Convert.ToInt32(Regex.Replace(monsterData.monsterWeekness, @"\D", ""));
        monsterWeak = monsterData.monsterWeekness.Substring(0, 2);

        monsterStrongSize = Convert.ToInt32(Regex.Replace(monsterData.monsterStrength, @"\D", ""));
        monsterStrong = monsterData.monsterStrength.Substring(0, 2);
    }

    private void SetMixScore()
    {

    }

    private void SetSameScore()
    {
        int frontCount = 0;
        int midCount = 0;
        int backCount = 0;

        int A = 0;  // 공격
        int B = 0;  // 방어
        int C = 0;  // 지원

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

    private void SetStrongAndWeak()
    {
        foreach (var item in adventureDatas)
        {
            if (item.adventureType == monsterWeak)
            {
                weakRate += ((float)0.1 * monsterWeakSize);
            }

            if (item.adventureType == monsterStrong)
            {
                strongRate += ((float)0.1 * monsterStrongSize);
            }
        }
    }

    private void SetTier()
    {
        foreach (var item in adventureDatas)
        {
            string tier = item.adventureTier;
            switch (tier)   // 모험가 등급 점수
            {
                case "브론즈":
                    tierScore += 100;
                    break;
                case "실버":
                    tierScore += 200;
                    break;
                case "골드":
                    tierScore += 300;
                    break;
                case "플래티넘":
                    tierScore += 400;
                    break;
                case "다이아":
                    tierScore += 600;
                    break;
            }
        }
    }

}

