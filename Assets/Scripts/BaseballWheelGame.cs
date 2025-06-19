using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BaseballWheelGame : MonoBehaviour
{
    public Text resultText;
    public Text inningText;
    public Text scoreText;
    public Button spinButton;

    // 十個可能結果的轉盤選項
    private string[] wheelOptions = new string[]
    {
        "三振",       // 三振
        "四壞",       // 四壞球
        "一壘安打", // 一壘安打
        "二壘安打", // 二壘安打
        "三壘安打", // 三壘安打
        "全壘打",   // 全壘打
        "滾地出局", // 滾地出局
        "接殺出局", // 接殺出局
        "觸身球",   // 觸身球
        "失誤上壘"  // 失誤上壘
    };

    private bool topInning = true;
    private int inning = 1;
    private int outs = 0;
    private int offenseScore = 0;
    private int defenseScore = 0;
    private List<bool> bases = new List<bool> { false, false, false };

    void Start()
    {
        if (spinButton != null)
        {
            spinButton.onClick.AddListener(Spin);
        }
        UpdateUI(string.Empty);
    }

    void Spin()
    {
        int index = Random.Range(0, wheelOptions.Length);
        string result = wheelOptions[index];
        ApplyResult(index);
        UpdateUI(result);
    }

    void ApplyResult(int index)
    {
        switch (index)
        {
            case 0: // 三振
            case 6: // 滾地出局
            case 7: // 接殺出局
                outs++;
                break;
            case 1: // 四壞球
            case 8: // 觸身球
            case 9: // 失誤上壘
            case 2: // 一壘安打
                AdvanceRunners(1);
                break;
            case 3: // 二壘安打
                AdvanceRunners(2);
                break;
            case 4: // 三壘安打
                AdvanceRunners(3);
                break;
            case 5: // 全壘打
                int runners = CountRunners();
                AddRuns(runners + 1);
                ClearBases();
                break;
        }
        CheckHalfInning();
    }

    void AdvanceRunners(int basesToAdvance)
    {
        for (int b = 2; b >= 0; b--)
        {
            if (bases[b])
            {
                int newIndex = b + basesToAdvance;
                if (newIndex >= 3)
                {
                    AddRuns(1);
                }
                else
                {
                    bases[newIndex] = true;
                }
                bases[b] = false;
            }
        }
        if (basesToAdvance >= 1)
        {
            int newIndex = basesToAdvance - 1;
            if (newIndex >= 3)
            {
                AddRuns(1);
            }
            else
            {
                bases[newIndex] = true;
            }
        }
    }

    int CountRunners()
    {
        int count = 0;
        foreach (bool b in bases) if (b) count++;
        return count;
    }

    void ClearBases()
    {
        for (int i = 0; i < 3; i++) bases[i] = false;
    }

    void AddRuns(int r)
    {
        if (topInning)
            offenseScore += r;
        else
            defenseScore += r;
    }

    void CheckHalfInning()
    {
        if (outs >= 3)
        {
            outs = 0;
            ClearBases();
            topInning = !topInning;
            if (topInning)
                inning++;
        }
    }

    void UpdateUI(string lastResult)
    {
        if (inningText != null)
            inningText.text = string.Format("{0}局{1}", inning, topInning ? "上" : "下");

        if (resultText != null)
            resultText.text = lastResult;

        if (scoreText != null)
            scoreText.text = string.Format("攻 {0} - 守 {1}", offenseScore, defenseScore);
    }
}
