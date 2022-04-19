using UnityEngine;

namespace GameCore
{
    public class AgentCombatLevel
    {
        private AgentCombat agentCombat;
        private AgentLevel agentLevel;

        public AgentCombatLevel(
            AgentCombat agentCombat,
            AgentLevel agentLevel
        )
        {
            this.agentCombat = agentCombat;
            this.agentLevel = agentLevel;

            agentCombat.enemyKilled += OnEnemyKilled;
        }

        void OnEnemyKilled(Agent agent)
        {
            // @todo magic number
            agentLevel.GainExp(300);
        }
    }

    public class AgentLevel : AgentComponent
    {
        private int currentExp;
        public int CurrentExp => currentExp;

        private int currentLevel;
        public int CurrentLevel => currentLevel;

        public int NextLevel => currentLevel + 1;
        public bool IsMaxLevel => currentLevel == agent.game.agentLeveling.MAX_LEVEL;

        // public int NextLevelRequiredExp {
        //     get {
        //         if (IsMaxLevel) return 0;

        //         return agent.game.agentLeveling.GetLevelRequiredExpCombined(NextLevel);
        //     }
        // }

        public float CurrentLevelExpProgress {
            get {
                if (IsMaxLevel) return 0;

                var currentLevelExp = currentExp - agent.game.agentLeveling.GetLevelRequiredExpCombined(currentLevel);

                return (float)currentLevelExp / agent.game.agentLeveling.GetLevelRequiredExpIndividual(NextLevel);
            }
        }

        public event System.Action<Agent> expChanged;
        public event System.Action<Agent> levelledUp;

        public AgentLevel(
            int currentExp = 0,
            int currentLevel = 1
        )
        {
            this.currentExp = currentExp;
            this.currentLevel = currentLevel;
        }

        public void GainExp(int expPoints)
        {
            currentExp += expPoints;

            expChanged?.Invoke(agent);

            var levelByExp = agent.game.agentLeveling.GetCurrentLevelByExp(currentExp);

            if (levelByExp > currentLevel)
            {
                LevelUp(levelByExp);
            }
        }

        void LevelUp(int newLevel)
        {
            currentLevel = newLevel;

            levelledUp?.Invoke(agent);
        }
    }
}