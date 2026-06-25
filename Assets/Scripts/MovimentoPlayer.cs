using UnityEngine;

public class MovimentoPlayer : MonoBehaviour
{
    public float velocidade = 5f;

    private InputJogo controles; // classe gerada
    private Rigidbody2D rb;      // fisica do Player
    private Vector2 direcao;     // direcao lida do Input

    private void Awake()
    {
        // Cria os controles e pega o Rigidbody 2D do objeto
        controles = new InputJogo();
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        // Liga o mapa Player para comecar a ler os comandos
        controles.Player.Enable();
    }

    private void OnDisable()
    {
        // Desliga o mapa quando o objeto e desativado
        controles.Player.Disable();
    }

    private void Update()
    {
        // Le a direcao atual do movimento vinda dos controles (Vector2) 
        direcao = controles.Player.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // Aplica a velocidade horizontal mantendo a vertical (gravidade)
        rb.linearVelocity = new Vector2(direcao.x * velocidade, rb.linearVelocity.y);
    }
}
