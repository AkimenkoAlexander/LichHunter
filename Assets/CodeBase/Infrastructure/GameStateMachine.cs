using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.State;
using CodeBase.Logic;

namespace CodeBase.Infrastructure
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;


        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain curtain, AllServices services  )
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootStrapState)] = new BootStrapState(this, sceneLoader,services),
                [typeof(LoadSceneState)] = new LoadSceneState(this, sceneLoader,curtain, services.Single<IGameFactory>(),services.Single<IPersistentProgressService>()),
                [typeof(LoadProgressState)] = new LoadProgressState(this, services.Single<IPersistentProgressService>(),services.Single<ISaveLoadSrvice>()),
                [typeof(GameLoopState)] = new GameLoopState(this)
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }
        public void Enter<TState,TPayLoad>(TPayLoad payLoad) where TState : class, IPayLoadedState<TPayLoad>
        { 
            TState state = ChangeState<TState>();
            state.Enter(payLoad);
        }
        private TState ChangeState<TState>() where TState:class, IExitableState
        {	
           _activeState?.Exit();
           TState state = GetState<TState>();
           _activeState = state;
           return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState => 
            _states[typeof(TState)] as TState;
    }

   
}