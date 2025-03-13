
//using System;
//using System.IO;
//using System.Xml.Serialization;

//namespace airplaneWar.Core.Manager
//{
//    public static class ConfigManager
//    {
//        private static readonly string ConfigDir;
//        private static readonly string ConfigPath;

//        static ConfigManager()
//        {
//            // 获取用户专属目录（如 C:\Users\用户名\AppData\Roaming\MyGame）
//            ConfigDir = Path.Combine(
//                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
//                "MyGame"
//            );
//            ConfigPath = Path.Combine(ConfigDir, "game_config.xml");

//            // 确保目录存在
//            if (!Directory.Exists(ConfigDir))
//            {
//                Directory.CreateDirectory(ConfigDir);
//            }
//        }

//        public static GameConfig LoadConfig()
//        {
//            if (!File.Exists(ConfigPath))
//            {
//                var defaultConfig = new GameConfig();
//                SaveConfig(defaultConfig);
//                return defaultConfig;
//            }

//            try
//            {
//                var serializer = new XmlSerializer(typeof(GameConfig));
//                using (var reader = new StreamReader(ConfigPath))
//                {
//                    return (GameConfig)serializer.Deserialize(reader);
//                }
//            }
//            catch
//            {
//                return new GameConfig();
//            }
//        }

//        public static void SaveConfig(GameConfig config)
//        {
//            var serializer = new XmlSerializer(typeof(GameConfig));
//            using (var writer = new StreamWriter(ConfigPath))
//            {
//                serializer.Serialize(writer, config);
//            }
//        }
//    }
//}