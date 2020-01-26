using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

public class TestRaycast : MonoBehaviour
{

    private Entity Raycast(float3 fromPosition, float3 toPosition)
    {
        BuildPhysicsWorld buildPhysicsWorld = World.DefaultGameObjectInjectionWorld.GetExistingSystem<BuildPhysicsWorld>();
        CollisionWorld collisionWorld = buildPhysicsWorld.PhysicsWorld.CollisionWorld;

        RaycastInput raycastInput = new RaycastInput
        {
            Start = fromPosition,
            End = toPosition,
            Filter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = ~0u,
                GroupIndex = 0,
            }
        };

        Unity.Physics.RaycastHit raycastHit = new Unity.Physics.RaycastHit();

        if (collisionWorld.CastRay(raycastInput, out raycastHit))
        {
            // Hit something
            Entity hitEntity = buildPhysicsWorld.PhysicsWorld.Bodies[raycastHit.RigidBodyIndex].Entity;
            return hitEntity;
        }
        else
        {
            return Entity.Null;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float rayDistance = 100f;
            Debug.Log(Raycast(ray.origin, ray.direction * rayDistance));
        }
    }

}
