using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public static class SettingsManager
{
    public static Settings mySettings = new Settings();

    public static void SaveSettings()
    {
        mySettings.HammerPrefix = SceneConfiguration.HammerPrefix;
        mySettings.HolePrefix = SceneConfiguration.HolePrefix;
        mySettings.MolePrefix = SceneConfiguration.MolePrefix;

        //Save from memory to xml on the disc
        XmlSerializer serializer = new XmlSerializer(typeof(Settings));
        FileStream stream = new FileStream(Application.dataPath + "/Settings.xml", FileMode.Create);
        serializer.Serialize(stream, mySettings);
        stream.Close();
    }

    public static void LoadSettings()
    {
        //Read settings from xml
        XmlSerializer serializer = new XmlSerializer(typeof(Settings));
        FileStream stream;
        Settings copiedSettings;

        //If no file is found or deserialize fails, create a new empty one
        try
        {
            stream = new FileStream(Application.dataPath + "/Settings.xml", FileMode.Open);
            copiedSettings = serializer.Deserialize(stream) as Settings;
        }
        catch
        {
            Settings emptySettings = new Settings();

            stream = new FileStream(Application.dataPath + "/Settings.xml", FileMode.Create);
            serializer.Serialize(stream, mySettings);
            stream.Close();

            serializer = new XmlSerializer(typeof(Settings));
            stream = new FileStream(Application.dataPath + "/Settings.xml", FileMode.Open);
            copiedSettings = serializer.Deserialize(stream) as Settings;
        }

        stream.Close();
        mySettings = copiedSettings;
        
        SceneConfiguration.HammerPrefix = mySettings.HammerPrefix;
        SceneConfiguration.HolePrefix = mySettings.HolePrefix;
        SceneConfiguration.MolePrefix = mySettings.MolePrefix;
    }

    public static void ResetSettings()
    {
        mySettings = new Settings();
        
        SceneConfiguration.HammerPrefix = mySettings.HammerPrefix;
        SceneConfiguration.HolePrefix = mySettings.HolePrefix;
        SceneConfiguration.MolePrefix = mySettings.MolePrefix;
        SaveSettings();
    }

    public class Settings
    {
        public string HammerPrefix = "";
        public string HolePrefix = "";
        public string MolePrefix = "";
        public float MusicLevel = 1.0f;
        public float SoundLevel = 1.0f;
        
        public Settings()
        {

        }
    }
}
