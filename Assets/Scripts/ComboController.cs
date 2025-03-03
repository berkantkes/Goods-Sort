using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Timer
{
    public class ComboController : MonoBehaviour
    {
        #region Variables

        // Public Variables
        public TextMeshProUGUI ComboCountText;
        public Slider ComboSlider;

        public int ComboCount
        {
            get { return _comboCount; }
        }

        // Private Variables
        private int _comboCount = 0;
        [SerializeField] float ComboTime = 25f;
        private float _timer = 0f;

        #endregion

        private void Awake()
        {
            ComboCountText.text = string.Empty;
            ComboSlider.value = 0;

            EventManager<Vector3>.Subscribe(GameEvents.OnMatch, Combo);
            // MatchGroup.OnMatched += Combo;
            // LevelPrefab.OnGameFinished += StopCombo;
        }
        
        private void OnDestroy()
        {
            EventManager<Vector3>.Unsubscribe(GameEvents.OnMatch, Combo);
            // MatchGroup.OnMatched -= Combo;
            // LevelPrefab.OnGameFinished -= StopCombo;
        }

        Coroutine ComboCoroutine;

        public void Combo(Vector3 position)
        {
            if (!gameObject.activeInHierarchy)
                return;
            _comboCount++;
            ComboCountText.text = "x" + _comboCount.ToString();
            _timer = ComboTime - _comboCount;

            if (ComboCoroutine != null) StopCoroutine(ComboCoroutine);
            ComboCoroutine = StartCoroutine(StartComboCor());
        }

        public void StopCombo(bool isWin, bool isAllLinesFilled)
        {
            if (ComboCoroutine != null) StopCoroutine(ComboCoroutine);
        }

        private IEnumerator StartComboCor()
        {
            ComboSlider.value = _timer / (ComboTime - _comboCount);

            while (_timer > 0)
            {
                _timer -= Time.deltaTime;
                ComboSlider.value = _timer / (ComboTime - _comboCount);
                yield return null;
            }

            _comboCount = 0;
            ComboCountText.text = string.Empty;
        }
    }
}