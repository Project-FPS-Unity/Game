using System;

[Serializable]
public class HighScoreElement
{
    public string playerName;
    public int points;

    public HighScoreElement(string name, int points)
    {
        this.playerName = name;
        this.points = points;
    }
}
