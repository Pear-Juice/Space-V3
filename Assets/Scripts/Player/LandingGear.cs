using UnityEngine;

namespace Player
{
    public class LandingGear : MonoBehaviour
    {
        public ShipController controller;
        public bool isLanded = false;
        public int numPointsToLand;
        public bool isLandable = false;
        public static int gearPointsConnected;
    
        public void OnLand()
        {
            if (!isLandable)
            {
                print("landing extended");
                isLandable = true;
                checkLanded();
            }
            else
            {
                print("landing retracted");
                isLandable = false;
                isLanded = false;

                controller.rb.constraints = RigidbodyConstraints.None;
            }
        }
    
        private void OnCollisionEnter(Collision collision)
        {
            gearPointsConnected += 1;
            checkLanded();
        }

        private void checkLanded()
        {
            if (isLandable)
                print($"Points touched: {gearPointsConnected} / {numPointsToLand}");
            
            if (gearPointsConnected >= numPointsToLand && isLandable)
            {
                print("touched down");
                isLanded = true;

                controller.rb.constraints = RigidbodyConstraints.FreezeAll;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            gearPointsConnected -= 1;
        }
    }
}
