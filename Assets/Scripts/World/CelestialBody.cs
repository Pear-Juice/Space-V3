using Generation;
using UnityEngine;

namespace World
{
    public class CelestialBody : MonoBehaviour
    {
        public float atmosphereRadius;
        public float mass = 1;
        public float distanceToSurfaceOffset;
    
        [Range(0,2)]
        public float gravityFalloff = 1;
        public CelestialBodyGenerator generator;

        public void Update()
        {
            foreach (var player in Universe.instance.playerManager.playersInWorld)
            {
                Vector3 dir = (transform.position - player.components.gameObject.transform.position).normalized;
                float force = 9.8f * mass / (player.quantitativeData.distanceToClosestCelestialBodySurface + distanceToSurfaceOffset);
                if (!float.IsPositiveInfinity(force))
                    player.components.rb.AddForce(dir * (force * Time.deltaTime));
            }
        }
    }
}
