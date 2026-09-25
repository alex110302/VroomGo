using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Controller<T> : MonoBehaviour where T : Controller<T> 
{
    public string Name { get; protected set; }
    public State<T> State { get; protected set; }
    /// <summary>
    /// This should be called after all game objects are initialized to the object
    /// </summary>
    protected abstract void InitializeStates();
    //If I ever wonder why this wrapper function is in here
    //its becuase states do not inherite from MonoBehavior
    public virtual void RunCoroutine(IEnumerator coroutine) { StartCoroutine(coroutine); }
    public virtual void ChangeState(State<T> newState)
    {
        if (newState != null)
        {
            State?.Exit();
            State = newState;
            State.Enter();
        }
        else Debug.LogError("Could not change CurrentState, The incomming state was null");
    }
}