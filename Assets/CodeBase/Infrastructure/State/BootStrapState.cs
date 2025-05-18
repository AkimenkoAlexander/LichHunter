
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.StaticData;
using IAssetProvider = CodeBase.Infrastructure.AssetManagment.IAssetProvider;


namespace CodeBase.Infrastructure.State
{
    public class BootStrapState : IState
    {
        private const string INITIAL = "InitializeScene";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;

        public BootStrapState (GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            RegisterServices();
        }

        public void Enter()
        {
          _sceneLoader.Load(INITIAL, EnterLoadLevel);
        }

        private void EnterLoadLevel() => 
            _stateMachine.Enter<LoadProgressState>();

        public void Exit()
        {
          
        }

        private void RegisterServices()
        {
            RegisterStaticData();
         _services.RegisterSingle<IAssetProvider>(new AssetProvider());
         _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssetProvider>(), _services.Single<IStaticDataService>()));
         _services.RegisterSingle<IPersistentProgressService>( new PersistentProgressService());
         _services.RegisterSingle<ISaveLoadSrvice>( new SaveLoadService( _services.Single<IPersistentProgressService>(), _services.Single<IGameFactory>()));
        }

        private void RegisterStaticData()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.LoadMonsters();
            _services.RegisterSingle(staticData);
        }
    }
}