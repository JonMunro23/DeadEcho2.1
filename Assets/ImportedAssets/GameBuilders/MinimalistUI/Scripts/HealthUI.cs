using GameBuilders.MinimalistUI.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField]
    private UIController m_UIController;

    Slider m_Slider;
    [SerializeField]
    Image selectedIndicatorImage;
    [SerializeField]
    Color healthyColor, lowHealthColor, nearDeathColor;

    private void Awake()
    {
        m_Slider = GetComponent<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        if (m_Slider.value != m_UIController.HealthController.HealthPercent)
            m_Slider.value = m_UIController.HealthController.HealthPercent;

        if(m_UIController.HealthController.HealthPercent > .5f)
        {
            selectedIndicatorImage.color = healthyColor;
        }
        else if(m_UIController.HealthController.HealthPercent <= .5f && m_UIController.HealthController.HealthPercent > .25f)
        {
            selectedIndicatorImage.color = lowHealthColor;
        }
        else if(m_UIController.HealthController.HealthPercent <= .25f)
        {
            selectedIndicatorImage.color = nearDeathColor;
        }
    }
}
