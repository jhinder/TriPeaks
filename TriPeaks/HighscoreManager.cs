using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace TriPeaks
{
    /// <summary>
    /// The high score manager allows to read and write the high score table.
    /// </summary>
    internal sealed class HighscoreManager
    {
        private const string SavefileName = "scores.bin";

        private readonly static Lazy<HighscoreManager> _sharedInstance = new Lazy<HighscoreManager>(() => new HighscoreManager());

        public static Lazy<HighscoreManager> LazyInstance => _sharedInstance;
        public static HighscoreManager Instance => _sharedInstance.Value;
        
        private List<HighScoreEntry> scoreboard = null;
        private bool canSave = true;
        private object scoreboardLock = new object();

        private HighscoreManager()
        {
            var appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TriPeaks");
            if (!Directory.Exists(appDataFolder)) {
                try {
                    Directory.CreateDirectory(appDataFolder);
                } catch {
                    canSave = false;
                    MessageBox.Show("Can't create a savefile directory.", "Cannot save highscores", MessageBoxButton.OK, MessageBoxImage.Warning);
                    PopulateScoreboardWithDefaults();
                    return;
                }
            }

            if (canSave)
                Environment.CurrentDirectory = Environment.CurrentDirectory = appDataFolder;

            Load();
        }

        public IList<HighScoreEntry> GetScoreboard()
        {
            return scoreboard;
        }

        public void TryAddHighscore(HighScoreEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));
            lock (scoreboardLock) {
                scoreboard.Add(entry);
                scoreboard = scoreboard
                .OrderByDescending(x => x.Score)
                .Take(10)
                .ToList();
            }
        }

        private void Load()
        {
            try {
                using (var file = File.Open(SavefileName, FileMode.Open, FileAccess.Read)) {
                    var bf = new BinaryFormatter();
                    scoreboard = bf.Deserialize(file) as List<HighScoreEntry>;
                }
            } catch {
            }

            if (scoreboard == null) {
                PopulateScoreboardWithDefaults();
                Save();
            }
        }

        private void PopulateScoreboardWithDefaults()
        {
            lock (scoreboardLock) {
                scoreboard = Enumerable
                    .Range(1, 10)
                    .Select((val, pos) => new HighScoreEntry { Name = $"Player {pos + 1}", Score = (11 - val) * 10 })
                    .ToList();
            }
        }

        internal void Save()
        {
            if (!canSave)
                return; // Don't even try to save. The user has been notified.
            try {
                using (var file = File.Open(SavefileName, FileMode.Create, FileAccess.Write)) {
                    var bf = new BinaryFormatter();
                    lock (scoreboardLock) {
                        bf.Serialize(file, scoreboard);
                    }
                }
            } catch {
                MessageBox.Show("The highscores cannot be saved.", "Cannot save highscores", MessageBoxButton.OK, MessageBoxImage.Warning);
                canSave = false;
            }
        }
    }

    /// <summary>
    /// An entry in the highscore table.
    /// </summary>
    [Serializable]
    internal sealed class HighScoreEntry
    {
        private int _score;

        /// <summary>
        /// The name of the player.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The score the player achieved.
        /// </summary>
        public int Score
        {
            get { return _score; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "The score must be larger than zero.");
                _score = value;
            }
        }
    }
}
