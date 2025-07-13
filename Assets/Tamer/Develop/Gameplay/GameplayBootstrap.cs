using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tamer.Develop.Features.SpringFeature;
using UnityEngine;

namespace Tamer.Develop.Gameplay
{
    public class GameplayBootstrap : MonoBehaviour
    {
        [SerializeField] private SpringConnection _springConnectionPrefab;

        [SerializeField] private Rigidbody _connectionParent;

        private Cube _cubePrefab;

        private GhostCube _ghostCubePrefab;

        private List<GhostCube> _ghostCubes = new();

        private GhostCube _chosenGhostCube;

        private async void Awake()
        {
            // var springConnection = Instantiate(_springConnectionPrefab);
            // springConnection.Connect(_connectionParent);

            _cubePrefab = Resources.Load<Cube>("Cube");
            _ghostCubePrefab = Resources.Load<GhostCube>("GhostCube");

            var cube = Instantiate(_cubePrefab);

            // Instantiate(_ghostCubePrefab, cube.GhostCubes[Random.Range(0,cube.GhostCubes.Count)].position, Quaternion.identity, null);

            _chosenGhostCube = await Generate(cube);
        }

        private async UniTask<GhostCube> Generate(Cube cube)
        {
            foreach (var cubeSpawnPoint in cube.GhostCubes)
            {
                var ghostCube = Instantiate(_ghostCubePrefab, cubeSpawnPoint.position, Quaternion.identity, null);

                ghostCube.gameObject.SetActive(false);

                _ghostCubes.Add(ghostCube);
            }

            await UniTask.WaitForSeconds(1);

            var chosenCube = _ghostCubes[Random.Range(0, _ghostCubes.Count)];

            chosenCube.gameObject.SetActive(true);

            return chosenCube;
        }

        private void Update()
        {
            // if (Input.GetMouseButtonDown(0))
            // {
            //     var created = Instantiate(_cubePrefab,_chosenGhostCube.transform.position, Quaternion.identity, null);
            //     Destroy(_chosenGhostCube.gameObject);
            // }
        }
    }
}