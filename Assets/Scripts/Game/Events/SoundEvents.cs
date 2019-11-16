namespace SoundEvents
{
    public enum SoundType
    {
        None,
        ButtonClick,
    }

    public class SoundEvent : GameEvent
    {
        public SoundType type;
    }
}
