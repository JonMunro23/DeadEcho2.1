using TMPro;
using UnityEngine;
using DG.Tweening;

public class PointsUI : MonoBehaviour
{
    /// <summary>
    /// Text that will display the current points
    /// </summary>
    [SerializeField]
    [Tooltip("Text that will display the current points.")]
    private TextMeshProUGUI m_CurrentPointsLabel;

    [SerializeField] Transform gainedPointsTextSpawnLocation;
    public GameObject gainedPointsTextPrefab;

    

    private void OnEnable()
    {
        PointsManager.onPointsAdded += PointsAdded;
        PointsManager.onPointsRemoved += PointsRemoved;
    }

    private void OnDisable()
    {
        PointsManager.onPointsAdded -= PointsAdded;
        PointsManager.onPointsRemoved -= PointsRemoved;
    }

    void PointsAdded(int pointsAdded)
    {
        UpdateTextLabel();
        SpawnGainedPointsText(pointsAdded);
    }

    void PointsRemoved(int pointsRemoved)
    {
        UpdateTextLabel();
    }

    /// <summary>
    /// Updates the current points label
    /// </summary>
    void UpdateTextLabel()
    {
        m_CurrentPointsLabel.text = "$" + PointsManager.Instance.CurrentPoints.ToString();
    }

    void SpawnGainedPointsText(int gainedPoints)
    {
        float randXOffset = UnityEngine.Random.Range(-5f, 5f);
        float randYOffset = UnityEngine.Random.Range(-5f, 5f);
        Vector2 spawnLocation = new Vector2(gainedPointsTextSpawnLocation.position.x + randXOffset, gainedPointsTextSpawnLocation.position.y + randYOffset);
        GameObject clone = Instantiate(gainedPointsTextPrefab, spawnLocation, Quaternion.identity, gainedPointsTextSpawnLocation);
        TMP_Text text = clone.GetComponent<TMP_Text>();
        text.text = "+" + gainedPoints.ToString();
        clone.GetComponent<Rigidbody2D>().AddForce(Vector2.right * UnityEngine.Random.Range(50, 80 + 1), ForceMode2D.Impulse);
        clone.GetComponent<Rigidbody2D>().AddForce(Vector2.up * UnityEngine.Random.Range(-15, 15 + 1), ForceMode2D.Impulse);

        text.DOColor(Color.clear, .2f).SetDelay(1).OnComplete(() =>
        {
            Destroy(clone);
        }
        );
    }
}
