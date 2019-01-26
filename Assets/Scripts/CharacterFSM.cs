using UnityEngine;
using RSG;
using Luminosity.IO;

public class CharacterFSM : AbstractMonoBehaviour
{
    //================================================================//
    // 状态定义
    //================================================================//
    public const string CSTATE_Idle = "Idle";
    public const string CSTATE_MoveLeft = "MoveLeft";
    public const string CSTATE_MoveRight = "MoveRight";
    public const string CSTATE_MoveUp = "MoveUp";
    public const string CSTATE_MoveDown = "MoveDown";


    private AbstractState m_stateMachine;
    private CharacterMoveController m_moveController;
    private SpriteRenderer m_actionRenderer;
    private Animator m_actionAnimator;

    protected override void Awake()
    {
        timeLayer = CupheadTime.Layer.Player;

        base.Awake();

        m_moveController = GetComponent<CharacterMoveController>();
        m_actionRenderer = transform.Find("ActionRenderer").GetComponent<SpriteRenderer>();
        m_actionAnimator = transform.Find("ActionRenderer").GetComponent<Animator>();

        initStateMachine();
    }

    private void initStateMachine()
    {
        m_stateMachine = new StateMachineBuilder().
            State(CSTATE_Idle)
                .Enter(state =>
                {

                })
                .Condition(() => InputManager.GetAxis("Horizontal") > 0f, state => m_stateMachine.ChangeState(CSTATE_MoveRight))
                .Condition(() => InputManager.GetAxis("Horizontal") < 0f, state => m_stateMachine.ChangeState(CSTATE_MoveLeft))
                .Condition(() => InputManager.GetAxis("Vertical") > 0f, state => m_stateMachine.ChangeState(CSTATE_MoveUp))
                .Condition(() => InputManager.GetAxis("Vertical") < 0f, state => m_stateMachine.ChangeState(CSTATE_MoveDown))
                .End().
            State(CSTATE_MoveLeft)
                .Enter(state =>
                {
                    m_actionAnimator.Play(CSTATE_MoveLeft);
                })
                .Update((state, dt) =>
                {
                    m_moveController.Move(InputManager.GetAxis("Horizontal"), InputManager.GetAxis("Vertical"));
                })
                .Condition(() => InputManager.GetAxis("Horizontal") == 0f, state => m_stateMachine.ChangeState(CSTATE_Idle))
                .Condition(() => InputManager.GetAxis("Horizontal") > 0f, state => m_stateMachine.ChangeState(CSTATE_MoveRight))
                .End().
            State(CSTATE_MoveRight)
                .Enter(state =>
                {
                    m_actionAnimator.Play(CSTATE_MoveRight);
                })
                .Update((state, dt) =>
                {
                    m_moveController.Move(InputManager.GetAxis("Horizontal"), InputManager.GetAxis("Vertical"));
                })
                .Condition(() => InputManager.GetAxis("Horizontal") == 0f, state => m_stateMachine.ChangeState(CSTATE_Idle))
                .Condition(() => InputManager.GetAxis("Horizontal") < 0f, state => m_stateMachine.ChangeState(CSTATE_MoveLeft))
                .End().
            State(CSTATE_MoveUp)
                .Enter(state =>
                {
                    m_actionAnimator.Play(CSTATE_MoveUp);
                })
                .Update((state, dt) =>
                {
                    m_moveController.Move(0f, InputManager.GetAxis("Vertical"));
                })
                .Condition(() => InputManager.GetAxis("Vertical") == 0f, state => m_stateMachine.ChangeState(CSTATE_Idle))
                .End().
            State(CSTATE_MoveDown)
                .Enter(state =>
                {
                    m_actionAnimator.Play(CSTATE_MoveDown);
                })
                .Update((state, dt) =>
                {
                    m_moveController.Move(0f, InputManager.GetAxis("Vertical"));
                })
                .Condition(() => InputManager.GetAxis("Vertical") == 0f, state => m_stateMachine.ChangeState(CSTATE_Idle))
                .End()
        .Build() as AbstractState;

        m_stateMachine.PushState(CSTATE_MoveRight);
    }

    protected override void Update()
    {
        m_stateMachine.Update(LocalDeltaTime);
    }
}

