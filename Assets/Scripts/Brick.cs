using System;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public static event Action<Brick> OnBrickDestruction;

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
    /// <param name="containerTransform">The transform component of the parent container.</param>
    /// <param name="sprite">The brick's sprite image.</param>
    /// <param name="color">The brick's color.</param>
    /// <param name="hitpoints">The brick's hitpoints.</param>
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
    /// Applies a particle system effect based on the color of the destroyed brick.
    /// </summary>
    private void ApplyDestructionEffect()
    {
        GameObject effect = Instantiate(_destructionEffect.gameObject, transform.position, Quaternion.identity);
        ParticleSystem.MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = _sr.color;
    }
}
