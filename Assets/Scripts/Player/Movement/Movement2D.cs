using System;
using Unity.Mathematics;
using UnityEngine;

public class Movement2D : MovementHandler2D<PlayerController>
{
    public Movement2D(PlayerController controller, Rigidbody2D rb) : base(controller, rb) { }

    private Vector2 directionVec;

    private bool isDirecitonNormalized = true;

    public override void MovementInputHandler()
    { 
        directionVec = controller.PCI.Player2D.Move.ReadValue<Vector2>();

        Turn();
    }

    private void Turn()
    {

    }

    public override void MovementHandler()
    {
        Move();
        DetectWall();
    }

    private void DetectWall()
    {
        float size = 1.1f;

        Ray right = new Ray(controller.transform.position, controller.transform.right);
        Ray left = new Ray(controller.transform.position, controller.transform.right * -1);
        Ray up = new Ray(controller.transform.position, controller.transform.up);
        Ray down = new Ray(controller.transform.position, controller.transform.up * -1);

        RaycastHit[] hits = new RaycastHit[4];

        Physics.Raycast(right, out hits[0], size, controller.BoundMask);
        Physics.Raycast(left, out hits[1], size, controller.BoundMask);
        Physics.Raycast(up, out hits[2], size, controller.BoundMask);
        Physics.Raycast(down, out hits[3], size, controller.BoundMask);
        
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null) isDirecitonNormalized = true;
            else isDirecitonNormalized = false;
        }
    }

    private void Move()
    {
        if (isDirecitonNormalized) directionVec.Normalize();
        controller.RB.linearVelocity = directionVec * controller.Speed;
    }

    public bool CheckCardinalKeyPressed(CardinalKey key)
    {
        switch (key)
        {
            case CardinalKey.W: return directionVec.y > 0;
            case CardinalKey.A: return directionVec.x < 0;
            case CardinalKey.S: return directionVec.y < 0;
            case CardinalKey.D: return directionVec.x > 0;
            default: return false;
        }
    }

    public enum CardinalKey
    {
        W,
        A,
        S,
        D
    }
}