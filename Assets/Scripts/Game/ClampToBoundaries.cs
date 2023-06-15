using UnityEngine;

public class ClampToBoundaries : MonoBehaviour
{
    internal float _minX;
    internal float _maxX;

    private void Awake()
    {
        float objectWidthHalf = gameObject.GetComponent<SpriteRenderer>().bounds.extents.x;  // bounds.extents.x is half of the object's width
        float backgroundWidthHalf = transform.parent.Find("Background").GetComponent<SpriteRenderer>().bounds.extents.x;
        float sideWallWidth = transform.parent.Find("Walls").Find("WallsLeft").GetComponent<BoxCollider2D>().size.x;

        // Local position is affected by parent position
        // Clamping depends on parent position, background width, wall width, and object width
        _minX = transform.parent.position.x - backgroundWidthHalf + sideWallWidth + objectWidthHalf;
        _maxX = transform.parent.position.x + backgroundWidthHalf - sideWallWidth - objectWidthHalf;
    }

    void LateUpdate()
    {
        // Clamp the X position
        Vector2 newPosition = transform.position;
        newPosition.x = Mathf.Clamp(newPosition.x, _minX, _maxX);
        transform.position = newPosition;
    }
}
