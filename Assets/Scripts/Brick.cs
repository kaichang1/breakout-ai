using System;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public static event Action<Brick> OnBrickDestruction;  // Subscribed by GameManager and LevelManager

    public int initialHp;

    internal Player _player;

    [SerializeField] private ParticleSystem _destructionEffect;

    private int _hp;
    private SpriteRenderer _sr;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Initialize the brick.
    /// </summary>
    /// <param name="owner">The player associated with the brick.</param>
    /// <param name="containerTransform">The player's bricks container.</param>
    /// <param name="sprite">Brick sprite image.</param>
    /// <param name="color">Brick color.</param>
    /// <param name="hitpoints">Brick hitpoints.</param>
    public void Init(Player owner, Transform containerTransform, Sprite sprite, Color color, int hitpoints)
    {
        _player = owner;
        transform.SetParent(containerTransform);
        _sr.sprite = sprite;
        _sr.color = color;
        initialHp = _hp = hitpoints;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _hp--;
        if (_hp <= 0)
        {
            // Destruction logic and invoke the OnBrickDestruction event
            _player._bricksCount--;
            OnBrickDestruction?.Invoke(this);
            ApplyDestructionEffect();
            Destroy(gameObject);
        }
        else
        {
            _sr.sprite = LevelManager.Instance.brickSprites[_hp - 1];
        }
    }

    /// <summary>
    /// Apply a particle system effect based on the color of the destroyed brick.
    /// </summary>
    private void ApplyDestructionEffect()
    {
        GameObject effect = Instantiate(_destructionEffect.gameObject, transform.position, Quaternion.identity);
        ParticleSystem.MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = _sr.color;
    }
}
