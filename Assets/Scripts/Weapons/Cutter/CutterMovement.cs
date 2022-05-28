using UnityEngine;

public class CutterMovement : ProjectileMovement
{
    private GameObject player;
    private PoolController poolController;
    private Transform playerTransform;
    private VectorLerper lerp;
    private float tick;
    public CutterMovement() : base()
    {
    }

    private void Awake()
    {
        lerp = new VectorLerper();
        player = DoStatic.GetPlayer();
        playerTransform = player.transform;
    }
    protected override void Start()
    {
        base.Start();
        poolController = DoStatic.GetGameController<PoolController>();
    }

    protected override void Update()
    {

        if (lerp.isLerping)
        {
            lerp.Update(Time.deltaTime);
            transform.position = lerp.currentValue;
            tick = 3f;
        }

        else
        {
            transform.position = Vector3.Lerp(transform.position, playerTransform.position, 1 / (tick -= Time.deltaTime));
            gameObject.SetActive(tick > 0);
        }
        

        gameObject.SetActive((lifeTick -= Time.deltaTime) > 0);

        if (!gameObject.activeInHierarchy)
        {
            poolController.AddObjectIntoPool("CutterPool", gameObject);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (lerp != null)
        {
            lerp.SetValues(playerTransform.position, playerTransform.position + new Vector3(5, 0, 0), 1f);
        }
    }
}
