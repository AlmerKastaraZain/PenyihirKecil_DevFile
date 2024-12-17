using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.VFX;



public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public float RunCoolDown = 0.5f;
    public bool ableToMove = true;
    public Rigidbody2D rb;
    public LayerMask interactableLayer;
    public BoxCollider2D playerColider;
    public Vector2 velocity;
    public float UseEnergySpeed = 0.025f;
    Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    //FOG

    public VisualEffect vfxRenderer;


    // Control
    public void HandleUpdate()
    {
        UpdateVelocity();
        if (Input.GetKeyDown(KeyCode.E) && GameController.state == GameState.FreeRoam) Interact();
        if (Input.GetKey(KeyCode.LeftShift) && !(StaminaBar.instance.currentStamina <= 0) && !(velocity.x == 0 && velocity.y == 0))
        {
            moveSpeed = 5f;
            Run();
        }
        else
        {
            moveSpeed = 3.5f;
        }

        UpdateAnimation();
    }

    public void AddEnergy(float val)
    {
        StaminaBar.instance.AddStamina(val);
        StaminaBar.instance.RefreshStamina();
    }

    private float timer = 0;
    private void Run()
    {
        timer += Time.deltaTime;
        if (timer > UseEnergySpeed)
        {
            StaminaBar.instance.useStamina(1);
            timer = 0;
        }
    }

    private void UpdateAnimation()
    {
        if (velocity.sqrMagnitude > 0.01f) // Check if velocity is not zero
        {
            animator.SetFloat("horizontal", Input.GetAxisRaw("Horizontal"));
            animator.SetFloat("vertical", Input.GetAxisRaw("Vertical"));
        }
        else
        {
            animator.SetFloat("horizontal", 0);
            animator.SetFloat("vertical", 0);
        }
    }

    // Movement
    private void UpdateVelocity()
    {
        velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxis("Vertical"));
    }


    void FixedUpdate()
    {
        if (velocity.sqrMagnitude > 1)
        {
            velocity = velocity.normalized;
        }
        rb.MovePosition(rb.position + velocity * moveSpeed * Time.fixedDeltaTime);
        vfxRenderer.SetVector3("ColliderPos", transform.position);
    }

    // Interaction
    void Interact()
    {
        var collider = Physics2D.OverlapCircle(rb.position, 2f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    [SerializeField]
    private Music_MusicManager musicManager;
    private Collider2D musicCollider;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Music"))
        {
            musicCollider = collision;

            musicManager.UpdateMusic(musicCollider.gameObject.GetComponent<MusicClass>().audioClip);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (musicCollider != null && collision.CompareTag("Music"))
        {
            musicCollider = null;
        }
    }


}