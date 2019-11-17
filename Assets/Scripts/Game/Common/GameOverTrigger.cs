using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class GameOverTrigger : MonoBehaviour
    {
        public void TriggerGameOver(bool won)
        {
            EventBus.Instance.Raise(new GameEvents.OnGameOver { won = won });
        }
    }
}
