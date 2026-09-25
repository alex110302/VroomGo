using NUnit.Framework.Constraints;
using UnityEngine;

public class CarMovement : MovementHandler2D<CarController>
{
    public CarMovement(CarController controller, Rigidbody2D rb) : base(controller, rb) { }

    public override void MovementHandler()
    {
        controller.RB.linearVelocity = controller.transform.up * -controller.Speed;
    }

    public override void MovementInputHandler()
    {
        

    }
}
