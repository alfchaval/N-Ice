using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class LanguageManager
{
    public static event Action<Language> OnLanguageChanged;

    private static Language currentLanguage;
    private static bool languageChecked;
    private static Dictionary<Language, Dictionary<string, string>> translations;
    
    private const string translationsFolder = "Translations";
    private const string PP_Language = "PP_Language";

    private const Language defaultLanguage = Language.English;

    private const string codeEnglish = "en";
    private const string codeSpanish = "es";

    public enum Language
    {
        English,
        Spanish
    }

    public static string GetLanguageCode(Language language)
    {
        return language switch
        {
            Language.English => codeEnglish,
            Language.Spanish => codeSpanish,
            _ => "false"
        };
    }

    public static Language GetLanguage(String languageCode)
    {
        return languageCode switch
        {
            codeEnglish => Language.English,
            codeSpanish => Language.Spanish,
            _ => defaultLanguage
        };
    }

    public static string GetLanguageCode()
    {
        return GetLanguageCode(GetLanguage());
    }

    public static Language GetLanguage()
    {
        CheckCorrectLanguage();
        return currentLanguage;
    }

    public static void SetLanguage(Language language)
    {
        if (currentLanguage != language)
        {
            currentLanguage = language;
            OnLanguageChanged?.Invoke(language);
        }
    }

    private static void CheckCorrectLanguage()
    {
        if (!languageChecked)
        {
            if(PlayerPrefs.HasKey(PP_Language))
            {
                string languageCode = PlayerPrefs.GetString(PP_Language, GetLanguageCode(defaultLanguage));
                currentLanguage = GetLanguage(languageCode);
            }
            else
            {
                CustomBuildSettings customBuildSettings = Resources.Load<CustomBuildSettings>("CustomBuildSettings");
                if (customBuildSettings)
                {
                    currentLanguage = customBuildSettings.targetLanguage;
                }
                else
                {
                    currentLanguage = defaultLanguage;
                }
            }
            languageChecked = true;
        }
    }

    public static string Localize(string localizationKey)
    {
        CheckCorrectLanguage();
        return Localize(localizationKey, currentLanguage);
    }

    public static string Localize(string localizationKey, Language language)
    {
        if (translations == null)
        {
            CreateDictionary();
            ReadTranslations();
        }
        if (translations[currentLanguage].ContainsKey(localizationKey))
        {
            return translations[language][localizationKey];
        }
        return "";
    }

    private static void CreateDictionary()
    {
        Language[] languages = (Language[])System.Enum.GetValues(typeof(Language));
        translations = new Dictionary<Language, Dictionary<string, string>>();
        for (int i = 0; i < languages.Length; i++)
        {
            translations[languages[i]] = new Dictionary<string, string>();
        }
    }

    public static void ReadTranslations()
    {
        TextAsset[] textAssets = Resources.LoadAll<TextAsset>(translationsFolder);

        foreach (TextAsset textAsset in textAssets)
        {
            try
            {
                string text = textAsset.text.Replace("\r\n", "\n").Replace("\"\"", "[_quote_]");
                MatchCollection matches = Regex.Matches(text, "\"[\\s\\S]+?\"");

                foreach (Match match in matches)
                {
                    text = text.Replace(match.Value, match.Value.Replace("\"", null).Replace(";", "[_semicolon_]").Replace("\n", "[_newline_]"));
                }

                // Making uGUI line breaks to work in Asian texts.
                text = text.Replace("。", "。 ").Replace("、", "、 ").Replace("：", "： ").Replace("！", "！ ").Replace("（", " （").Replace("）", "） ").Trim();

                List<string> lines = text.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line) && !line.TrimStart().StartsWith(";")).ToList();
                List<string> languages = lines[0].Split(';').Select(i => i.Trim()).ToList();

                for (int i = 1; i < lines.Count; i++)
                {
                    try
                    {
                        List<string> columns = lines[i].Split(';').Select(j => j.Trim()).Select(j => j.Replace("[_quote_]", "\"").Replace("[_semicolon_]", ";").Replace("[_newline_]", "\n")).ToList();

                        if (columns.Count < languages.Count)
                        {
                            throw new Exception($"Insufficient number of columns. {languages.Count} was expected, but {columns.Count} was found.");
                        }

                        string key = columns[0];
                        if (string.IsNullOrEmpty(key)) continue;

                        Language languageFound;
                        for (int j = 1; j < languages.Count; j++)
                        {
                            if (Enum.TryParse(languages[j], out languageFound))
                            {
                                translations[languageFound].Add(key, columns[j]);
#if UNITY_EDITOR
                                if (columns[j].Length < 1)
                                {
                                    Debug.LogError("Missing key " + key + "for " + languageFound);
                                }
#endif
                            }
                            else
                            {
                                throw new Exception($"Invalid column name: '{languages[j]}'.");
                            }
                        }
                    }
                    catch (Exception lineException)
                    {
                        Debug.LogError($"Error in file '{textAsset.name}', line {i + 1}: {lineException.Message}");
                    }
                }
            }
            catch (Exception fileException)
            {
                Debug.LogError($"Error processing file '{textAsset.name}': {fileException.Message}");
            }
        }
    }
}