using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public UpgradesData upgradesData;

    void Awake()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("playerupgrades");
        UpgradeWrapper wrapper = JsonUtility.FromJson<UpgradeWrapper>(jsonFile.text);
        upgradesData = wrapper.upgrades;
    }
}
