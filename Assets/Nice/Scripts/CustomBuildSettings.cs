using UnityEngine;

[CreateAssetMenu(fileName = "CustomBuildSettings", menuName = "ScriptableObjects/CustomBuildSettings")]
public class CustomBuildSettings : ScriptableObject
{
    public LanguageManager.Language targetLanguage;
}