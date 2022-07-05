using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using World;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        public enum Type
        {
            Ship,
            Lander,
            Freighter,
        }
        [Space(20)]
        public PlayerData currentPlayer;
        public Type currentType;
    
        [Space(20)]
        public List<PlayerData> playersInWorld;

        [Space(20)]
        public List<PlayerData> players;

        [Space(20)]
        public GameObject UI;

        private void Awake()
        {
            currentType = Type.Lander;
            summonPlayer(players[1]);
            setCurrentPlayer(players[1]);
        }

        public void summonPlayer(PlayerData pd)
        {
            playersInWorld.Add(pd);
            GameObject player = Instantiate(pd.prefab);
            pd.components.gameObject = player;
            pd.components.rb = player.GetComponent<Rigidbody>();
            pd.components.camera = player.GetComponentInChildren<Camera>();
            pd.components.input = player.GetComponent<PlayerInput>();

            if (pd.type == Type.Ship)
            {
                player.GetComponent<ShipController>().pd = pd;
            }
            if (pd.type == Type.Lander)
                player.GetComponent<LanderController>().pd = pd;
        }

        public void setCurrentPlayer(PlayerData pd)
        {
            pd.components.camera.enabled = true;
            pd.components.input.enabled = true;
            pd.components.rb.isKinematic = false;
            currentPlayer = pd;
            setUI(pd);
        }

        public void disablePlayer(PlayerData pd)
        {
            pd.components.camera.enabled = false;
            pd.components.input.enabled = false;
            pd.components.rb.isKinematic = true;
        }

        public void setUI(PlayerData pd)
        {
            if (UI.transform.childCount > 0)
                Destroy(UI.transform.GetChild(0));
            pd.UIInstance = Instantiate(pd.UIPrefab, UI.transform);
        }

        public Vector3? getSpawnLocation(Vector3 size, bool spawnOnLand)
        {
            Vector3 playerPos = currentPlayer.components.gameObject.transform.position;

            Vector3? spawnLoc = null;
        
            if (spawnOnLand)
            {
                if (Physics.Raycast(playerPos + new Vector3(size.x, 100, 0), Vector3.down, out var hit))
                {
                    spawnLoc = hit.point + new Vector3(0,size.y / 2,0);
                }
            
                print("no spawn location found, please try again");
            }
            else
            {
                spawnLoc = currentPlayer.components.gameObject.transform.position + new Vector3(size.x, 0,0);
            }

            return spawnLoc;
        }

        public AtmosphericLayer[] atmosphericLayers =
        {
            new AtmosphericLayer("Troposphere", 4, 20, 100),
            new AtmosphericLayer("Stratosphere", 2, 10, 200),
            new AtmosphericLayer("Mesosphere", 1.5f, 7.5f, 300),
            new AtmosphericLayer("Thermosphere", 1.25f, 6, 400),
            new AtmosphericLayer("Exosphere", 1, 5, 500)
        };
    
        public class AtmosphericLayer
        {
            public string name;
            public float drag;
            public float angularDrag;
            public float altitude;
            public AtmosphericLayer(string name, float drag, float angularDrag, float altitude)
            {
                this.name = name;
                this.drag = drag;
                this.angularDrag = angularDrag;
                this.altitude = altitude;
            }
        }

        [Serializable]
        public class PlayerData
        {
            [Serializable]
            public class ComponentData
            {
                public GameObject gameObject;
                public Rigidbody rb;
                public Camera camera;
                public PlayerInput input;
            }
            
            public string name;
            public ComponentData components;
            public QuantitativeData quantitativeData;
            public Type type;
            public GameObject prefab;
            public GameObject UIPrefab;
            public GameObject UIInstance;
            public Vector3 size;

            [Serializable]
            public class QuantitativeData
            {
                public SolarSystem closestSystem;
                public float distanceToClosestSystem;
                public CelestialBody closestCelestialBody;
                public float distanceToClosestCelestialBody;
                public float distanceToClosestCelestialBodySurface;
                public float distanceToSun;
                public AtmosphericLayer atmosphericLayer;
                public int atmosphericLayerIndex;
            }
        }
    }
}
