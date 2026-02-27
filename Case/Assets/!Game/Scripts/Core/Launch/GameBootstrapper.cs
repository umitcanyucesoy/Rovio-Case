using System;
using _Game.Scripts.Core.Grid;
using _Game.Scripts.Core.Levels;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Core.Launch
{
    public class GameBootstrapper : MonoBehaviour
    {
        [Title("Controllers")]
        [SerializeField] private GridController gridController;
        [SerializeField] private LevelController levelController;
        
        private IGridProvider _gridProvider;
        private ILevelProvider _levelProvider;

        private void Awake()
        {
            Setup();
        }

        private void Start()
        {
            InitializeGame();
        }

        private void Setup()
        {
            _gridProvider = gridController;
            _levelProvider = levelController;
        }

        private void InitializeGame()
        {
            _levelProvider.Init(_gridProvider);
        }
    }
}