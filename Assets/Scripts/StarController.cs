using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Zenject;

public class StarController : MonoBehaviour
{
    public TextMeshProUGUI countText;
    public GameObject starPrefab;
    public Transform starIcon;
    public float moveDuration = 1.5f;
    [SerializeField] private float spawnRandomRange = 200f; 

    private int starCount = 0;
    private List<GameObject> activeStars = new List<GameObject>();
    private EventManager _eventManager;
    private ComboController _comboController;

    [Inject]
    public void Construct(EventManager eventManager, ComboController comboController)
    {
        _eventManager = eventManager;
        _comboController = comboController;
    }

    private void OnEnable()
    {
        _eventManager.Subscribe<Vector3>(GameEvents.OnMatch, SpawnStars);
    }

    private void OnDisable()
    {
        _eventManager.Unsubscribe<Vector3>(GameEvents.OnMatch, SpawnStars);
    }

    private void SpawnStars(Vector3 worldPosition)
    {
        int comboCount = _comboController.ComboCount;
        int starsToSpawn = CalculateStarCount(comboCount);

        for (int i = 0; i < starsToSpawn; i++)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

            // Küçük rastgele kaydırma
            screenPosition.x += Random.Range(-spawnRandomRange, spawnRandomRange);
            screenPosition.y += Random.Range(-spawnRandomRange, spawnRandomRange);

            GameObject star = Instantiate(starPrefab, transform);
            star.transform.position = screenPosition;

            activeStars.Add(star);
            MoveStar(star);
        }
    }

    private void MoveStar(GameObject star)
    {
        Vector3 start = star.transform.position;
        Vector3 target = starIcon.position;

        Vector3 controlPoint = start + (target - start) * 0.3f;
        controlPoint.y += 100f;

        star.transform
            .DOPath(new Vector3[] { start, controlPoint, target }, moveDuration, PathType.CatmullRom)
            .SetEase(Ease.InOutCubic)
            .OnComplete(() =>
            {
                Destroy(star);
                activeStars.Remove(star);
                EarnStar();
            });
    }

    private void EarnStar()
    {
        starCount++;
        countText.text = starCount.ToString();
    }

    private int CalculateStarCount(int comboCount)
    {
        if (comboCount < 3)
            return 1;
        else if (comboCount < 7)
            return 2;
        else
            return 2 + (comboCount - 7) / 7;
    }
}
