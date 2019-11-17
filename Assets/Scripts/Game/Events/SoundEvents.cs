namespace SoundEvents
{
    public enum SoundType
    {
        None,
        ButtonClick,
        BombExplosion,
        DeathHero,
        DeathEnemy,
    }

    public class SoundEvent : GameEvent
    {
        public SoundType type;
    }
}
