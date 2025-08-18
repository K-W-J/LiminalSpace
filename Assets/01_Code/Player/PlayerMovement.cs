using UnityEngine;
using Code.Entities;

namespace Code.Players
{
    //플레이어의 모든 육체적 움직임 관리.
    public class PlayerMovement : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private Rigidbody _rigidbody;

        public bool IsJumping => _rigidbody.linearVelocity.y > 3;
        public bool IsFalling => _rigidbody.linearVelocity.y < -3;
        public bool IsRuning { get; private set; }
        public bool IsFlyAway { get; set; }
        
        private GroundChecker _groundChecker;
        private StaminaChecker _staminaChecker;
        
        private Player _agent;
        public Vector3 Velocity => _velocity;
        private Vector3 _velocity;
        
        private Vector3 _cameraRotation;
        private Vector3 _cameraRotations;
        
        private float _moveSpeed;
        private float _cameraCurrentRotationZ;
        private int _cameraRotationSmooth;
        private int _jumpPower;

        public void Initialize(Entity entity)
        {
            _agent = entity as Player;
            _groundChecker = entity.GetCompo<GroundChecker>();
            _staminaChecker = entity.GetCompo<StaminaChecker>();

            if (_agent != null)
            {
                _moveSpeed = _agent.PlayerStatsSo.WalkSpeed;
                _jumpPower = _agent.PlayerStatsSo.JumpPower;
                _cameraRotationSmooth = _agent.PlayerStatsSo.CameraRotationSmooth;
            }
        }

        private void OnEnable()
        {
            _agent.PlayerInputSo.OnMoveAction += OnMovement;
            _agent.PlayerInputSo.AtLootAction += OnAtLoot;
            _agent.PlayerInputSo.OnCrouchAction += OnCrouch;
            _agent.PlayerInputSo.OnJumpAction += OnJump;
            _agent.PlayerInputSo.OnRunAction += OnRun;
        }
        private void OnDisable()
        {
            _agent.PlayerInputSo.OnMoveAction -= OnMovement;
            _agent.PlayerInputSo.AtLootAction -= OnAtLoot;
            _agent.PlayerInputSo.OnCrouchAction -= OnCrouch;
            _agent.PlayerInputSo.OnJumpAction -= OnJump;
            _agent.PlayerInputSo.OnRunAction -= OnRun;
        }
        
        private void Update()
        {
            
            _cameraRotations += new Vector3(_cameraRotation.y, _cameraRotation.x);
            
            _cameraRotations.x = Mathf.Clamp(_cameraRotations.x, -80f, 70f);
            
            float targetRotationZ = Mathf.Clamp(-_cameraRotation.x * 2f, -10f, 10f);
            _cameraCurrentRotationZ = Mathf.Lerp(_cameraCurrentRotationZ, targetRotationZ, Time.deltaTime * 5f);
            
            Quaternion currentCameraRotation = _agent.CinemaCamera.transform.localRotation;
            Quaternion targetCameraRotation = Quaternion.Euler(-_cameraRotations.x, _cameraRotations.y, _cameraCurrentRotationZ);
            Quaternion smoothCameraRotation = Quaternion.Lerp(currentCameraRotation, targetCameraRotation, _cameraRotationSmooth * Time.deltaTime);

            _agent.CinemaCamera.transform.localRotation = smoothCameraRotation;
            

            if (!_groundChecker.GroundCheck())
            {
                
                _rigidbody.linearVelocity += Vector3.up * (Physics.gravity.y * (3 * Time.deltaTime));
                IsFlyAway = true;
            }
            else
            {
                IsFlyAway = false;
            }

            if (!_staminaChecker.CanRun)
            {
                _moveSpeed = _agent.PlayerStatsSo.WalkSpeed;
                IsRuning = false;
            }
        }
        
        private void FixedUpdate()
        {
            if(IsFlyAway) return;
                
            Quaternion cameraRotation = Quaternion.Euler(0, _agent.CinemaCamera.transform.localEulerAngles.y, 0); //회전에 따라 이동 방향이도 수정
            Vector3 moveDir = new Vector3(_velocity.x, 0, _velocity.y) * _moveSpeed;
            moveDir.y = _rigidbody.linearVelocity.y;
 
            _rigidbody.linearVelocity = cameraRotation * moveDir;
        }
        
        public void SetVelocityZero() => _rigidbody.linearVelocity = Vector3.zero;
        
        public void SetForce(Vector3 velocity) => _rigidbody.AddForce(velocity, ForceMode.Impulse);
        
        private void OnAtLoot(Vector2 obj)
        {
            _cameraRotation = obj * 0.5f;
        }

        private void OnMovement(Vector2 obj)
        {
            _velocity = obj;
        }

        private void OnJump()
        {
            if (_groundChecker.GroundCheck())
            {
                _rigidbody.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
                _rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, 0, _rigidbody.linearVelocity.z);
            }
        }

        private void OnRun(bool obj)
        {
            if (!_staminaChecker.CanRun) return;

            if (obj)
            {
                _moveSpeed = _agent.PlayerStatsSo.RunSpeed;
                IsRuning = true;
            }
            else
            {
                _moveSpeed = _agent.PlayerStatsSo.WalkSpeed;
                IsRuning = false;
            }
            
        }
        
        private void OnCrouch(bool obj)
        {
            if (obj)
            {
                
            }
            else
            {
                
            }
        }
    }
}