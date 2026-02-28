using _Game.Scripts.Core.Conveyor;
using _Game.Scripts.Core.Cubes;
using _Game.Scripts.Core.Grid;
using _Game.Scripts.Core.Input;
using _Game.Scripts.Core.Levels;
using _Game.Scripts.Services;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Core.Launch
{
    public class GameBootstrapper : MonoBehaviour
    {
        [Title("Controllers")]
        [SerializeField] private GridController gridController;
        [SerializeField] private LevelController levelController;
        [SerializeField] private CubeController cubeController;
        
        [Title("Services")]
        [SerializeField] private InputService inputService;
        
        private IGridProvider _gridProvider;
        private ILevelProvider _levelProvider;
        private ICubeProvider _cubeProvider;

        private void Awake()
        {
            Setup();
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<IInputService>();
        }

        private void Start()
        {
            InitializeGame();
        }

        private void Setup()
        {
            ServiceLocator.Register<IInputService>(inputService);
            
            _gridProvider = gridController;
            _levelProvider = levelController;
            _cubeProvider = cubeController;
        }

        private void InitializeGame()
        {
            _levelProvider.Init(_gridProvider, _cubeProvider);
            inputService.Init();
        }
    }
}