using UnityEngine;
using UnityEngine.UI;

public class ParticleToggle : MonoBehaviour
{
    // Reference to the toggle component
    public Toggle toggle;

    // Reference to the background particles script
    public GameObject background;

    // Start is called before the first frame update
    void Start()
    {
        // Add listener for toggle value change
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    // Callback method for toggle value change
    public void OnToggleValueChanged(bool isOn)
    {
        
        // If particles are being enabled, generate the particle grid
        if (isOn)
        {
            background.SetActive(true);
        }else
        {
            background.SetActive(false);
        }
    }
}
