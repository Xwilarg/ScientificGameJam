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

                    var checkpointsLength = reader.ReadInt32();
                    var cpts = new List<float>();
                    for (int i = 0; i < checkpointsLength; i++)
                    {
                        cpts.Add(reader.ReadSingle());
                    }
                    var dictLength = reader.ReadInt32();
                    var coor = new List<PlayerCoordinate>();
                    for (int i = 0; i < dictLength; i++)
                    {
                        var time = reader.ReadSingle();
                        var pos = new Vector2(reader.ReadSingle(), reader.ReadSingle());
                        var angle = reader.ReadSingle();
                        var vel = new Vector2(reader.ReadSingle(), reader.ReadSingle());
                        coor.Add(new PlayerCoordinate
                        {
                            TimeSinceStart = time,
                            Position = pos,
                            Rotation = angle,
                            Velocity = vel
                        });
                    }
                    Checkpoints = cpts;
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
            writer.Write(Checkpoints.Count);
            foreach (var elem in Checkpoints)
            {
                writer.Write(elem);
            }
            writer.Write(Coordinates.Count);
            foreach (var elem in Coordinates)
            {
                writer.Write(elem.TimeSinceStart);
                writer.Write(elem.Position.x);
                writer.Write(elem.Position.y);
                writer.Write(elem.Rotation);
                writer.Write(elem.Velocity.x);
                writer.Write(elem.Velocity.y);
            }
        }

        public bool HaveBestTime => Coordinates.Any();
        public float BestTime { private set; get; } = -1f;
        public IReadOnlyList<PlayerCoordinate> Coordinates { private set; get; } = new List<PlayerCoordinate>();
        public IReadOnlyList<float> Checkpoints { private set; get; } = new List<float>();
        public bool UpdateBestTime(float timer, List<PlayerCoordinate> coordinates, List<float> checkpoints)
        {
            if (!HaveBestTime || timer < BestTime)
            {
                BestTime = timer;
                Coordinates = coordinates;
                Checkpoints = checkpoints;
                UpdateSavesTime();
                return true;
            }
            return false;
        }

        private readonly string _pathTime = $"{Application.persistentDataPath}/time.bin";
    }
}
