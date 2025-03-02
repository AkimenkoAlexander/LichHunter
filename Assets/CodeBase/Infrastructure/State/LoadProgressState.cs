using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;

namespace CodeBase.Infrastructure.State
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadSrvice _saveLoadService;

        public LoadProgressState( GameStateMachine gameStateMachine, IPersistentProgressService persistentProgressService, ISaveLoadSrvice saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = persistentProgressService;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            _gameStateMachine.Enter<LoadSceneState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
        }

        public void Exit()
        {
          
        }

        private void LoadProgressOrInitNew()
        {
            _progressService.Progress = _saveLoadService.LoadProgress() ?? NewProgress();
        }

        private PlayerProgress NewProgress()
        {
             PlayerProgress progress = new PlayerProgress("Level1");
             progress.StateHealth.MaxHP = 50.0f;
             progress.StateHealth.ResetHP();
             progress.PlayerStats.Damage = 30.0f;
             progress.PlayerStats.DamageRadius = 1.0f;
             
             return progress;
        }
    }
}