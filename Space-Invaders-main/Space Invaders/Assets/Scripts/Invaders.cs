using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InvadersGrid : MonoBehaviour
{
    // Declaração das variáveis públicas
    public Invader[] prefabs;
    public Projectile missilePrefab;
    public int rows = 5;
    public int columns = 11;
    public AnimationCurve speed;
    public float MissileAttackRate = 2.0f;
    public AudioClip deathSound;
    public TextMeshProUGUI scoreText;

    public int amountKilled { get; private set;}
    public int amountAlive => this.totalInvaders - this.amountKilled;
    public int totalInvaders => this.rows * this.columns;
    public float percentKilled => (float)this.amountKilled / (float)this.totalInvaders;
    public float horizontalSpacing = 1f;
    public float verticalSpacing = 0f;

    private Vector3 _direction = Vector2.right;
    private AudioSource _audioSource;
    private int score;
    private Vector3[,] initialPositions;
    private Vector3 startPosition; 

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        float horizontalSpacing = 2.5f;
        float verticalSpacing = 1.7f;

        initialPositions = new Vector3[rows, columns];

        startPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.8f, 10.0f));

        for (int row = 0; row < this.rows; row++)
        {
            float width = (this.columns - 1) * horizontalSpacing;
            float height = (this.rows - 1) * verticalSpacing;

            Vector2 centering = new Vector2(-width / 2, -height / 2);
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (row * verticalSpacing), 0.0f);

            for (int column = 0; column < this.columns; column++)
            {
                Invader i = Instantiate(this.prefabs[row % this.prefabs.Length], this.transform);
                i.invaderDestroyed += OnInvaderKilled;
                i.row = row;
                Vector3 position = rowPosition;
                position.x += column * horizontalSpacing;
                i.transform.localPosition = position;

                initialPositions[row, column] = position;
            }
        }
    }

    private void Start()
    {
        score = 0;
        UpdateScore(score);
        InvokeRepeating(nameof(MissileAttack), this.MissileAttackRate, this.MissileAttackRate);
    }

    private void Update()
    {
        this.transform.position += _direction * this.speed.Evaluate(this.percentKilled) * Time.deltaTime;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        
        foreach (Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy) {
                continue;
            }

            if (_direction == Vector3.right && invader.position.x >= rightEdge.x - 1.0f) 
            {
                AdvanceRow();
            } else if (_direction == Vector3.left && invader.position.x <= leftEdge.x + 1.0f) 
            {
                AdvanceRow();
            }
        }
    }

    private void AdvanceRow()
    {
        _direction.x *= -1.0f;

        Vector3 position = this.transform.position;
        position.y -= 1.0f;
        this.transform.position = position;
    }

    private void MissileAttack()
    {
        foreach (Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy) {
                continue;
            }

            if (Random.value < (1.0f / (float)this.amountAlive))
            {
                Instantiate(this.missilePrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }

    private void OnInvaderKilled(int row)
    {
        _audioSource.PlayOneShot(this.deathSound);

        amountKilled++;

        switch(row)
        {
            case 4:
                score += 40;
                break;
            case 3:
            case 2:
                score += 20;
                break;
            case 1:
            case 0:
                score += 10;
                break;
        }

        UpdateScore(score);

        if (this.amountKilled >= this.totalInvaders)
        {   
            ResetInvaders();
        }
    }


    private void ResetInvaders()
    {
    
        this.transform.position = startPosition;

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                int index = row * columns + column;
                Transform invader = transform.GetChild(index);
                invader.gameObject.SetActive(true);
                invader.localPosition = initialPositions[row, column];
                invader.GetComponent<SpriteRenderer>().sprite = prefabs[row % prefabs.Length].GetComponent<SpriteRenderer>().sprite;
            }
        }

        amountKilled = 0;
        UpdateScore(score);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader") || 
            other.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            Debug.Log("You lose!");
        }
    }

    private void UpdateScore(int pontos)
    {
        scoreText.text = "Score: " + score.ToString();

        if (score % 1000 == 0)
        {
            //Time.timeScale = 0;
           // SceneManager.LoadScene("WinGame");
        }
    }
}
