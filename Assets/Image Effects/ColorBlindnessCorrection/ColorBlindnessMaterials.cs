using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "ColorBlindnessMaterials", menuName = "Game/ColorBlindnessMaterials")]
public class ColorBlindnessMaterials : UnityEngine.ScriptableObject
{
    //---Your Materials---
    public Material customEffect;

    //---Accessing the data from the Pass---
    static ColorBlindnessMaterials _instance;

    public static ColorBlindnessMaterials Instance
    {
        get
        {
            if (_instance != null) return _instance;
            // TODO check if application is quitting
            // and avoid loading if that is the case

            _instance = UnityEngine.Resources.Load<ColorBlindnessMaterials>("ColorBlindnessMaterials");
            return _instance;
        }
    }
}
