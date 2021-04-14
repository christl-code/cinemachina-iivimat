using UnityEngine;
using UnityEngine.UI;
public class HUDManager : MonoBehaviour
{
    private GameObject HUD;
    private GameObject textPositionHUDGO;
    private Text textPositionHUD;

    private RectTransform rectTransform;
    void Start()
    {
        // HUD where one add text or others UI GameObjects
        HUD = GameObject.FindGameObjectWithTag("HUD");
        HUD.transform.SetParent(Camera.main.transform);
        HUD.GetComponent<Canvas>().planeDistance = 0.50f;
        // GameObject which holds Text object
        textPositionHUDGO = new GameObject();
        textPositionHUDGO.name = "textPositionHUDGO";
        textPositionHUDGO.transform.SetParent(HUD.transform);

        textPositionHUD = textPositionHUDGO.AddComponent<Text>();
        textPositionHUD.font = Font.CreateDynamicFontFromOSFont(Font.GetPathsToOSFonts()[0], 50);
        textPositionHUD.fontSize = 10;

        rectTransform = textPositionHUD.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(-200, 0, 0);
        rectTransform.right = -(rectTransform.localPosition - Camera.main.transform.position).normalized;

    }

    void Update()
    {
        textPositionHUD.text = HUD.transform.position.ToString();
    }
}
