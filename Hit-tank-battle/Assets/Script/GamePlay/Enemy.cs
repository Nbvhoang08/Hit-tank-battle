using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
     [Header("AI Settings")]
    [SerializeField] private float _detectionRange = 20f;
    [SerializeField] private float _shootRange = 15f;
    [SerializeField] private float _fireRate = 3f; // Thời gian giữa mỗi lần bắn
    [SerializeField] private LayerMask _obstacleLayer;
    [SerializeField] private LayerMask _enemyLayer;

    [Header("References")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _bulletPrefab;

    [SerializeField] private Transform _player;
    [SerializeField] private NavMeshAgent _agent;
    private bool _canShoot = true;
    public float Hp;
    public float MaxHp;
    private void Start()
    {
        _player = TankController.Instance?.transform;
        _agent = GetComponent<NavMeshAgent>();
        _agent.avoidancePriority = Random.Range(0, 100); // Mức độ ưu tiên ngẫu nhiên để tránh va chạm
        _agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        _agent.SetDestination(_player.position);
        Hp = MaxHp;
    }

    private void Update()
    {
        if (_player == null) return;
        _agent.SetDestination(_player.position);
        RotateTowardsPlayer();
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
        if (distanceToPlayer <= _detectionRange)
        {
            //Debug.Log(CanSeePlayer());
            if (distanceToPlayer <= _shootRange && CanSeePlayer())
            {
                // _agent.isStopped = true; // Dừng di chuyển
                
                if (_canShoot)
                {
                    StartCoroutine(ShootCooldown());
                }
            }
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = (_player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private bool CanSeePlayer()
    {
        
        // Tính toán hướng từ _firePoint đến _player
        Vector3 directionToPlayer = (_player.position - _firePoint.position).normalized;

        // Kiểm tra xem tia có va chạm với vật cản trong phạm vi _shootRange
        if (Physics.Raycast(_firePoint.position, directionToPlayer, out RaycastHit hit, _shootRange, _obstacleLayer))
        {
            // Nếu vật thể va chạm thuộc lớp "Player", Player có thể thấy
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                return true;  // Player có thể thấy
            }

            // Nếu vật thể va chạm không phải là Player, trả về false
            return false;  // Player bị che khuất bởi vật cản
        }

        // Nếu không có vật cản nào trong phạm vi bắn, Player có thể thấy
        return true;
    }

    private IEnumerator ShootCooldown()
    {
        _canShoot = false;
        Shoot();
        yield return new WaitForSeconds(_fireRate);
        _canShoot = true;
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = _firePoint.forward * 20f; // Điều chỉnh tốc độ viên đạn
        }
    }
      void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        {
            if(Hp>0)
            {
                Hp --;
            }else
            {
                Destroy(gameObject);
                Subject.NotifyObservers("enemyDie");
            }
            
        }
    }
}
