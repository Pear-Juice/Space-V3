using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class ShipController : MonoBehaviour
    {
        public PlayerManager.PlayerData pd;
        public Rigidbody rb;

        [Space(20)] public FlightSettings flightSettings;
    
        [Serializable]
        public class FlightSettings
        {
            public float flightSpeed = 600;
            public float reverseFlightSpeed = 400;
            public float verticalSpeed = 1000;
            public float boost = 10;
            public float rollSpeed = 2000f;
            public float turnSpeed = 7f;
        }
    
        private GameObject mousePointer;
    
        public float maxCursorDistance;

        private Vector2 mouseVal;
        private Vector2 moveVal;
        private float moveVertical;

        private float boostAmount = 1;

        // Start is called before the first frame update
        void Start()
        {
            mousePointer = pd.UIInstance.transform.Find("MousePointer").gameObject;

            flightSettings.flightSpeed *= Settings.shipSettings.speedMultiplier;
            flightSettings.reverseFlightSpeed *= Settings.shipSettings.speedMultiplier;
            flightSettings.boost *= Settings.shipSettings.boostMultiplier;
            flightSettings.rollSpeed *= Settings.shipSettings.boostMultiplier;
            flightSettings.turnSpeed *= Settings.shipSettings.turnSpeedMultiplier;
        }

        // Update is called once per frame
        void Update()
        {
            updateMove();
            updateMouse();
        }

        public void OnMove(InputValue value)
        {
            moveVal = value.Get<Vector2>();
        }

        public void OnMoveVertical(InputValue value)
        {
            moveVertical = value.Get<float>();
            print(moveVertical);
        }

        public void OnLook(InputValue value)
        {
            mouseVal = value.Get<Vector2>();
        }

        public void OnBoost(InputValue value)
        {
            if (value.Get<float>() == 1)
            {
                boostAmount = flightSettings.boost;
            }
            else
            {
                boostAmount = 1;
            }
        }
    
        //Called by player input
        void updateMouse()
        {
            Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0.0f);
            Vector3 crosshairOrigin = new Vector3(mouseVal.x - screenCenter.x,mouseVal.y - screenCenter.y,0);

            float distance = Vector2.Distance(new Vector2(),crosshairOrigin);
            if (distance > maxCursorDistance) distance = maxCursorDistance;
 
            crosshairOrigin = crosshairOrigin.normalized * distance;
            mousePointer.transform.position = new Vector3(crosshairOrigin.x + screenCenter.x, crosshairOrigin.y + screenCenter.y);
            rb.AddRelativeTorque(new Vector3(-crosshairOrigin.y * flightSettings.turnSpeed, crosshairOrigin.x * flightSettings.turnSpeed, 0) * Time.deltaTime);
        }

        //called by player input
        void updateMove()
        {
            rb.AddRelativeForce(new Vector3(0,moveVertical * flightSettings.verticalSpeed, moveVal.y * flightSettings.flightSpeed * boostAmount) * Time.deltaTime);
            rb.AddRelativeTorque(new Vector3(0,0,-moveVal.x * flightSettings.rollSpeed * Time.deltaTime));
        }

        public void setDrag(PlayerManager.AtmosphericLayer layer)
        {
            //rb.drag = layer.
        }

        [System.Serializable]
        public class Drag
        {
            public float drag;
            public float angularDrag;
        
            public Drag(float drag, float angularDrag)
            {
                this.drag = drag;
                this.angularDrag = angularDrag;
            }
        }
    }
}
