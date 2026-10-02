using System.Collections;
using UnityEngine;

public class CarController : Controller<CarController>, IObstacles<PlayerController>
{
    [field: SerializeField] public Rigidbody2D RB {  get; private set; }
    [field: SerializeField] public float Speed { get; set; }
    public CarMovement Movement { get; private set; }
    public new State<CarController> State { get; private set; }
    public Road Road { get; set; }

    protected override void InitializeStates()
    {
        State = new State<CarController>(this);
    }

    private void Awake()
    {
        Movement = new CarMovement(this, RB);

        InitializeStates();
    }

    private void Update()
    {
        State?.Execute();
        Movement.MovementInputHandler();
    }

    private void FixedUpdate()
    {
        State?.FixedExecute();
        Movement.MovementHandler();
    }

    private void OnDestroy()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") CauseAffect(collision.GetComponent<PlayerController>());
    }

    public void CauseAffect(PlayerController controller)
    {
        controller.Health -= 1;

        StartCoroutine(SlowPlayer(controller));
    }

    //TODO If Player continues to hit a car it will slow down more and more untill the roads movement is revers
    //TODO Same with the players speed 
    //? we need some sort of locking system but im just to tired rn to write it lol
    private IEnumerator SlowPlayer(PlayerController controller)
    {

        int tempRoadSpeed = Road.Spawner.RoadSpeed;
        float tempSpeed = controller.Speed;

        Road.Spawner.RoadSpeed -= 3;
        controller.Speed -= 1.5f;

        yield return new WaitForSeconds(5);

        Road.Spawner.RoadSpeed = tempRoadSpeed;
        controller.Speed = tempSpeed;
    }    
}