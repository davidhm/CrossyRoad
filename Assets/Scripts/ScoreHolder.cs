using UnityEngine;
using System.IO;
public class ScoreHolder : MonoBehaviour
{
    private uint playerMaxScore, currentPlayerScore;

    public uint PlayerMaxScore
    {
        get
        {
            return playerMaxScore;
        }

        set
        {
            playerMaxScore = value;
        }
    }

    public uint CurrentPlayerScore
    {
        get
        {
            return currentPlayerScore;
        }

        set
        {
            currentPlayerScore = value;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        processScoreFile();
    }

    void OnApplicationQuit()
    {
        using (StreamWriter fileWriter = new StreamWriter(@".\playerScore.txt"))
        {
            fileWriter.WriteLine(playerMaxScore.ToString());
        }
    }

    private void processScoreFile()
    {
        if (!File.Exists(@".\playerScore.txt"))
        {
            playerMaxScore = 0;
        }
        else
        {
            using (StreamReader fileReader = new StreamReader(@".\playerScore.txt"))
            {
                string scoreString = fileReader.ReadLine();
                playerMaxScore = uint.Parse(scoreString);
            }
        }
    }

}
