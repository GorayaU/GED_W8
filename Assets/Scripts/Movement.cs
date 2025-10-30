using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody _rb;

    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _rb.AddForce(Vector3.left * 100, ForceMode.Acceleration);
        }else if (Input.GetKeyDown(KeyCode.D))
        {
            _rb.AddForce(Vector3.right * 100, ForceMode.Acceleration);
        }
    }
}
