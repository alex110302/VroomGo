using UnityEngine;

public class State<T> where T : Controller<T>
{
    public bool IsExecuting { get; protected set; } = false;

    protected T controller;

    public State(T controller) { this.controller = controller; }  

    public virtual void Enter() { IsExecuting = true; }
    public virtual void Execute() { }
    public virtual void FixedExecute() { }
    public virtual void Exit() { IsExecuting = false; }

    public virtual void RunCollisonEnter(Collision2D collision) { }
    public virtual void RunCollisonStay(Collision2D collision) { }
    public virtual void RunCollisonExit(Collision2D collision) { }

    public virtual void RunTriggerEnter(Collider2D collision) { }
    public virtual void RunTriggerStay(Collider2D collision) { }
    public virtual void RunTriggerExit(Collider2D collision) { }
}
