using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    Result result;

    [SerializeField] private bool ground;
    [SerializeField] private float distance = 0.055f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpSpeed = 10f;
    private float horiInput, verInput;
    private Vector3 moveDir;
    private Vector3 moveInput;
    private Vector3 movement;

    private bool done;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        result = FindObjectOfType<Result>(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (done) return;
        if (Fall()) Die();

        ground = IsGrounded();
        MoveInput();
    }

    private void MoveInput()
    {
        horiInput = Input.GetAxis("Horizontal");
        verInput = Input.GetAxis("Vertical");
        moveInput = new Vector3(horiInput, 0, verInput);
        movement = (moveDir != Vector3.zero) ? moveDir : moveInput;

        if (movement.magnitude > 1) movement.Normalize();

        if (movement.magnitude > 0)
            rb.velocity = new Vector3(movement.x * moveSpeed, rb.velocity.y, movement.z * moveSpeed);
        else rb.velocity = Vector3.up * rb.velocity.y;

        if ((Input.GetKeyDown(KeyCode.Space) || moveDir.y > 0) && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
        }
    }

    public void SetMoveDir(Vector3 dir) => moveDir = dir;

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector2.down, distance, LayerMask.GetMask("Ground"));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * distance);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Star"))
        {
            SoundManager18.Instance.PlaySound(5);
            result.IncreaseStar(1);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Finish"))
        {
            done = true;
            other.GetComponent<Collider>().enabled = false;
            rb.isKinematic = true;
            StartCoroutine(Winner());
        }
    }

    private IEnumerator Winner()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Win");
        result.ShowStar();
        GameManager18.Instance.GameWin();
    }

    private bool Fall()
    {
        return Physics.Raycast(transform.position, Vector2.down, distance, LayerMask.GetMask("Plane"));
    }

    private void Die()
    {
        done = true;
        GameManager18.Instance.GameLose();
    }

}
