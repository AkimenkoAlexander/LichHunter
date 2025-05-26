using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Player
{
  [RequireComponent(typeof(CharacterController))]
  public class PlayerMove : MonoBehaviour, ISavedProgress
  {
    public PlayerAnimator PlayerAnimator;
    public float Speed = 4.0f;
    
    [SerializeField] private CharacterController _characterController;

    private IGameFactory _gameFactory;
    private Vector3 _inputVector; 
    private Joystick _joystick;

     private void Start()
     {
       _gameFactory = AllServices.Container().Single<IGameFactory>();
       if (_gameFactory.hud !=null)
       {
         GetInputService();
       }
       _gameFactory.HudCreated += GetInputService;
     }
     
    private void Update()
    {
      MovePlayer();
      AnimationWalkCotroll();
    }

    private void MovePlayer()
    {
      _inputVector = Vector3.zero;
      if (JoystickInitiazie() && _joystick.GetAxis().sqrMagnitude > 0.01f)
      {
        _inputVector = GetInputVector3();
        
        _inputVector = Camera.main.transform.TransformDirection(_inputVector);
        _inputVector.y = 0;
        _inputVector = _inputVector.normalized * _joystick.GetAxis().magnitude;
        transform.forward = _inputVector;
      }
      _inputVector += Physics.gravity;
      _characterController.Move(_inputVector * Speed * Time.deltaTime);
    }


    private bool JoystickInitiazie()
    {
      return _joystick != null;
    }

    private void AnimationWalkCotroll()
    {
      Vector3 tempVector = GetInputVector3();
      PlayerAnimator.Move(tempVector.magnitude);
      if (tempVector.magnitude <= 0 )
      {
        PlayerAnimator.StopMoving();
      }
    }

    private Vector3 GetInputVector3() => 
      new(_joystick.GetAxis().x, 0, _joystick.GetAxis().y);


    private void GetInputService() => 
      _joystick = _gameFactory.hud.GetComponentInChildren<Joystick>();

    public void UpdateProgress(PlayerProgress progress)
    {
      progress.WorldData.PositionOnLevel =
        new PositionOnLevel(CurrentLevel(), transform.position.AsVector3Data());
    }

    public void LoadProgress(PlayerProgress progress)
    {
      if (CurrentLevel() == progress.WorldData.PositionOnLevel.Level)
      {
        Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
        if (savedPosition != null)
          Warp(savedPosition);
      }
    }

    private void Warp(Vector3Data savedPosition)
    {
      _characterController.enabled = false;
      transform.position = savedPosition.AsUnityVector().AddY(2);
      _characterController.enabled = true;
    }

    private static string CurrentLevel() => 
      SceneManager.GetActiveScene().name;
  }
}
