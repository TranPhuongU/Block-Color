using System;
using System.Collections.Generic;

[Serializable]
public class LevelProgress 
{
    public bool unlocked;
}

public class GameProgress
{
    public List<LevelProgress> levels = new List<LevelProgress>();
}
