using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float offsetY = 0.3f;
    public FloatingJoystick Movejoystick;
    public float moveSpeed = 5f;
    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Input
        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");
        movement.x = Movejoystick.Horizontal;
        movement.y = Movejoystick.Vertical;

        // Animation
        if (movement != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
            animator.SetFloat("moveX", movement.x);
            animator.SetFloat("moveY", movement.y);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    void FixedUpdate()
    {
        // Movement
        Vector2 targetPos = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
        if (isWalkable(targetPos))
        {
            rb.MovePosition(targetPos);
        }
    }

    private bool isWalkable(Vector2 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.15f, solidObjectsLayer) != null)
        {
            return false;
        }

        return true;
    }

    private void OnMoveOver()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position - new Vector3(0, offsetY), 0.2f, GameLayers.i.TriggerableLayers);

        foreach (var collider in colliders)
        {
            var triggerable = collider.GetComponent<IPlayerTriggerable>();
            if (triggerable != null)
            {
                triggerable.OnPlayerTriggered(this);
                break;
            }
        }
    }

}
