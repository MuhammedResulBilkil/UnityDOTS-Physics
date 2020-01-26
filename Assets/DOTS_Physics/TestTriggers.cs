using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

public class TestTriggers : JobComponentSystem
{

    [BurstCompile]
    private struct TriggerJob : ITriggerEventsJob
    {

        public ComponentDataFromEntity<PhysicsVelocity> physicsVelocityEntities;

        public void Execute(TriggerEvent triggerEvent)
        {
            if (physicsVelocityEntities.HasComponent(triggerEvent.Entities.EntityA))
            {
                PhysicsVelocity physicsVelocity = physicsVelocityEntities[triggerEvent.Entities.EntityA];
                physicsVelocity.Linear.y = 5f;
                physicsVelocityEntities[triggerEvent.Entities.EntityA] = physicsVelocity;
            }

            if (physicsVelocityEntities.HasComponent(triggerEvent.Entities.EntityB))
            {
                PhysicsVelocity physicsVelocity = physicsVelocityEntities[triggerEvent.Entities.EntityB];
                physicsVelocity.Linear.y = 5f;
                physicsVelocityEntities[triggerEvent.Entities.EntityB] = physicsVelocity;
            }
        }
    }


    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;

    protected override void OnCreate()
    {
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        TriggerJob triggerJob = new TriggerJob
        {
            physicsVelocityEntities = GetComponentDataFromEntity<PhysicsVelocity>()
        };
        return triggerJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
    }
}
