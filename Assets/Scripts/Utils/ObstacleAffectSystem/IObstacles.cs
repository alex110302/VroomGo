using UnityEngine;

public interface IObstacles<T> where T : Controller<T>
{
    public void CauseAffect(T controller);
}
