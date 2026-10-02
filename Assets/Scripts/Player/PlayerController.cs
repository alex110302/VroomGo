using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : Controller<PlayerController>
{
    //Player Object
    [field: SerializeField] public GameObject playerObj { get; private set; }
    
    //Game Objects
    [field: SerializeField] public new Camera camera { get; private set; }

    //Componets
    [field: SerializeField] public Rigidbody2D RB { get; private set; }
    [field: SerializeField] public BoxCollider2D BoxCollider { get; private set; }
    [field: SerializeField] public LayerMask BoundMask { get; private set; }

    //Attributes
    [field: SerializeField] public int Health { get; set; }
    [field: SerializeField] public float Speed { get; set; }


    //Input
    [field: SerializeField] public PlayerControlsInput PCI { get; private set; }

    //Scripts
    public Movement2D Movement { get; private set; }
    public new State<PlayerController> State { get; private set; }
    

    protected override void InitializeStates()
    {
        //State = new State<PlayerController>(this);   

        //! Initalize Statse...
    }

    private void Awake()
    {
        //Attributes.Speed = 10;


        PCI = new PlayerControlsInput();
        PCI.Enable();
    }

    private void Start()
    {
        Movement = new Movement2D(this, RB);

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
        PCI.Disable();
    }
}