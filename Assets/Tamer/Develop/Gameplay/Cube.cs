using System.Collections.Generic;
using Tamer.Develop.Features.SpringFeature;
using Tamer.Develop.Services.ResourcesManagement;
using UnityEngine;

namespace Tamer.Develop.Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class Cube : MonoBehaviour
    {
        [SerializeField] private List<Transform> _ghostCubes;

        public IReadOnlyList<Transform> GhostCubes => _ghostCubes;

        [SerializeField] private Transform _jointParent;
        [SerializeField] private List<SpringConnection> _springConnection;

        private SpringConnection _springConnectionPrefab;

        private Rigidbody _rigidbody;

        private List<SpringConnection> _springConnections = new();

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            _springConnectionPrefab = new ResourcesAssetLoader().Load<SpringConnection>("SpringConnection");

            _ghostCubes.ForEach(cube =>
                _springConnections
                    .Add(Instantiate(_springConnectionPrefab,
                        cube.position,
                        Quaternion.identity,
                        _jointParent)));

            _springConnections.ForEach(spring => spring.Connect(_rigidbody));
        }
    }
}