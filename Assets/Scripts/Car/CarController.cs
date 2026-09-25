using UnityEngine;

public class CarController : Controller<CarController>
{
    [field: SerializeField] public Rigidbody2D RB {  get; private set; }
    [field: SerializeField] public float Speed { get; set; }
    public CarMovement Movement { get; private set; }
    public new State<CarController> State { get; private set; }


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
}
