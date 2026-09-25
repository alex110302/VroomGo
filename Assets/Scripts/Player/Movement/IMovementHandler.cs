public interface IMovementHandler
{
    /// <summary>
    /// Handles movement Input
    /// 
    /// !!!Runs in Update!!!
    /// </summary>
    public void MovementInputHandler();

    /// <summary>
    /// Handles movement Implimentation
    /// 
    /// !!!Runs in FixedUpdate!!!
    /// </summary>
    public void MovementHandler();
}