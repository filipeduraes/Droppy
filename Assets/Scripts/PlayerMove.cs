using UnityEngine;
using UnityEngine.InputSystem; // precisa disso pro novo sistema de input

public class PlayerMove : MonoBehaviour
{
    private DroppyControls controls; // referência pro input
    private Vector2 moveInput;       // armazena o valor do input
    public float speed = 5f;         // velocidade horizontal

    private void Awake()
    {
        controls = new DroppyControls();

        // Quando a ação "Mover" for usada, pega o valor
        controls.Player.Mover.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Mover.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        // Movimento apenas no eixo X
        Vector3 movement = new Vector3(moveInput.x, 0, 0);
        transform.Translate(movement * speed * Time.deltaTime);
    }
}
