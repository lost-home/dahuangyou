using Prime31;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMoveController : AbstractMonoBehaviour
{
    [Header("移动速度")]
    public float MoveSpeed = 3f;

    [HideInInspector] public CharacterController2D controller;

    protected override void Awake()
    {
        timeLayer = CupheadTime.Layer.Player;
        base.Awake();

        controller = GetComponent<CharacterController2D>();
    }

    public void Move(float horizontal, float vertical)
    {
        if (horizontal > 0f)
            horizontal = 1f;
        else if (horizontal < 0f)
            horizontal = -1f;
        else horizontal = 0f;

        if (vertical > 0f)
            vertical = 1f;
        else if (vertical < 0f)
            vertical = -1f;
        else vertical = 0f;

        controller.Move(new Vector2(
            horizontal != 0 ? horizontal * MoveSpeed * LocalDeltaTime : 0f,
            vertical != 0 ? vertical * MoveSpeed * LocalDeltaTime : 0f));
    }
}
