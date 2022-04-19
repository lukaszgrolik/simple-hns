using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAgentLeveling
{
    public readonly Dictionary<int, int> agentLevelsIndividual = new Dictionary<int, int>();
    public readonly Dictionary<int, int> agentLevelsCombined = new Dictionary<int, int>();
    public readonly List<int> expThresholdsCombined = new List<int>(); // starting from 2nd level

    public readonly int MAX_LEVEL = 99;

    public GameAgentLeveling()
    {
        int levelReqExp = 1000;
        int levelReqTotal = levelReqExp;
        agentLevelsIndividual.Add(2, levelReqExp);
        agentLevelsCombined.Add(2, levelReqExp);
        expThresholdsCombined.Add(levelReqExp);

        for (int i = 3; i <= MAX_LEVEL; i++)
        {
            levelReqExp = Mathf.RoundToInt(levelReqExp * 1.25f);
            levelReqTotal += levelReqExp;

            agentLevelsIndividual.Add(i, levelReqExp);
            agentLevelsCombined.Add(i, levelReqTotal);
            expThresholdsCombined.Add(levelReqTotal);
        }
    }

    public int GetLevelRequiredExpIndividual(int level)
    {
        if (level == 1) return 0;

        return agentLevelsIndividual[level];
    }

    public int GetLevelRequiredExpCombined(int level)
    {
        if (level == 1) return 0;

        return agentLevelsCombined[level];
    }

    public int GetCurrentLevelByExp(int exp)
    {
        for (int i = 0; i < expThresholdsCombined.Count; i++)
        {
            if (exp < expThresholdsCombined[i])
            {
                return i + 1;
            }
        }

        return MAX_LEVEL;
    }
}