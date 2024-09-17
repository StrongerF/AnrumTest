namespace Quantum.Platformer
{
    public unsafe class PlatformMovementResetSystem : SystemMainThreadFilter<PlatformMovementResetSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef EntityRef;
            public Platform* Platform;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            filter.Platform->ResetMovementBlockers();
        }
    }
}