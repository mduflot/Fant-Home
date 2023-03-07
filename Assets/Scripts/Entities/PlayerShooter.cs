using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

namespace Entities
{
    public class PlayerShooter : MonoBehaviour
    {
        [SerializeField] private float _projectileSpeed = 50f;
        [SerializeField] private float _reloadTime = 0.05f;
        [SerializeField] private Material _material;
        [SerializeField] private Mesh _mesh;

        private bool _shootOrder;
        private EntityManager _entityManager;
        private float _lastShootTime;

        private RenderMeshDescription _renderMeshDescription =
            new(ShadowCastingMode.Off, false);

        private EntityArchetype _entityArchetype;

        private void Start()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            _entityArchetype =
                _entityManager.CreateArchetype(typeof(LocalTransform), typeof(RenderMeshArray), typeof(MoveForward),
                    typeof(ProjectileTag));
        }

        private void Update()
        {
            _shootOrder = Input.GetMouseButton(0);
        }

        private void FixedUpdate()
        {
            if (_shootOrder)
            {
                if (_lastShootTime + _reloadTime < Time.fixedTime)
                {
                    _lastShootTime = Time.fixedTime;

                    Shoot();
                }
            }
        }

        private void Shoot()
        {
            Entity entity = _entityManager.CreateEntity(_entityArchetype);
            _entityManager.SetName(entity, "Projectile");

            _entityManager.SetComponentData(entity, new LocalTransform()
            {
                Position = transform.position,
                Rotation = transform.rotation,
                Scale = 1
            });

            RenderMeshUtility.AddComponents(entity, _entityManager, _renderMeshDescription,
                new RenderMeshArray(new Material[] { _material }, new Mesh[] { _mesh }),
                MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));

            _entityManager.SetComponentData(entity, new MoveForward()
            {
                Speed = _projectileSpeed
            });
        }
    }
}