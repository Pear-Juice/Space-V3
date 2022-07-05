using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace World
{
    public class Universe : MonoBehaviour
    {
        public static Universe instance;
        public PlayerManager playerManager;
    
        public const float tickRate = .5f;
        public static int tick;
    
        public List<SolarSystem> systems = new List<SolarSystem>();

        private void Awake()
        {
            instance = this;
            StartCoroutine(nameof(mainLoop));
            loop += updateDistances;
        }

        public void updateDistances()
        {
            foreach (PlayerManager.PlayerData pd in playerManager.playersInWorld)
            {
                if (!pd.components.gameObject)
                    continue;
                updateSolarSystemDistance(pd);
                pd.quantitativeData.closestSystem.updateCelestialBodyDistance(pd);
                pd.quantitativeData.closestSystem.updateSunDistance(pd);
            }
        }

        public void updateSolarSystemDistance(PlayerManager.PlayerData pd)
        {
            float closestDistance = Mathf.Infinity;
            SolarSystem closestSystem = null;
        
            for (int i = 0; i < systems.Count; i++)
            {
                float distance = Vector3.Distance(systems[i].transform.position, pd.components.gameObject.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestSystem = systems[i];
                }
            }
        
            pd.quantitativeData.closestSystem = closestSystem;
            pd.quantitativeData.distanceToClosestSystem = closestDistance;
        }

        public static Action loop;

        IEnumerator mainLoop()
        {
            while (true)
            {
                if (loop != null)
                    loop.Invoke();
                tick++;
                yield return new WaitForSeconds(tickRate);
            }
        }
    }
}
