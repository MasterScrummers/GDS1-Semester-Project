using UnityEngine;

public class CutterMovement : ProjectileMovement
{
    private PoolController poolController;
    private Transform player;
    private Lerper lerp;
    private bool isComingBack = false;
    [SerializeField] float acceleration = 0.5f;

    private void Awake()
    {
        lerp = new();
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
        speed.value = lerp.currentValue;
        base.Update();

        if (!isComingBack)
        {
            isComingBack = !lerp.isLerping;
            if (isComingBack)
            {
                lerp.SetValues(0, speed.originalValue, 1 / acceleration);
            }
        } else
        {
            DoStatic.LookAt(transform, player.position);
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
        lerp.SetValues(speed.originalValue, 0, 1 / acceleration);
    }
}
