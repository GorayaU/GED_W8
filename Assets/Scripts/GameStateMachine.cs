using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Fuel fuel;                       
    [SerializeField] private PlayerHitDetector playerHit;     
    [SerializeField] private GameObject gameOverUI;           

    private IGameState _current;
    private PlayingState _playing;
    private GameOverState _gameOver;

    private void Awake()
    {
        _playing = new PlayingState(this, fuel, playerHit, gameOverUI);
        _gameOver = new GameOverState(this, gameOverUI);
    }

    private void Start()
    {
        SetState(_playing);
    }

    public void SetState(IGameState next)
    {
        _current?.Exit();
        _current = next;
        _current.Enter();
    }

    public interface IGameState
    {
        void Enter();
        void Exit();
    }

    private class PlayingState : IGameState
    {
        private readonly GameStateMachine _ctx;
        private readonly Fuel _fuel;
        private readonly PlayerHitDetector _playerHit;
        private readonly GameObject _gameOverUI;

        public PlayingState(GameStateMachine ctx, Fuel fuel, PlayerHitDetector playerHit, GameObject gameOverUI)
        {
            _ctx = ctx;
            _fuel = fuel;
            _playerHit = playerHit;
            _gameOverUI = gameOverUI;
        }

        public void Enter()
        {
            if (_gameOverUI) _gameOverUI.SetActive(false);

            if (_fuel) _fuel.OnFuelUsedUp += HandleFuelOut;
            if (_playerHit) _playerHit.OnHitEnemy += HandlePlayerHitEnemy;

            Time.timeScale = 1f; // resume game if needed
        }

        public void Exit()
        {
            if (_fuel) _fuel.OnFuelUsedUp -= HandleFuelOut;
            if (_playerHit) _playerHit.OnHitEnemy -= HandlePlayerHitEnemy;
        }

        private void HandleFuelOut()
        {
            _ctx.SetState(new GameOverState(_ctx, _gameOverUI));
        }

        private void HandlePlayerHitEnemy()
        {
            _ctx.SetState(new GameOverState(_ctx, _gameOverUI));
        }
    }

    private class GameOverState : IGameState
    {
        private readonly GameStateMachine _ctx;
        private readonly GameObject _gameOverUI;

        public GameOverState(GameStateMachine ctx, GameObject gameOverUI)
        {
            _ctx = ctx;
            _gameOverUI = gameOverUI;
        }

        public void Enter()
        {
            if (_gameOverUI) _gameOverUI.SetActive(true);
            Time.timeScale = 0f; // pause
        }

        public void Exit()
        {
            if (_gameOverUI) _gameOverUI.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
