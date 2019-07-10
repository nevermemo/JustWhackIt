using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public static class HighScoreManager
{
    public static int TopCapacity = 5;
    public static HighScoreHolder LocalHighscores;
    public static HighScoreHolder OnlineHighscores;
    public static HighScoreHolder LocalAdvancedHighscores;
    public static HighScoreHolder OnlineAdvancedHighscores;


    public static void SaveLocalHighScores()
    {
        //Save from memory to xml on the disc
        XmlSerializer serializer = new XmlSerializer(typeof(HighScoreHolder));
        FileStream stream = new FileStream(Application.dataPath + "/HighScores.xml", FileMode.Create);
        serializer.Serialize(stream, LocalHighscores);
        stream.Close();
    }

    public static void LoadLocalHighScores()
    {
        //Read Local scores from xml
        XmlSerializer serializer = new XmlSerializer(typeof(HighScoreHolder));
        FileStream stream;

        //If no file is found, create a new empty one
        try
        {
            stream = new FileStream(Application.dataPath + "/HighScores.xml", FileMode.Open);
        }
        catch
        {
            HighScoreHolder emptyHighScores = new HighScoreHolder();
            emptyHighScores.Capacity = TopCapacity;
            for (int i = 0; i < TopCapacity; i++)
                emptyHighScores.InsertNew(0, "EMPTY");

            stream = new FileStream(Application.dataPath + "/HighScores.xml", FileMode.Create);
            serializer.Serialize(stream, emptyHighScores);
            stream.Close();

            serializer = new XmlSerializer(typeof(HighScoreHolder));
            stream = new FileStream(Application.dataPath + "/HighScores.xml", FileMode.Open);
        }

        HighScoreHolder localScores = serializer.Deserialize(stream) as HighScoreHolder;
        stream.Close();
        LocalHighscores = localScores;
    }

    public static void ResetLocalHighScores()
    {
        HighScoreHolder emptyHighScores = new HighScoreHolder();
        emptyHighScores.Capacity = TopCapacity;
        for (int i = 0; i < TopCapacity; i++)
            emptyHighScores.InsertNew(0, "EMPTY");
        LocalHighscores = emptyHighScores;
        SaveLocalHighScores();
    }
    public static void SaveLocalAdvancedHighScores()
    {
        //Save from memory to xml on the disc
        XmlSerializer serializer = new XmlSerializer(typeof(HighScoreHolder));
        FileStream stream = new FileStream(Application.dataPath + "/AdvancedHighScores.xml", FileMode.Create);
        serializer.Serialize(stream, LocalAdvancedHighscores);
        stream.Close();
    }

    public static void LoadLocalAdvancedHighScores()
    {
        //Read Local scores from xml
        XmlSerializer serializer = new XmlSerializer(typeof(HighScoreHolder));
        FileStream stream;

        //If no file is found, create a new empty one
        try
        {
            stream = new FileStream(Application.dataPath + "/AdvancedHighScores.xml", FileMode.Open);
        }
        catch
        {
            HighScoreHolder emptyHighScores = new HighScoreHolder();
            emptyHighScores.Capacity = TopCapacity;
            for (int i = 0; i < TopCapacity; i++)
                emptyHighScores.InsertNew(0, "EMPTY");

            stream = new FileStream(Application.dataPath + "/AdvancedHighScores.xml", FileMode.Create);
            serializer.Serialize(stream, emptyHighScores);
            stream.Close();

            serializer = new XmlSerializer(typeof(HighScoreHolder));
            stream = new FileStream(Application.dataPath + "/AdvancedHighScores.xml", FileMode.Open);
        }

        HighScoreHolder localScores = serializer.Deserialize(stream) as HighScoreHolder;
        stream.Close();
        LocalAdvancedHighscores = localScores;
    }

    public static void ResetLocalAdvancedHighScores()
    {
        HighScoreHolder emptyHighScores = new HighScoreHolder();
        emptyHighScores.Capacity = TopCapacity;
        for (int i = 0; i < TopCapacity; i++)
            emptyHighScores.InsertNew(0, "EMPTY");
        LocalAdvancedHighscores = emptyHighScores;
        SaveLocalAdvancedHighScores();
    }

    public static void ResetAllLocalHighScores()
    {
        ResetLocalHighScores();
        ResetLocalAdvancedHighScores();
    }

    public static void LoadAllLocalHighScores()
    {
        LoadLocalHighScores();
        LoadLocalAdvancedHighScores();
    }

    public static void SaveAllLocalHighScores()
    {
        SaveLocalHighScores();
        SaveLocalAdvancedHighScores();
    }

    //TODO: Add online support

    public class HighScoreHolder
    {
        public int Capacity = 5;
        public List<ScoreEntry> Entries = new List<ScoreEntry>();        //Key: Score    //Value: Name

        public HighScoreHolder()
        {

        }

        public void InsertNew(int score, string name)
        {
            Entries.Add(new ScoreEntry(score, name));
            Entries.Sort(new ScoreEntryComparor());
            if (Entries.Count > Capacity)
                Entries.RemoveAt(Capacity);
        }


    }

    public class ScoreEntry
    {
        public int Score;
        public string Name;

        public ScoreEntry()
        {

        }

        public ScoreEntry(int s, string n)
        {
            Score = s;
            Name = n;
        }
    }

    public class ScoreEntryComparor : IComparer<HighScoreManager.ScoreEntry>
    {
        public int Compare(ScoreEntry x, ScoreEntry y)
        {
            int result = y.Score.CompareTo(x.Score);
            if (result == 0)
                return 1;
            return result;
        }
    }

}