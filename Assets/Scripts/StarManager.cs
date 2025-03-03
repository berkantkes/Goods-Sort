using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class StarController : MonoBehaviour
{
    public TextMeshProUGUI countText;
    public GameObject starPrefab;
    public Transform starIcon;
    public float moveDuration = 1.5f;

    private int starCount = 0;
    private List<GameObject> activeStars = new List<GameObject>();

    private void Awake()
    {
        EventManager<Vector3>.Subscribe(GameEvents.OnMatch, SpawnStar);
    }

    private void OnDestroy()
    {
        EventManager<Vector3>.Unsubscribe(GameEvents.OnMatch, SpawnStar);
    }

    private void SpawnStar(Vector3 worldPosition)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        GameObject star = Instantiate(starPrefab, transform);
        star.transform.position = screenPosition; 

        activeStars.Add(star);

        MoveStar(star);
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
}


    