using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHUDScript : MonoBehaviour
{
    [SerializeField]
    private GameObject camera;

    [SerializeField]
    private GameObject HealthBar;

    [SerializeField]
    private float HealthRate;

    [SerializeField]
    private GameObject BlueCollectedText;

    [SerializeField]
    private GameObject GreenCollectedText;

    [SerializeField]
    private GameObject OrangeCollectedText;

    [SerializeField]
    private GameObject PinkCollectedText;

    // Start is called before the first frame update
    void Start()
    {
        HealthBar.GetComponent<Slider>().value = 100;
        GameObject fill = HealthBar.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;

        fill.GetComponent<Image>().color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.GetComponent<Slider>().value -= Time.deltaTime * HealthRate;

        UpdateSlider();

        bool win = BlueCollectedText.GetComponent<DiamondCollectScript>().CollectedAll();

        win &= GreenCollectedText.GetComponent<DiamondCollectScript>().CollectedAll();

        win &= OrangeCollectedText.GetComponent<DiamondCollectScript>().CollectedAll();

        win &= PinkCollectedText.GetComponent<DiamondCollectScript>().CollectedAll();

        if (win)
        {
            SceneManager.LoadScene("GameWin");
        }
    }

    void UpdateSlider()
    {
        float value = HealthBar.GetComponent<Slider>().value;

        float maxValue = HealthBar.GetComponent<Slider>().maxValue;

        GameObject fill = HealthBar.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;

        if (value > maxValue / 2.0f)
        {
            fill.GetComponent<Image>().color = Color.Lerp(new Color(1.0f, 1.0f, 0.0f), Color.green, ((value - maxValue / 2.0f) / (maxValue / 2.0f)));
        }
        else
        {
            fill.GetComponent<Image>().color = Color.Lerp(Color.red, new Color(1.0f, 1.0f, 0.0f), (value / (maxValue / 2.0f)));
        }

        if (value > maxValue / 10.0f)
        {
            camera.GetComponent<CameraEffect>().ChangeVignette(0.0f);
        }
        else
        {
            camera.GetComponent<CameraEffect>().ChangeVignette(1.0f - value / (maxValue / 10.0f));
        }

        if (value < 0.2f)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    public void CollectDiamond(GameObject gameObject)
    {
        if(gameObject.tag == "Blue Ore")
        {
            BlueCollectedText.GetComponent<DiamondCollectScript>().CollectDiamond();
        }
        else if (gameObject.tag == "Green Ore")
        {
            GreenCollectedText.GetComponent<DiamondCollectScript>().CollectDiamond();
        }
        else if (gameObject.tag == "Pink Ore")
        {
            PinkCollectedText.GetComponent<DiamondCollectScript>().CollectDiamond();
        }
        else if (gameObject.tag == "Orange Ore")
        {
            OrangeCollectedText.GetComponent<DiamondCollectScript>().CollectDiamond();
        }
    }

    public void CollectOxygen()
    {
        HealthBar.GetComponent<Slider>().value += 40;

        UpdateSlider();
    }

    public void FillOxygen()
    {
        HealthBar.GetComponent<Slider>().value = HealthBar.GetComponent<Slider>().maxValue;
    }
}
