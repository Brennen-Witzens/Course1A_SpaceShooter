using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _ammoAmountText;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Image _thrusterFuel;
    [SerializeField]
    private Text _thrusterCoolDownText;

    private Player _player;
    [SerializeField]
    private Sprite[] _livesSprites;

   

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("Player is NULL");
        }


        _thrusterFuel.color = Color.green;
        _scoreText.text = "Score: " + 0;
        _ammoAmountText.text = "Ammo Count: " + 15;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }
    }



    public void CheckScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        if(currentLives <= 3)
        {
            _livesImage.sprite = _livesSprites[currentLives];
        }
        if (currentLives < 1)
        {
            GameOverSequence();
        }
    }

    public void UpdateAmmo(int ammoCount)
    {
        _ammoAmountText.text = "Ammo Count: " + ammoCount;
    }


    public void UpdateThrusters()
    {

        _thrusterFuel.fillAmount -= 0.01f;

        if (_thrusterFuel.fillAmount < 1 && _thrusterFuel.fillAmount > .90)
        {
            _thrusterFuel.color = Color.green;
        }
        else if(_thrusterFuel.fillAmount < .89 && _thrusterFuel.fillAmount > .50)
        {
            _thrusterFuel.color = Color.yellow;
        }
        else
        {
            _thrusterFuel.color = Color.red;
        }


        if (_thrusterFuel.fillAmount == 0)
        {
            _player.ThrusterOnCooldown();
            _thrusterCoolDownText.gameObject.SetActive(true);
            StartCoroutine(ThurstersCoolDownRoutine());
        }
    }


    private void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverTextFlickerRoutine());
    }


    IEnumerator ThurstersCoolDownRoutine()
    {
        while (true)
        {
            
            _thrusterFuel.fillAmount += .05f + Time.deltaTime;
            yield return new WaitForSeconds(0.5f);
            _thrusterCoolDownText.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            _thrusterCoolDownText.color = Color.white;
            yield return new WaitForSeconds(1f);
            
            if(_thrusterFuel.fillAmount == 1)
            {
                _thrusterCoolDownText.gameObject.SetActive(false);
                _player.ThrusterCooldownReset();
                _thrusterFuel.color = Color.green;
                yield break;
            }
        }
       
    }


    IEnumerator GameOverTextFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "Game Over!";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);

        }
    }

}
