using UnityEngine;
using System.IO;

public static class ProgressManager
{
    private static string filePath =
        Path.Combine(Application.persistentDataPath, "progress.json");

    public static GameProgress Load(LevelDatabaseSO database)
    {
        GameProgress progress;

        if (!File.Exists(filePath))
        {
            progress = new GameProgress();
        }
        else
        {
            string json = File.ReadAllText(filePath);
            progress = JsonUtility.FromJson<GameProgress>(json);
        }

        int total = database.allLevels.Length;

        // 🚑 TỰ MỞ RỘNG SAVE NẾU THIẾU
        while (progress.levels.Count < total)
        {
            progress.levels.Add(new LevelProgress
            {
                unlocked = progress.levels.Count == 0
            });
        }

        Save(progress);
        return progress;
    }


    public static void Save(GameProgress progress)
    {
        File.WriteAllText(filePath, JsonUtility.ToJson(progress, true));
    }

    public static void UnlockNext(int index, LevelDatabaseSO database)
    {
        GameProgress progress = Load(database);

        int next = index + 1;
        if (next < progress.levels.Count)
        {
            progress.levels[next].unlocked = true;
            Save(progress);
        }
    }

    public static void DeleteSave()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("[SAVE] progress.json đã bị xóa");
        }
        else
        {
            Debug.Log("[SAVE] Không có save để xóa");
        }
    }

}
