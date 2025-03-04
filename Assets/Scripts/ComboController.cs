using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class ComboController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _comboCountText;
        [SerializeField] private Slider _comboSlider;
        [SerializeField] private float _comboTime = 25f;

        private EventManager _eventManager;
        private StarController _starController;
        private Coroutine _comboCoroutine;
        public int ComboCount
        {
            get { return _comboCount; }
        }

        private int _comboCount = 0;
        private float _timer = 0f;

        [Inject]
        public void Construct(EventManager eventManager)
        {
            _eventManager = eventManager;
        }
        private void OnEnable()
        {
            _comboCountText.text = string.Empty;
            _comboSlider.value = 0;

            _eventManager.Subscribe<Vector3>(GameEvents.OnMatch, Combo);
        }
        private void OnDisable()
        {
            _eventManager.Unsubscribe<Vector3>(GameEvents.OnMatch, Combo);
        }


        public void Combo(Vector3 position)
        {
            if (!gameObject.activeInHierarchy)
                return;
            _comboCount++;
            _comboCountText.text = "x" + _comboCount.ToString();
            _timer = _comboTime - _comboCount;

            if (_comboCoroutine != null) StopCoroutine(_comboCoroutine);
            _comboCoroutine = StartCoroutine(StartComboCor());
        }

        public void StopCombo(bool isWin, bool isAllLinesFilled)
        {
            if (_comboCoroutine != null) StopCoroutine(_comboCoroutine);
        }

        private IEnumerator StartComboCor()
        {
            _comboSlider.value = _timer / (_comboTime - _comboCount);

            while (_timer > 0)
            {
                _timer -= Time.deltaTime;
                _comboSlider.value = _timer / (_comboTime - _comboCount);
                yield return null;
            }

            _comboCount = 0;
            _comboCountText.text = string.Empty;
        }
    }