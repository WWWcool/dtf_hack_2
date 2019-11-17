using UnityEngine;

namespace GameEvents
{
    public class OnTileReached : GameEvent
    {
        public bool spawnTail;
        public Vector2 currPosition;
    }

    public class OnDash : GameEvent
    {
        public Vector2 startPosition;
        public Vector2 direction;
        public Vector2 finishPosition;
        public OnDash(Vector2 startPosition, Vector2 direction, Vector2 finishPosition)
        {
            this.startPosition = startPosition;
            this.direction = direction;
            this.finishPosition = finishPosition;
        }
    }

    public class OnDashRecharge : GameEvent
    {
        public int newCount;
    }

    public class OnGameOver : GameEvent
    {
        public bool won = false;
    }
}
