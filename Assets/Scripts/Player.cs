using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour

{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Health")]
    public int maxHealth = 3;

    [Header("References")]
    public Transform cameraTransform;

    private int currentHealth;
    private float velocityY;
    private Vector2 moveInput;
    private CharacterController controller;
    private PlayerInputActions input;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = new PlayerInputActions();
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        input.Player.Enable();
        input.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        input.Player.Jump.performed += ctx => Jump();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);
        direction = cameraTransform.TransformDirection(direction);
        direction.y = 0;

        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        controller.Move(direction * moveSpeed * Time.deltaTime);

        if (controller.isGrounded && velocityY < 0)
            velocityY = -2f;

        velocityY += gravity * Time.deltaTime;
        controller.Move(Vector3.up * velocityY * Time.deltaTime);
    }

    private void Jump()
    {
        if (controller.isGrounded)
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    public void TakeDamage()
    {
        currentHealth--;
        Debug.Log("Игрок получил урон. Осталось жизней: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Игрок умер.");
        Destroy(gameObject);
    }
}
