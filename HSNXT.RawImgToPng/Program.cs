using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace HSNXT.RawImgToPng
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            foreach (var arg in args)
            {
                if (!File.Exists(arg))
                {
                    Console.WriteLine($"ERR: No file '{arg}'");
                    continue;
                }

                using var reader = new BinaryReader(File.OpenRead(arg));
                var version = reader.ReadInt32();
                if (version != 1)
                {
                    Console.WriteLine($"WARN: Invalid version {version}, expected version 1.");
                }

                var width = reader.ReadInt32();
                var height = reader.ReadInt32();

                using var bmp = new Bitmap(width, height);
                    
                for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                {
                    var b = reader.ReadByte();
                    var g = reader.ReadByte();
                    var r = reader.ReadByte();
                    var a = reader.ReadByte();
                    bmp.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                }
                
                bmp.Save(arg + ".png", ImageFormat.Png);
            }
        }
    }
}
