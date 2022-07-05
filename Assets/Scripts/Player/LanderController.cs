using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using World;

namespace Player
{
    public class LanderController : MonoBehaviour
    {
        public Rigidbody rb;
        [ReadOnly]
        public PlayerManager.PlayerData pd;
        private Vector2 moveVal;

        public WalkSettings walkSettings = new WalkSettings();
    
        [Serializable]
        public class WalkSettings
        {
            public float walkSpeed;
            public float jumpHeight;
        }
    
        public void OnMove(InputValue val)
        {
            moveVal = val.Get<Vector2>();
        }

        private void Update()
        {
            UpdateMove();

            if (pd.quantitativeData.closestCelestialBody)
            {
                if (Array.IndexOf(Universe.instance.playerManager.atmosphericLayers, pd.quantitativeData.atmosphericLayer) <= 3)
                {
                    transform.LookAt(pd.quantitativeData.closestCelestialBody.transform);
                    transform.eulerAngles += new Vector3(-90, 0, 0);
                }
            }
        }

        public void UpdateMove()
        {
            rb.AddRelativeForce(new Vector3(moveVal.x * walkSettings.walkSpeed * Time.deltaTime, 0, moveVal.y * walkSettings.walkSpeed * Time.deltaTime));
        }
    }
}
