using UnityEngine;

namespace ScientificGameJam.Race
{
    public class Arrow : MonoBehaviour
    {
        [SerializeField]
        private Sprite _greyedout;

        private Sprite _normal;

        private SpriteRenderer _sr;

        private void Start()
        {
            _sr = GetComponent<SpriteRenderer>();
        }

        public void DisplayNormal()
        {
            _sr.sprite = _normal;
        }

        public void DisplayGrey()
        {
            _sr.sprite = _greyedout;
        }
    }
}
