using System.Collections.Generic;
using Player;
using UnityEngine;

namespace World
{
    public class SolarSystem : MonoBehaviour
    {
        public List<CelestialBody> bodies = new List<CelestialBody>();
        public void updateCelestialBodyDistance(PlayerManager.PlayerData pd)
        {
            float closestDistance = Mathf.Infinity;
            CelestialBody closestCelestialBody = null;
        
            for (int i = 0; i < bodies.Count; i++)
            {
                float distance = Vector3.Distance(pd.components.gameObject.transform.position, bodies[i].transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCelestialBody = bodies[i];
                }
            }
        
            pd.quantitativeData.closestCelestialBody = closestCelestialBody;
            if (closestCelestialBody != null)
                pd.quantitativeData.distanceToClosestCelestialBodySurface = closestDistance - closestCelestialBody.generator.shape.radius;
            pd.quantitativeData.distanceToClosestCelestialBody = closestDistance;
        }

        public void updateSunDistance(PlayerManager.PlayerData pd)
        {
            float distance = Vector3.Distance(transform.position, pd.components.gameObject.transform.position);
            pd.quantitativeData.distanceToSun = distance;
        }
    }
}
