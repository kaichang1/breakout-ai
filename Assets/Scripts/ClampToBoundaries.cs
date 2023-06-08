using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ClampToBoundaries : MonoBehaviour
{
    private float _minX;
    private float _maxX;

    // Start is called before the first frame update
    void Start()
    {
        float objectWidthHalf = gameObject.GetComponent<SpriteRenderer>().bounds.extents.x;  // bounds.extents.x is half of the object's width
        float backgroundWidthHalf = transform.parent.Find("Background").GetComponent<SpriteRenderer>().bounds.extents.x;
        float sideWallWidth = transform.parent.Find("Walls").Find("WallsLeft").GetComponent<BoxCollider2D>().size.x;

        // Local position is affected by parent position
        // Clamping depends on parent position, background width, wall width, and object width
        _minX = transform.parent.position.x - backgroundWidthHalf + sideWallWidth + objectWidthHalf;
        _maxX = transform.parent.position.x + backgroundWidthHalf - sideWallWidth - objectWidthHalf;
    }

    // LateUpdate is called after all Update functions have been called
    void LateUpdate()
    {
        // Clamp the X position
        Vector2 newPosition = transform.position;
        newPosition.x = Mathf.Clamp(newPosition.x, _minX, _maxX);
        transform.position = newPosition;
    }
}
