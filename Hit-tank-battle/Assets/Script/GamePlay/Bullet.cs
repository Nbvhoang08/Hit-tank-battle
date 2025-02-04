using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bulletType type;
    [SerializeField] private GameObject _explosionEffect; // Prefab hiệu ứng nổ
    [SerializeField] private float _lifetime = 3f; // Thời gian tồn tại tối đa của viên đạn

    private void Start()
    {
        // Tự hủy sau 3 giây nếu không va chạm
        Destroy(gameObject, _lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("Car") || other.CompareTag("Enemy") && type == bulletType.Player || other.CompareTag("Player") && type == bulletType.Enemy )
        {
            SpawnExplosion();
            Destroy(gameObject);
        }
    }

    private void SpawnExplosion()
    {
        if (_explosionEffect != null)
        {
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        }
    }
}
public enum bulletType
{
    Player,
    Enemy
}
