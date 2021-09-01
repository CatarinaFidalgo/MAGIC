using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class SliderTextUpdate : MonoBehaviour
{
    public string format = "0.0";

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void UpdateText(Slider slider)
    {
        GetComponent<Text>().text = slider.value.ToString(format);
    }
}
