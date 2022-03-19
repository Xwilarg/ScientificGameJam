using ScientificGameJam.Player;
using System.Collections;
using TMPro;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Text displaying the countdown when the race start
    /// </summary>
    [SerializeField]
    private TMP_Text _raceCountdown;

    /// <summary>
    /// Current time of the player in the race
    /// </summary>
    [SerializeField]
    private TMP_Text _mainTimer;

    /// <summary>
    /// Reference to the player
    /// </summary>
    [SerializeField]
    private PlayerController _player;

    private float _raceTimer;
    private bool _didRaceStart;

    private void Start()
    {
        _player.CanMove = false;
        _raceCountdown.gameObject.SetActive(true);
        _mainTimer.text = "0.00";
        StartCoroutine(LaunchRaceCountdown());
    }

    private void Update()
    {
        if (_didRaceStart)
        {
            _raceTimer += Time.deltaTime;
            _mainTimer.text = $"{_raceTimer:0.00}";
        }
    }

    public void EndRace()
    {
        _didRaceStart = false;
    }

    private IEnumerator LaunchRaceCountdown()
    {
        _raceCountdown.text = "3";
        yield return new WaitForSeconds(1f);
        _raceCountdown.text = "2";
        yield return new WaitForSeconds(1f);
        _raceCountdown.text = "1";
        yield return new WaitForSeconds(1f);
        _raceCountdown.gameObject.SetActive(false);
        _player.CanMove = true;
        _didRaceStart = true;
    }
}