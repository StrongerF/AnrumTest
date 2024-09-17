using System;
using Photon.Deterministic;
using Quantum;
using UnityEngine;

public class PlayerAnimationView : MonoBehaviour
{
    private const string SPEED_PARAMETER = "Speed";
    
    [SerializeField] private Animator _animator;

    private EntityView _view;
    
    private static readonly int _speed = Animator.StringToHash(SPEED_PARAMETER);

    private void Awake()
    {
        _view = GetComponent<EntityView>();
    }

    private void Update()
    {
        if(QuantumRunner.Default == null || QuantumRunner.Default.IsRunning == false)
            return;

        Frame f = QuantumRunner.Default.Game.Frames.Predicted;

        FPVector3 vec = f.Get<CharacterController3D>(_view.EntityRef).Velocity;

        _animator.SetFloat(_speed, vec == default ? 0f : 1f);
    }
}
