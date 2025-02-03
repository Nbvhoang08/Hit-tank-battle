using UnityEngine;
using UnityEngine.InputSystem;

public class TankController : Singleton<TankController>
{
   public float moveSpeed = 5f; // Tốc độ di chuyển
    private Joystick _joystick;
    private Rigidbody _rb;
    private Transform _targetEnemy;
    public Transform gun; // Gán Transform của súng vào đây
    public float rotationSpeed = 5f; // Tốc độ quay của súng
    public GameObject bulletPrefab; // Bullet Prefab
    public Transform bulletSpawnPoint; // Điểm bắn của súng (ở nòng súng)
    public float bulletSpeed = 10f; // Tốc độ bắn của đạn
    public ParticleSystem muzzleEffect; // Hiệu ứng muzzle (tia lửa từ nòng súng)
    public float timeBetweenShots = 0.5f; // Thời gian tối thiểu giữa mỗi lần bắn
    public float lastShotTime = 0f; // Thời gian lần bắn cuối cùng
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        // Tự động tìm Joystick trong scene
        _joystick = FindObjectOfType<Joystick>();

        // if (_joystick == null)
        // {
        //     Debug.LogError("Không tìm thấy Joystick! Hãy đảm bảo bạn có Joystick trong scene.");
        // }
    }
    void Update()
    {
        if (_joystick == null)
        {
            _joystick = FindObjectOfType<Joystick>();
            
        }
        FindNearestEnemy();
         // Tự động xoay súng về phía kẻ địch gần nhất
        
    }
    void FixedUpdate()
    {
        if (_joystick == null) return;

        // Lấy giá trị từ joystick
        float horizontal = _joystick.Horizontal;
        float vertical = _joystick.Vertical;

        // Vector di chuyển trên mặt phẳng XZ
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        // Di chuyển Player theo hướng joystick
        _rb.velocity = moveDirection * moveSpeed + new Vector3(0, _rb.velocity.y, 0);

        // Nếu joystick đang di chuyển, xoay Player theo hướng đó
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            _rb.rotation = Quaternion.Slerp(_rb.rotation, toRotation, Time.deltaTime * 3f);
        }
        if (_targetEnemy != null)
        {
            RotateGunTowardsEnemy();
        }
    }
    // Hàm tìm kẻ địch gần nhất
    void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Cả kẻ địch trong scene
        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        _targetEnemy = nearestEnemy; // Gán mục tiêu là kẻ địch gần nhất
    }

    // Hàm xoay súng về phía kẻ địch
    void RotateGunTowardsEnemy()
    {
        Vector3 directionToEnemy = _targetEnemy.position - gun.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);

        // Xoay súng từ góc hiện tại tới vị trí mục tiêu
        gun.rotation = Quaternion.Slerp(gun.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
        public void ShootBullet()
        {
             if (bulletPrefab != null && bulletSpawnPoint != null)
    {
        // Tạo đạn từ prefab tại điểm spawn
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Lấy Rigidbody của đạn và bắn nó về phía kẻ thù
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            // Tính hướng bắn từ súng đến kẻ thù
            Vector3 direction = (_targetEnemy.position - bulletSpawnPoint.position).normalized;

            // Đảm bảo đạn không thay đổi theo chiều cao (Y)
            direction.y = 0;

            // Gán tốc độ cho đạn
            bulletRb.velocity = direction * bulletSpeed;

            // Xoay đạn theo hướng của súng nhưng giữ nguyên tọa độ Y
            Quaternion bulletRotation = Quaternion.LookRotation(direction);
            bullet.transform.rotation = bulletRotation; // Xoay đạn về hướng bắn
            bullet.transform.position = new Vector3(bullet.transform.position.x, bulletSpawnPoint.position.y, bullet.transform.position.z); // Giữ nguyên Y
        }

        // Chạy hiệu ứng muzzle (tia lửa từ nòng súng)
        if (muzzleEffect != null)
        {
            muzzleEffect.Play(); // Bật hiệu ứng muzzle khi bắn
        }
    }
    }
}
