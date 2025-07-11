using System;
using UnityEngine;

namespace Tamer.Develop.Gameplay
{
    public class GameplayBootstrap : MonoBehaviour
    {
        private DependOn _dependOn;
        private GameplayEntity _gameplayEntityPrefab;

        private void Awake()
        {
            _dependOn = new DependOn(1);
            var gameplayEntity = Instantiate(_gameplayEntityPrefab);
            gameplayEntity.Initialize(Yap);
        }

        private bool Yap(bool reg1, bool reg2)
        {
            return reg1 && reg2;
        }
    }

    public class GameplayEntity : MonoBehaviour
    {
        private DependOn _dependOn;

        public void Initialize(Func<bool,bool,bool> t)
        {
            t.Invoke(true,true);
        }

        private void Start()
        {
            Debug.Log(_dependOn.Dependency);
        }
    }

    public class DependOn
    {
        private readonly int _dependency;

        public DependOn(int dependency)
        {
            _dependency = dependency;
        }

        public int Dependency => _dependency;
    }
}