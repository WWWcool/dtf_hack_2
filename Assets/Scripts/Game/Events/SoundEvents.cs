namespace SoundEvents
{
    public enum SoundType
    {
        None,
        BallHitEnvironment,
        BallHitGoal,
        BallPassedThroughGoal,
        SpikesAppeared,
        LevelFailed,
        LevelCompleted,
        ButtonClick,
        Face,
    }

    public class SoundEvent : GameEvent
    {
        public SoundType type;
    }
}
