using ScientificGameJam.Player;
using System.Collections;
using TMPro;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    /// <summary>
    /// Text displaying the countdown when the race start
    /// </summary>
    [SerializeField]
    private TMP_Text _raceCountdown;

    /// <summary>
    /// Reference to the player
    /// </summary>
    [SerializeField]
    private PlayerController _player;

    private void Start()
    {
        _player.CanMove = false;
        _raceCountdown.gameObject.SetActive(true);
        StartCoroutine(LaunchRaceCountdown());
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
    }
}