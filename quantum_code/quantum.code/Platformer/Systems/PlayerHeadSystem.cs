using Photon.Deterministic;
using Quantum.Core;
using Quantum.Physics3D;

namespace Quantum.Platformer
{
    public unsafe class PlayerHeadSystem : SystemMainThreadFilter<PlayerHeadSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef EntityRef;
            public PlayerPlatformController* PlatformController;
            public CharacterController3D* CharacterController;
            public Transform3D* Transform3D;
        }
        
        private static HitCollection3D OverlapShapeThenCast(
            FrameBase f, 
            FPVector3 position,
            FPQuaternion rotation,
            FPVector3 castTranslation,
            Shape3D shape3D, 
            int layerMask = -1,
            QueryOptions options = QueryOptions.HitAll
        )
        {
            var hits = f.Physics3D.OverlapShape(
                position,
                rotation,
                shape3D,
                layerMask,
                options
            );

            return hits.Count == 0 ? 
                f.Physics3D.ShapeCastAll(
                    position, 
                    rotation,
                    shape3D, 
                    castTranslation, 
                    layerMask
                ) : 
                hits;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            var asset = f.FindAsset<PlatformControllerConfig>(filter.PlatformController->Config.Id);

            var head = new FPVector3(0, asset.HeadOffset, 0) + filter.Transform3D->Position;

            var hits = OverlapShapeThenCast(
                f,
                head,
                FPQuaternion.Identity,
                filter.CharacterController->Velocity * f.DeltaTime,
                Shape3D.CreateSphere(FP._0_50),
                options: QueryOptions.HitAll | QueryOptions.ComputeDetailedInfo
            );
            
            hits.Sort(head);
            
            FP penetrationCorrectionFactor = asset.PlayerHeadPenetrationCorrectionFactor;

            for (int i = 0; i < hits.Count; i++)
            {
                var hit = hits[i];

                if (!f.Unsafe.TryGetPointer<Platform>(hit.Entity, out var platform))
                    continue;
                
                filter.CharacterController->Velocity -= FPVector3.Dot(filter.CharacterController->Velocity, hit.Normal) * hit.Normal;
                filter.Transform3D->Position += hit.Normal * hit.OverlapPenetration * penetrationCorrectionFactor;

                platform->MovementBlockerCount++;
            }
        }
    }
}
