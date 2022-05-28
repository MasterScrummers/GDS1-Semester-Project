using UnityEngine;

public class CutterMovement : ProjectileMovement
{
    private PoolController poolController;
    private Transform player;
    private Lerper lerp;
    private float originalSpd;
    private bool isComingBack = false;
    [SerializeField] float acceleration = 0.5f;

    private void Awake()
    {
        lerp = new();
        originalSpd = speed;
    }

    protected override void Start()
    {
        base.Start();
        player = DoStatic.GetPlayer<Transform>();
        poolController = DoStatic.GetGameController<PoolController>();
    }

    protected override void Update()
    {
        lerp.Update(Time.deltaTime);
        rb.velocity = lerp.currentValue * transform.right; //Allow the object to move in the direction it is facing.
        if (!isComingBack)
        {
            isComingBack = !lerp.isLerping;
            if (isComingBack)
            {
                lerp.SetValues(0, originalSpd, 1 / acceleration);
            }
        } else
        {
            transform.rotation = DoStatic.LookAt(transform.position, player.position);
            if (Vector3.Distance(transform.position, player.position) < 0.5f)
            {
                poolController.AddObjectIntoPool("CutterPool", gameObject);
                gameObject.SetActive(false);
            }
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        isComingBack = false;
        lerp.SetValues(originalSpd, 0, 1 / acceleration);
    }
}
