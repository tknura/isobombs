using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    private enum ControlMode {
        Grid,
        Global
    }
    private enum PlayerNumber {
        P1,
        P2,
        P3,
        P4
    }
    [SerializeField] private ControlMode controlMode = ControlMode.Global;
    [SerializeField] private PlayerNumber playerNumber = PlayerNumber.P1;

    [SerializeField] private float gridSize = 1f;
    [SerializeField] private float moveSpeed = 2;

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject bomb;

    private List<Collider> m_collisions = new List<Collider>();

    private PlayerStatistics stats;

    private float currentV = 0;
    private float currentH = 0;
    private float interpolation = 10;

    private bool isGrounded;

    private Vector3 currentDirection = Vector3.zero;
    private Vector2 movementInput;

    private PlayerInput playerInput;

    private void Awake() {
        stats = this.gameObject.GetComponent<PlayerStatistics>();
        if (playerInput == null) {
            playerInput = GetComponent<PlayerInput>();
        }
    }

    private void FixedUpdate() {
        animator.SetBool("Grounded", isGrounded);
        switch (controlMode) {
            case ControlMode.Global:
                GlobalMovementUpdate();
                break;

            case ControlMode.Grid:
                GridMovementUpdate();
                break;

            default:
                Debug.LogError("Unsupported state");
                break;
        }
    }

    private void GlobalMovementUpdate() {
        float v = movementInput.x;
        float h = movementInput.y;

        if (Mathf.Abs(v) > Mathf.Abs(h)) {
            h = 0;
        }
        else {
            v = 0;
        }

        currentV = Mathf.Lerp(currentV, v, Time.deltaTime * interpolation);
        currentH = Mathf.Lerp(currentH, h, Time.deltaTime * interpolation);

        Vector3 direction = Vector3.forward * currentV + Vector3.right * currentH;

        float directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * directionLength;

        if (direction != Vector3.zero) {
            currentDirection = Vector3.Slerp(currentDirection, direction, Time.deltaTime * interpolation);

            transform.rotation = Quaternion.LookRotation(currentDirection);
            transform.position += currentDirection * moveSpeed * Time.deltaTime;

            animator.SetFloat("MoveSpeed", direction.magnitude);
        }
    }

    private void SpawnBombTrigger() {
        if (stats.GetBombAmount() > 0) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f)) {
                SpawnBomb(hit.collider.gameObject.transform.position + new Vector3(0, 1, 0));
            }
        }
    }

    private void GridMovementUpdate() {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(v) > Mathf.Abs(h)) {
            h = 0;
        }
        else {
            v = 0;
        }

        currentV = Mathf.Lerp(currentV, v, Time.deltaTime * interpolation);
        currentH = Mathf.Lerp(currentH, h, Time.deltaTime * interpolation);

        Vector3 direction = Vector3.forward * currentV + Vector3.right * currentH;


        direction.y = 0;
        Debug.Log("dir = " + direction);

        if (direction != Vector3.zero) {
            currentDirection = Vector3.Slerp(currentDirection, direction, Time.deltaTime * interpolation);
            Debug.Log("currdir = " + currentDirection);
            transform.rotation = Quaternion.LookRotation(currentDirection);
            Vector3 newPos = transform.position;
            newPos.x = Mathf.Floor((transform.position.x + currentDirection.x) / gridSize) * gridSize;
            newPos.z = Mathf.Floor((transform.position.z + currentDirection.z) / gridSize) * gridSize;

            transform.position = newPos;
            Debug.Log(newPos);
            animator.SetFloat("MoveSpeed", direction.magnitude);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++) {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f) {
                if (!m_collisions.Contains(collision.collider)) {
                    m_collisions.Add(collision.collider);
                }
                isGrounded = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision) {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++) {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f) {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal) {
            isGrounded = true;
            if (!m_collisions.Contains(collision.collider)) {
                m_collisions.Add(collision.collider);
            }
        }
        else {
            if (m_collisions.Contains(collision.collider)) {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) {
                isGrounded = false;
            }
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (m_collisions.Contains(collision.collider)) {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) {
            isGrounded = false;
        }
    }

    public void OnMove(InputValue value) {
        movementInput = value.Get<Vector2>();
    }

    public void OnBombDrop() {
        Debug.Log("Bomb is dropping!");
        SpawnBombTrigger();
        Debug.Log("Bombed Dropped!");
    }

    private void SpawnBomb(Vector3 position) {
        Instantiate(bomb, position, bomb.transform.rotation);
        stats.DropBomb();
    }
}

