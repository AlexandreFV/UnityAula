using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimentar : MonoBehaviour
{
    public GameObject Personagem; // Referência ao personagem
    public float Speed = 2f; // Velocidade do personagem

    public Rigidbody2D rb; // Referência ao Rigidbody2D
    private Animator animator; // Referência ao Animator
    private float lastMoveX = 0f; // Última direção em X
    private float lastMoveY = 0f; // Última direção em Y

    void Start()
    {
        // Inicializa as referências
        rb = Personagem.GetComponent<Rigidbody2D>();
        animator = Personagem.GetComponent<Animator>();

        // Impede a rotação no eixo Z
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Movimentação usando física
        float moveX = 0f;
        float moveY = 0f;

        // Detecta as teclas pressionadas
        if (Input.GetKey(KeyCode.W)) moveY += 1f; // Cima
        if (Input.GetKey(KeyCode.S)) moveY -= 1f; // Baixo
        if (Input.GetKey(KeyCode.D)) moveX += 1f; // Direita
        if (Input.GetKey(KeyCode.A)) moveX -= 1f; // Esquerda

        // Armazena a última direção de movimento
        if (moveX != 0f || moveY != 0f)
        {
            lastMoveX = moveX;
            lastMoveY = moveY;
        }

        // Calcula o vetor de movimento
        Vector2 movimento = new Vector2(moveX, moveY).normalized * Speed;

        // Aplica o movimento ao Rigidbody2D
        rb.linearVelocity = movimento;

        // Atualiza os parâmetros do Animator
        animator.SetFloat("moveX", lastMoveX); // Usando as últimas direções armazenadas
        animator.SetFloat("moveY", lastMoveY); // Usando as últimas direções armazenadas

        // Define o estado de movimento (idle ou movimentando)
        animator.SetBool("isMoving", movimento.magnitude > 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Colisão com: {collision.gameObject.name}");
    }
}
