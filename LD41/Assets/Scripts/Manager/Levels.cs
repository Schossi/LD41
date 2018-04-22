using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Manager
{
    public static class Levels
    {
        public static Level GetLevel(int level)
        {
            switch (level)
            {
                case 1:
                    return Level.Level1;
                case 2:
                    return Level.Level2;
                case 3:
                    return Level.Level3;
                default:
                    return Level.Level1;
            }
        }
    }

    public class Level
    {
        public static Level Level1
        {
            get
            {
                return new Level(1, 0.20f, GetLevel1Waves());
            }
        }
        public static Level Level2
        {
            get
            {
                return new Level(2, 0.25f, GetLevel2Waves());
            }
        }
        public static Level Level3
        {
            get
            {
                return new Level(3, 0.35f, GetLevel3Waves());
            }
        }

        public int Number { get; set; }
        public float EnemySpeed { get; set; }
        public List<Wave> Waves { get; set; }

        public Level(int number,float enemySpeed,List<Wave> waves)
        {
            Number = number;
            EnemySpeed = enemySpeed;
            Waves = waves;
        }

        /*
         
            waves.Add(new Wave(
"XXXXXXX" +
"XXXXXXX"
            ));
             
             */

        public static List<Wave> GetLevel1Waves()
        {
            List<Wave> waves = new List<Wave>();
            
            waves.Add(new Wave(
"XX111XX" +
"XXX1XXX"
            ));

            waves.Add(new Wave(
"X11X11X" +
"X11X11X"
            ));

            waves.Add(new Wave(
"X1XXX1X" +
"XXX2XXX"
            ));

            waves.Add(new Wave(
"X11111X" +
"XX111XX"
            ));

            waves.Add(new Wave(
"X11111X" +
"XX2X2XX"
            ));

            waves.Add(new Wave(
"X2X2X2X" +
"XX2X2XX"
            ));

            waves.Add(new Wave(
"X2X1X2X" +
"1112111"
            ));

            waves.Add(new Wave(
"22XXX22" +
"22XXX22"
            ));

            return waves;
        }

        public static List<Wave> GetLevel2Waves()
        {
            List<Wave> waves = new List<Wave>();

            waves.Add(new Wave(
"X12221X" +
"XX121XX"
            ));

            waves.Add(new Wave(
"1221221" +
"1221221"
            ));

            waves.Add(new Wave(
"X12X21X" +
"XXX3XXX"
            ));

            waves.Add(new Wave(
"X11111X" +
"X31213X"
            ));

            waves.Add(new Wave(
"2XX3XX2" +
"332X233"
            ));

            waves.Add(new Wave(
"X2X2X2X" +
"XX2X2XX"
            ));

            waves.Add(new Wave(
"2121212" +
"3232323"
            ));

            waves.Add(new Wave(
"3333333" +
"2221222"
            ));

            return waves;
        }

        public static List<Wave> GetLevel3Waves()
        {
            List<Wave> waves = new List<Wave>();

            waves.Add(new Wave(
"11XXX11" +
"11XXX11"
            ));

            waves.Add(new Wave(
"X11111X" +
"XX222XX"
            ));

            waves.Add(new Wave(
"X12221X" +
"X33X33X"
            ));

            waves.Add(new Wave(
"11XXX11" +
"XX333XX"
            ));

            waves.Add(new Wave(
"1111111" +
"3X222X3"
            ));

            waves.Add(new Wave(
"2222222" +
"1111111"
            ));

            waves.Add(new Wave(
"XXXXXXX" +
"3333333"
            ));

            waves.Add(new Wave(
"3X3X3X3" +
"1212121"
            ));

            return waves;
        }
    }

    public class Wave
    {
        public string Content { get; set; }

        public Wave(string content)
        {
            Content = content;
        }

        public List<WaveEntry> GetEntries()
        {
            List<WaveEntry> entries = new List<WaveEntry>();

            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    int index = x + y * 7;
                    char c = Content[index];

                    switch (c)
                    {
                        case '1':
                            entries.Add(new WaveEntry(x, y, 1));
                            break;
                        case '2':
                            entries.Add(new WaveEntry(x, y, 2));
                            break;
                        case '3':
                            entries.Add(new WaveEntry(x, y, 3));
                            break;
                        default:
                            break;
                    }
                }
            }

            return entries;
        }
    }

    public class WaveEntry
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int EnemyLevel { get; set; }

        public WaveEntry(int x,int y,int enemyLevel)
        {
            X = x;
            Y = y;
            EnemyLevel = enemyLevel;
        }
    }
}
