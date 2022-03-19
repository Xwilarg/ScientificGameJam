using ScientificGameJam.Player;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ScientificGameJam.SaveData
{
    public class SaveLoad
    {
        public static SaveLoad Instance
        {
            get
            {
                _instance ??= new SaveLoad();
                return _instance;
            }
        }
        private static SaveLoad _instance;

        private SaveLoad()
        {
            if (Directory.Exists(Application.persistentDataPath))
            {
                if (File.Exists(_pathTime))
                {
                    using FileStream file = new FileStream(_pathTime, FileMode.Open, FileAccess.Read);
                    using BinaryReader reader = new BinaryReader(file);
                    BestTime = reader.ReadSingle();

                    var dictLength = reader.ReadInt32();
                    var coor = new List<PlayerCoordinate>();
                    for (int i = 0; i < dictLength; i++)
                    {
                        var time = reader.ReadSingle();
                        var pos = new Vector2(reader.ReadSingle(), reader.ReadSingle());
                        var angle = reader.ReadSingle();
                        coor.Add(new PlayerCoordinate
                        {
                            TimeSinceStart = time,
                            Position = pos,
                            Rotation = angle
                        });
                    }
                    Coordinates = coor;
                }
            }
            else
            {
                Directory.CreateDirectory(Application.persistentDataPath);
            }
        }

        private void UpdateSavesTime()
        {
            UnityEngine.Debug.Log($"Saves updated at {_pathTime}");
            using FileStream file = new FileStream(_pathTime, FileMode.OpenOrCreate, FileAccess.Write);
            using BinaryWriter writer = new BinaryWriter(file);

            writer.Write(BestTime);
            writer.Write(Coordinates.Count);
            foreach (var elem in Coordinates)
            {
                writer.Write(elem.TimeSinceStart);
                writer.Write(elem.Position.x);
                writer.Write(elem.Position.y);
                writer.Write(elem.Rotation);
            }
        }

        public bool HaveBestTime => Coordinates.Any();
        public float BestTime { private set; get; } = -1f;
        public IReadOnlyList<PlayerCoordinate> Coordinates { private set; get; } = new List<PlayerCoordinate>();
        public void UpdateBestTime(float timer, List<PlayerCoordinate> coordinates)
        {
            if (!HaveBestTime || timer < BestTime)
            {
                BestTime = timer;
                Coordinates = coordinates;
                UpdateSavesTime();
            }
        }

        private string _pathTime = $"{Application.persistentDataPath}/time.bin";
    }
}
