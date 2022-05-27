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

    private Label UIScore;
    private int score;
    private Label UIPrimaryAmmoLabel;

    private int primaryAmmoCount;

    private Label UISecondaryAmmoLabel;

    private int secondaryAmmoCount;

    [field: SerializeField] private Sprite[] UIWeaponIconArr;

    private int selectedPrimary = 0;
    private int selectedSecondary = 7;
    private void Stuff()
    {
        // Do stuff
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
    }
    void Start()
    {
        //Todo Remove tests
        HealthBar.value = 50f;

        primaryAmmoCount = 99;
        secondaryAmmoCount = 50;

        score = 0;


        HealthBar.title = $"{HealthBar.value}%";



        root.Q<VisualElement>("weapon_primary__image").style.backgroundImage =
        new StyleBackground(UIWeaponIconArr[selectedPrimary]);

        root.Q<VisualElement>("weapon_secondary__image").style.backgroundImage =
        new StyleBackground(UIWeaponIconArr[selectedSecondary]);

        UIPrimaryAmmoLabel.text = $"{primaryAmmoCount}";
        UISecondaryAmmoLabel.text = $"{secondaryAmmoCount}";


    }


    void Update()
    {
        score++;

        UIScore.text = $"Score : {score}";
    }
}
