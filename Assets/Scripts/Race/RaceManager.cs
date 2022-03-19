using ScientificGameJam.Player;
using ScientificGameJam.SaveData;
using ScientificGameJam.Translation;
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
    /// Current and best time of the player in the race
    /// </summary>
    [SerializeField]
    private TMP_Text _mainTimer, _bestTimer;

    /// <summary>
    /// Reference to the player
    /// </summary>
    [SerializeField]
    private PlayerController _player;

    public float RaceTimer { private set; get; }
    private bool _didRaceStart;

    private void Start()
    {
        _player.CanMove = false;
        _raceCountdown.gameObject.SetActive(true);
        _mainTimer.text = "0.00";
        _bestTimer.text = $"{Translate.Instance.Tr("best")}{Translate.Instance.Tr("colon")} ";
        if (SaveLoad.Instance.HaveBestTime)
        {
            _bestTimer.text += $"{SaveLoad.Instance.BestTime:0.00}";
        }
        else
        {
            _bestTimer.text += $"{Translate.Instance.Tr("none")}";
        }
        StartCoroutine(LaunchRaceCountdown());
    }

    private void Update()
    {
        if (_didRaceStart)
        {
            RaceTimer += Time.deltaTime;
            _mainTimer.text = $"{RaceTimer:0.00}";
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