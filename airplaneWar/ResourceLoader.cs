
//using System.Drawing.Imaging;

//namespace airplaneWar
//{
//    // ResourceLoader.cs
//    public static class ResourceLoader
//    {
//        private static readonly Dictionary<string, Texture2D> _textures = new();

//        public static void LoadTextures()
//        {
//            LoadTexture("player", Properties.Resources.player_ship);
//            LoadTexture("enemy1", Properties.Resources.enemy_fighter);
//        }

//        private static void LoadTexture(string key, Bitmap bitmap)
//        {
//            using var stream = new MemoryStream();
//            bitmap.Save(stream, ImageFormat.Png);
//            _textures[key] = Texture2D.FromStream(GraphicsDevice, stream);
//        }
//    }
//}