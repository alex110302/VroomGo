using UnityEngine;

public abstract class MovementHandler2D<T> : IMovementHandler where T : Controller<T> 
{
    public MovementHandler2D(T controller, Rigidbody2D rb)
    {
        this.controller = controller;
        this.rb = rb;
    }
    protected Rigidbody2D rb;

    protected T controller;

    public abstract void MovementInputHandler();

    public abstract void MovementHandler();
}