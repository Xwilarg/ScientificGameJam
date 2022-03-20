using UnityEngine;

namespace ScientificGameJam.Audio
{
    public class BGMManager : MonoBehaviour
    {
        public static BGMManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            _source = GetComponent<AudioSource>();
        }

        private void Start()
        {
            PlayPowerupSelect();
        }

        private AudioSource _source;

        [SerializeField]
        private AudioClip _duringRace, _endRace, _endRaceAlternate, _powerupSelect;

        public void PlayDuringRace()
        {
            _source.clip = _duringRace;
            _source.Play();
        }

        public void PlayEndRace()
        {
            _source.clip = _endRace;
            _source.Play();
        }

        public void PlayEndRaceAlt()
        {
            _source.clip = _endRaceAlternate;
            _source.Play();
        }

        public void PlayPowerupSelect()
        {
            _source.clip = _powerupSelect;
            _source.Play();
        }
    }
}
