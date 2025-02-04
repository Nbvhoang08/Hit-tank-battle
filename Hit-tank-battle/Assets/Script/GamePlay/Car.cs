
using UnityEngine;

public class Car : MonoBehaviour
{
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Invoke(nameof(SetKinematic), 0.5f);
    }

    private void SetKinematic()
    {
        if (_rb != null)
        {
            _rb.isKinematic = true;
        }
    }
}
