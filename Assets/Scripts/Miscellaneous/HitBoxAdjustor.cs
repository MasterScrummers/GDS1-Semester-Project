using UnityEngine;

public class HitBoxAdjustor : MonoBehaviour
{
    private GameObject hitDetector; //The child GameObject

    private BoxCollider2D box; //Adjustable box collider
    private CircleCollider2D circle; //Adjustable circle collider
    private EdgeCollider2D edge; //Adjustable edge collider
    private PolygonCollider2D polygon; //Adjustable polygon collider
    private CapsuleCollider2D capsule; //Adjustable capsule collider

    private Collider2D prevCollider;

    void Start()
    {
        hitDetector = new GameObject("HitBox");
        hitDetector.transform.parent = transform;

        box = hitDetector.AddComponent<BoxCollider2D>();
        circle = hitDetector.AddComponent<CircleCollider2D>();
        edge = hitDetector.AddComponent<EdgeCollider2D>();
        polygon = hitDetector.AddComponent<PolygonCollider2D>();
        capsule = hitDetector.AddComponent<CapsuleCollider2D>();

        foreach (Collider2D collider in new Collider2D[] { box, circle, edge, polygon, capsule })
        {
            collider.enabled = false;
            collider.isTrigger = true;
        }
    }

    private void UpdateCollider(Collider2D newCollider, Vector2 offset)
    {
        prevCollider.enabled = false;
        prevCollider = newCollider;
        prevCollider.enabled = true;
        prevCollider.offset = offset;
    }

    /// <summary>
    /// Should be called from the animation.
    /// Controls the box collider.
    /// </summary>
    /// <param name="offset">The (x, y) offset of the collider</param>
    /// <param name="size">The size of the collider</param>
    /// <param name="edgeRadius">The edge radius of the collider</param>
    public void UpdateBox(Vector2 offset, Vector2 size, float edgeRadius)
    {
        UpdateCollider(box, offset);
        box.size = size;
        box.edgeRadius = edgeRadius;
    }

    /// <summary>
    /// Should be called from the animation.
    /// Controls the circle collider.
    /// </summary>
    /// <param name="offset">The (x, y) offset of the collider</param>
    /// <param name="size">The size of the collider</param>
    public void UpdateCircle(Vector2 offset, float size)
    {
        UpdateCollider(circle, offset);
        circle.radius = size;
    }

    /// <summary>
    /// Should be called from the animation.
    /// Controls the edge collider.
    /// </summary>
    /// <param name="offset">The (x, y) offset of the collider</param>
    /// <param name="edgeRadius">The edge radius of the collider</param>
    /// <param name="points">The points of the collider.</param>
    public void UpdateEdge(Vector2 offset, float edgeRadius, Vector2[] points)
    {
        UpdateCollider(edge, offset);
        edge.edgeRadius = edgeRadius;
        edge.points = points;
    }

    /// <summary>
    /// Should be called from the animation.
    /// Controls the polygon collider.
    /// </summary>
    /// <param name="offset">The (x, y) offset of the collider</param>
    /// <param name="points">The points of the collider.</param>
    public void UpdatePolygon(Vector2 offset, Vector2[] points)
    {
        UpdateCollider(polygon, offset);
        polygon.points = points;
    }

    /// <summary>
    /// Should be called from the animation.
    /// Controls the capsule collider.
    /// </summary>
    /// <param name="offset">The (x, y) offset of the collider</param>
    /// <param name="size">The size of the collider</param>
    /// <param name="capsuleDirection">The direction of the capsule</param>
    public void UpdateCapsule(Vector2 offset, Vector2 size, CapsuleDirection2D capsuleDirection)
    {
        UpdateCollider(capsule, offset);
        capsule.size = size;
        capsule.direction = capsuleDirection;
    }
}
