using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BaseballWheelGame : MonoBehaviour
{
    public Text resultText;
    public Text inningText;
    public Text scoreText;
    public Text outsText;
    public Text basesText;
    public Button spinButton;

    // 十個可能結果的轉盤選項
    private string[] wheelOptions = new string[]
    {
        "三振出局",     // Strikeout
        "四壞球保送",   // Walk
        "一壘安打",     // Single
        "二壘安打",     // Double
        "三壘安打",     // Triple
        "全壘打",       // Home run
        "飛球接殺",     // Fly out
        "滾地球出局",   // Ground out
        "犧牲打",       // Sacrifice fly
        "雙殺打"        // Double play
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
            case 0: // 三振出局
            case 6: // 飛球接殺
            case 7: // 滾地球出局
                outs++;
                break;
            case 1: // 四壞球保送
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
            case 8: // 犧牲打
                outs++;
                AdvanceRunners(1, false);
                break;
            case 9: // 雙殺打
                outs += 2;
                RemoveLeadRunner();
                break;
        }
        CheckHalfInning();
    }

    void AdvanceRunners(int basesToAdvance, bool batterSafe = true)
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
        if (batterSafe && basesToAdvance >= 1)
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

    void RemoveLeadRunner()
    {
        for (int b = 0; b < 3; b++)
        {
            if (bases[b])
            {
                bases[b] = false;
                break;
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

    string FormatBases()
    {
        string first = bases[0] ? "有人" : "空";
        string second = bases[1] ? "有人" : "空";
        string third = bases[2] ? "有人" : "空";
        return string.Format("一:{0} 二:{1} 三:{2}", first, second, third);
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

        if (outsText != null)
            outsText.text = string.Format("出局數: {0}", outs);

        if (basesText != null)
            basesText.text = FormatBases();
    }
}
