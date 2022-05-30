using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIInputManager : MonoBehaviour
{
    private VisualElement root;                             // Boilerplate.. get root root of UI
    private VisualElement UIPrimaryWeaponImage;
    private VisualElement UISecondaryWeaponImage;
    private ProgressBar HealthBar;
    private Button menuButton;

    private Label UIScore;
    private int score = 0;
    private Label UIPrimaryAmmoLabel;

    private Label UISecondaryAmmoLabel;

    private int secondaryAmmoCount;

    [field: SerializeField] private Sprite[] UIWeaponIconArr;

    private int selectedPrimary = 3;
    private int selectedSecondary = 7;

    [field: SerializeField] public GameObject player;
    public float playerHealth;


    private void Menu()
    {
        GameObject.Find("SceneManager").GetComponent<SceneLoader>().ChangeScene("MainMenu");
    }

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        // UIPrimaryWeaponImage = root.Q<VisualElement>("weapon__primary__image");
        // UISecondaryWeaponImage = root.Q<VisualElement>("weapon__secondary__image");

        UIWeaponIconArr = Resources.LoadAll<Sprite>("Images/");
        HealthBar = root.Q<ProgressBar>("health_bar");

        UIPrimaryAmmoLabel = root.Q<Label>("weapon_primary__ammo");
        UISecondaryAmmoLabel = root.Q<Label>("weapon_secondary__ammo");

        UIScore = root.Q<Label>("score__label");

        menuButton = root.Q<Button>("menu");
    }
    void Start()
    {
        secondaryAmmoCount = 0; //Todo : Remove Test
        UISecondaryAmmoLabel.text = $"{secondaryAmmoCount}"; //Todo : Remove Test

        root.Q<VisualElement>("weapon_primary__image").style.backgroundImage =
        new StyleBackground(UIWeaponIconArr[selectedPrimary]);

        root.Q<VisualElement>("weapon_secondary__image").style.backgroundImage =
        new StyleBackground(UIWeaponIconArr[selectedSecondary]);

        menuButton.clicked += Menu;
    }


    void Update()
    {
        score++;
        HealthBar.value = player.GetComponent<NPC>().Health;
        HealthBar.title = $"{HealthBar.value}%";

        UIScore.text = $"Score : {player.GetComponent<NPC>().Score}";

        UIPrimaryAmmoLabel.text = $"{ player.GetComponent<FireArm>().Ammo }";
    }
}
