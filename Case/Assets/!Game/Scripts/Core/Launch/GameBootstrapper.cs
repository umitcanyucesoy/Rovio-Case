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
        [SerializeField] private LevelController levelController;
        [SerializeField] private CubeController cubeController;
        [SerializeField] private ConveyorController conveyorController;
        
        [Title("Services")]
        [SerializeField] private InputService inputService;
        [SerializeField] private GridService gridService;
        
        private ILevelProvider _levelProvider;
        private ICubeProvider _cubeProvider;

        private void Awake()
        {
            Setup();
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<IInputService>();
            ServiceLocator.Unregister<IGridService>();
        }

        private void Start()
        {
            InitializeGame();
        }

        private void Setup()
        {
            ServiceLocator.Register<IInputService>(inputService);
            ServiceLocator.Register<IGridService>(gridService);
            
            _levelProvider = levelController;
            _cubeProvider = cubeController;
        }

        private void InitializeGame()
        {
            conveyorController.Init();
            _levelProvider.Init(_cubeProvider);
            inputService.Init();
        }
    }
}