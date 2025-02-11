using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // Importa o SceneManager

public class Minigame : MonoBehaviour
{
    //Image branca dentro do quadrado q trocara pelas sprites no sorteio
    public Image quadrado0; 
    public Image quadrado1;
    public Image quadrado2;

    //Conjunto de sprites corretas e incorretas q serão exibidas no sorteio
    public Sprite[] opcoesErradasSorteio;
    public Sprite[] opcoesCorretasSorteio;

    //Indice utilizado para saber em qual posicao de acerto ou erro estamos
    public int indexCorreta = 0;
    public int indexIncorreta = 0;

    private List<int> indicesDisponiveis = new List<int>();
    private int posicao;

    public GameObject Button0;
    public GameObject Button1;
    public GameObject Button2;

    public GameObject caixa1;
    public GameObject caixa2;
    public GameObject caixa3;

    public Animator Personagem;

    Button button0;
    Button button1;
    Button button2;

    public GameObject ButtonFinalizar;

    public AudioSource ControladorDeSom;
    public AudioClip SomAcerto;
    public AudioClip SomErro;
    public AudioClip somFinal;
    public AudioClip somSorteio;

    public TMPro.TextMeshProUGUI TextoDialogue;
    public string[] frasesCorretas;
    public string[] frasesIncorretas;
    public string[] frasesDicas;

    public string fraseInicial;

    private string[][] matrizDicasFrases;

    public int QuantidadeFrasesErroPorNivel;
    private string[][] matrizFrasesIncorretas; //Precisa ser uma matriz diferente dos acertos, visto que para um unico nivel pode ter varias frases de erro
    private int Erros = 0;
    private bool primeiraExec = true;

    private int indexDica = 0;

    private string[,] errosText = new string[20, 20];
    public int quantidadeNiveis = 0;
    public TMPro.TextMeshProUGUI textoNivelAtual;
    private int valorNivel = 0;
    // Start is called before the first frame update
    void Start()
    {
        alterarNivelAtualUI(-1);
        SetDialogues();
        ObterButtonsMinigame();
        DesabilitarButtonsMinigame();
        StartCoroutine(SortearAnimacao());
    }

        
    public void SortearItens(int indexCorreta)
    {
        int rnd = Random.Range(0, 3);

         // Limpa a lista de índices disponíveis
        indicesDisponiveis.Clear();
        
        for (int i = 0; i < opcoesErradasSorteio.Length; i++)
        {
            indicesDisponiveis.Add(i); // Adiciona todos os índices disponíveis
        }

        if (rnd == 0)
        {
            quadrado0.sprite = opcoesCorretasSorteio[indexCorreta];
            indexIncorreta = SortearIndiceIncorreto();
            quadrado1.sprite = opcoesErradasSorteio[indexIncorreta];
            indicesDisponiveis.Remove(indexIncorreta); // Remove o índice usado da lista
            indexIncorreta = SortearIndiceIncorreto();
            quadrado2.sprite = opcoesErradasSorteio[indexIncorreta];

        }
        else if (rnd == 1)
        {
            quadrado1.sprite = opcoesCorretasSorteio[indexCorreta];
            indexIncorreta = SortearIndiceIncorreto();
            quadrado0.sprite = opcoesErradasSorteio[indexIncorreta];
            indicesDisponiveis.Remove(indexIncorreta); // Remove o índice usado da lista
            indexIncorreta = SortearIndiceIncorreto();
            quadrado2.sprite = opcoesErradasSorteio[indexIncorreta];

        }
        else if (rnd == 2)
        {
            quadrado2.sprite = opcoesCorretasSorteio[indexCorreta];
            indexIncorreta = SortearIndiceIncorreto();
            quadrado1.sprite = opcoesErradasSorteio[indexIncorreta];
            indicesDisponiveis.Remove(indexIncorreta); // Remove o índice usado da lista
            indexIncorreta = SortearIndiceIncorreto();
            quadrado0.sprite = opcoesErradasSorteio[indexIncorreta];

        }

        posicao = rnd;
    }

    public void SetDialogues()
    {
        int contador = 0; // Inicia o contador
        int indexDicaLocal = 0; // Variável para controlar o índice da linha

        // Itera pelas frases incorretas
        for (int i = 0; i < frasesIncorretas.Length; i++)
        {
            // Atribui as frases nas linhas e colunas apropriadas da matriz
            errosText[indexDicaLocal, contador] = frasesIncorretas[i];

            // Incrementa o contador (coluna)
            contador++;

            // Verifica se o contador ultrapassou o número de erros por nível
            if (contador >= QuantidadeFrasesErroPorNivel)
            {
                // Se sim, reinicia o contador e vai para a próxima linha
                contador = 0;
                indexDicaLocal++;
            }

            // Certifique-se de não ultrapassar a quantidade de linhas da matriz
            if (indexDicaLocal >= quantidadeNiveis)
            {
                break; // Interrompe o loop se as linhas estiverem completas
            }
        }
    }


    //Verifica quais indices n foram usados para evitar duplicidade de sprites
    private int SortearIndiceIncorreto()
    {
        int index = Random.Range(0, indicesDisponiveis.Count);
        int indiceEscolhido = indicesDisponiveis[index];
        return indiceEscolhido;
    }

    public void CompararPosicao(int buttonApertado)
    {
        if (posicao == buttonApertado)
        {
            DesabilitarButtonsMinigame();
            AcertouMinigame();
        } else if (posicao != buttonApertado) {
            DesabilitarButtonsMinigame();
            ErrouMinigame();
        }
    }

    IEnumerator SortearAnimacao()
    {
        if (primeiraExec)
        {
            MostrarFraseInicial();
            primeiraExec = false;
        }

        float intervaloEntreSorteios = 0.1f; // Intervalo entre os sorteios
        float tempoDecorrido = 0f;

        // Define o som do sorteio e reproduz
        ControladorDeSom.clip = somSorteio;
        ControladorDeSom.Play();

        // Enquanto o tempo decorrido for menor que a duração do áudio
        //4f é o tamanaho do clip, n peguei diretamente o tamanho do clip pq ele tem uma parte sem som
        while (tempoDecorrido < 4f)
        {
            SortearItens(indexCorreta); // Chama a função de sorteio
            yield return new WaitForSeconds(intervaloEntreSorteios); // Espera o intervalo antes de repetir
            tempoDecorrido += intervaloEntreSorteios;
        }
    }


    private void AcertouMinigame()
    {
            // Verifica se já ultrapassou o número de opções corretas
            if (indexCorreta >= opcoesCorretasSorteio.Length - 1)
            {   alterarNivelAtualUI(valorNivel);
                TerminouMinigame();  // Chama a função quando terminar
                return;  // Impede que o jogo continue após o término
            }

            Erros = 0;
            StartCoroutine(MostrarAnimacaoAcerto());
            ControladorDeSom.clip = SomAcerto;
            ControladorDeSom.Play();
            StartCoroutine(MostrarFraseAcerto());

            StartCoroutine(mostrarFraseDica(5f));
            indexCorreta++;
            alterarNivelAtualUI(valorNivel);
    }

    private int alterarNivelAtualUI(int valor)
    {
        valor = valor + 1;
        Debug.Log("Valor " + valor);
        textoNivelAtual.SetText(valor + "/" + quantidadeNiveis);
        return valor;
    }
    private void ErrouMinigame()
    {
            ControladorDeSom.clip = SomErro;
            ControladorDeSom.Play();
            StartCoroutine(MostrarFraseErro());
    }

    private void TerminouMinigame()
    {
        DesabilitarButtonsMinigame();
        ButtonFinalizar.SetActive(true);
        ControladorDeSom.clip = somFinal;
        ControladorDeSom.Play();
    }

    private void ObterButtonsMinigame()
    {
         button0 = Button0.GetComponent<Button>();
         button1 = Button1.GetComponent<Button>();
         button2 = Button2.GetComponent<Button>();
    }

    private void DesabilitarButtonsMinigame()
    {
            button0.interactable = false;
            button1.interactable = false;
            button2.interactable = false;
    }
    
    private void HabilitarButtonsMinigame()
    {
            button0.interactable = true;
            button1.interactable = true;
            button2.interactable = true;
    }

    private void MostrarFraseInicial()
    {   
        DesabilitarButtonsMinigame();
        TextoDialogue.SetText(fraseInicial);
        StartCoroutine(mostrarFraseDica(4f));
    }

    private IEnumerator mostrarFraseDica(float duracao)
    {
        yield return new WaitForSeconds(duracao);
        TextoDialogue.SetText(frasesDicas[indexCorreta]);
        HabilitarButtonsMinigame();
    }

    private IEnumerator MostrarFraseAcerto()
    {
        TextoDialogue.SetText(frasesCorretas[indexCorreta]);
        Debug.Log("Valor indexCorreta: " + indexCorreta);
        yield return new WaitForSeconds(1f);
        if(indexCorreta < opcoesCorretasSorteio.Length){StartCoroutine(SortearAnimacao());}
    }


    private IEnumerator MostrarFraseErro()
    {
        if(Erros == QuantidadeFrasesErroPorNivel - 1)
        {
        TextoDialogue.SetText(errosText[indexCorreta,Erros]);
        Erros = 0;
        } else {
        TextoDialogue.SetText(errosText[indexCorreta,Erros]);
        Erros++;
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(SortearAnimacao());
        yield return new WaitForSeconds(4f);
        HabilitarButtonsMinigame();
    }

    private IEnumerator MostrarAnimacaoAcerto()
    {
        Personagem.SetInteger("nivelAtual", indexCorreta);
        Personagem.SetBool("isAcerto", true);
        yield return null;

        AnimatorStateInfo stateInfo = Personagem.GetCurrentAnimatorStateInfo(0);
    
        // Obtém a duração do estado atual
        float duracaoAnimacao = stateInfo.length;
        yield return new WaitForSeconds(duracaoAnimacao);

        Personagem.SetInteger("nivelAtual", 0);
        Personagem.SetBool("isAcerto", false);

    }

    public void VoltarParaSceneAnterior()
    {
        SceneManager.LoadScene("Cenario2");
    }

}