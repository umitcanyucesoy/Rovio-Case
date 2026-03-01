using _Game.Scripts.Core.Conveyor;
using _Game.Scripts.Core.Cubes;
using _Game.Scripts.Core.GameFlow;
using _Game.Scripts.Core.Grid;
using _Game.Scripts.Core.Input;
using _Game.Scripts.Core.Levels;
using _Game.Scripts.Core.Slots;
using _Game.Scripts.Core.UI;
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
        [SerializeField] private SlotController slotController;
        [SerializeField] private GameFlowController gameFlowController;
        [SerializeField] private UIController uiController;
        
        [Title("Services")]
        [SerializeField] private InputService inputService;
        [SerializeField] private GridService gridService;
        
        private ILevelProvider _levelProvider;
        private ICubeProvider _cubeProvider;
        private ISlotProvider _slotProvider;
        private IUIProvider _uiProvider;

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
            _slotProvider = slotController;
            _uiProvider = uiController;
        }

        private void InitializeGame()
        {
            conveyorController.Init();
            _levelProvider.Init(_cubeProvider, _slotProvider);
            gameFlowController.Init(_uiProvider, _levelProvider);
            inputService.Init();
        }
    }
}