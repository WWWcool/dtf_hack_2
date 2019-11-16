namespace InputEvents
{
    public enum SwipeDirection
    {
        Left,
        Right,
        Up,
        Down,
    }

    public class SwipeDetected : GameEvent
    {
        public SwipeDirection direction;
    }
}
