using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace Player
{
    public class Interaction : MonoBehaviour
    {
        private PlayerManager.PlayerData pd;
        private PlayerManager playerManager;
        
        public void OnEnterExit()
        {
            playerManager = Universe.instance.playerManager;
            pd = playerManager.currentPlayer;

            if (pd.type == PlayerManager.Type.Ship)
            {
                exitShip();
            }
        }
        
        private void exitShip()
        {
            PlayerManager.PlayerData newPd = playerManager.players[0];
            
            playerManager.summonPlayer(newPd);
            playerManager.disablePlayer(pd);
            playerManager.setCurrentPlayer(newPd);
        }
    }
}
