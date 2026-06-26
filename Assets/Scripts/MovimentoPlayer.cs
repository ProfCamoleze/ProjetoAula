using UnityEngine;

public class MovimentoPlayer : MonoBehaviour
{
    [Header("Movimento Horizontal")]
    [SerializeField] private float velocidade = 5f;

    [Header("Pulo")]
    [SerializeField] private float forcaPulo = 10f;
    [SerializeField] private int maxPulos = 2;     //2 = pulo duplo
    private int pulosDados;     //pulos ja dados
    [SerializeField] private float corteDoPulo = 0.5f;  // NOVO VARIAVEL

    [Header("Deteccao de Chao")]
    [SerializeField] private Transform pe;
    [SerializeField] private float raio = 0.2f;
    [SerializeField] private LayerMask camadaChao;

    private InputJogo controles;
    private Rigidbody2D rb;
    private Vector2 direcao;
    private bool noChao;

    private Animator anime;          // NOVO - o Animator do Player

    private void Awake()
    {
        controles = new InputJogo();
        rb = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();   // NOVO - pega o Animator do player
    }

    private void OnEnable() { controles.Player.Enable(); }
    private void OnDisable() { controles.Player.Disable(); }

    private void Update()
    {
        // Le a direcao do movimento horizontal (Vector2)
        direcao = controles.Player.Move.ReadValue<Vector2>();
        direcao = controles.Player.Move.ReadValue<Vector2>();

        // Physics2D verifica se ha chao sob o Pe
        noChao = Physics2D.OverlapCircle(pe.position, raio, camadaChao);
        //no chao, recupera todos os pulos
        if (noChao)
        {
            pulosDados = 1;
        }

        // pode pular enquanto nao atingiu o maximo de pulos
        if (controles.Player.Jump.WasPressedThisFrame() && pulosDados < maxPulos)
        {
            Pular();
        }
        // Novo VARIAVEL: soltou cedo enquanto sobe? Corta a subida
        if (controles.Player.Jump.WasReleasedThisFrame() && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * corteDoPulo);
        }
        Virar();    // NOVO - vira o personagem
        Animar();   // NOVO - atualiza as animacoes

    }

    private void FixedUpdate()
    {
        // Movimento horizontal: muda o X, mantem o Y (gravidade/pulo)
        rb.linearVelocity = new Vector2(direcao.x * velocidade, rb.linearVelocity.y);
    }

    private void Pular()
    {
        // Impulso para cima no eixo Y, mantendo o eixo X
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
        pulosDados++;    // NOVO - conta mais um pulo
    }
    private void Virar()
    {
        // direcao.x > 0 -> direita | direcao.x < 0 -> esquerda
        if (direcao.x > 0.1f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);    // direita
        }
        else if (direcao.x < -0.1f)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);  // esquerda
        }
    }

    private void Animar()
    {
        // 'andar' recebe a velocidade horizontal (sempre positiva)
        anime.SetFloat("andar", Mathf.Abs(rb.linearVelocityX));

        if (noChao)
        {
            // No chao: nao esta pulando nem caindo
            anime.SetBool("pular", false);
            anime.SetBool("cair", false);
        }
        else
        {
            if (rb.linearVelocityY > 0.1f)
            {
                // Subindo
                anime.SetBool("pular", true);
                anime.SetBool("cair", false);
            }
            else if (rb.linearVelocityY < -0.1f)
            {
                // Descendo
                anime.SetBool("pular", false);
                anime.SetBool("cair", true);
            }
        }
    }


}
