using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace World
{
    public class WarningManager : MonoBehaviour
    {
        public List<Warning> warnings = new List<Warning>{new Warning()};

        public enum WarningTypes
        {
            SunProximity,
            PlanetVelocityProximity,
        }
        
        public enum CheckType
        {
            And,
            Or,
        }
        
        private PlayerManager.PlayerData pd;
        private void Start()
        {
            Universe.loop += checkWarnings;
            pd = Universe.instance.playerManager.currentPlayer;
        }
        
        public void checkWarnings()
        {
            foreach (Warning warning in warnings)
            {
                switch (warning.type)
                {
                    case WarningTypes.SunProximity: warning.level = checkThreshold(warning, pd.quantitativeData.distanceToSun);
                        break;
                    case WarningTypes.PlanetVelocityProximity:
                        break;
                }
            }
        }

        public int checkThreshold(Warning warning, float value)
        {
            for (int i = 0; i < warning.thresholds.Count; i++)
            {
                if (value < warning.thresholds[i])
                {
                    return i;
                }
            }

            return -1;
        }
        
        public void checkThreshold(Warning warning, List<float> values)
        {
            
        }
        

        [Serializable]
        public class Warning
        {
            public float level;
            public WarningTypes type;
            public CheckType checkType;

            public List<float> thresholds = new List<float>();
            public List<List<float>> comparisonThresholds = new List<List<float>>();
        }
    }
}
