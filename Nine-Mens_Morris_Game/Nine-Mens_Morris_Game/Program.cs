using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Nine_Mens_Morris_Game
{
    static class Program
    {
        public static List<Játékos> játékosok = new List<Játékos>();
        public static List<Pont> pontok = new List<Pont>();
        public static Dictionary<Játékos, List<string[]>> malmok = new Dictionary<Játékos, List<string[]>>();
        public static Dictionary<int, int[]> oszlopRendezés = new Dictionary<int, int[]>();
        public static string[] bábúk = { "$", "O", "X" };

        public class Játékos
        {
            public string név;
            public string bábú;
            public int bábúkSzáma;
            public bool utolsóBábú;

            public Játékos(string név, string bábú, int bábúkSzáma)
            {
                this.név = név;
                this.bábú = bábú;
                this.bábúkSzáma = bábúkSzáma;
                this.utolsóBábú = false;
            }
        }

        public class Pont
        {
            public int id;
            public string név;
            public int sor;
            public int oszlop;
            public string érték;

            public Pont(int id, string név, int sor, int oszlop, string érték)
            {
                this.id = id;
                this.név = név;
                this.sor = sor;
                this.oszlop = oszlop;
                this.érték = érték;
            }
        }

        public class Tábla
        {
            public static void PontokLétrehozása()
            {
                //Oszlopba rendezés szabályai
                oszlopRendezés.Add(1, new int[] { 1, 10, 22 });
                oszlopRendezés.Add(2, new int[] { 4, 11, 19 });
                oszlopRendezés.Add(3, new int[] { 7, 12, 16 });
                oszlopRendezés.Add(4, new int[] { 2, 5, 8 });
                oszlopRendezés.Add(5, new int[] { 17, 20, 23 });
                oszlopRendezés.Add(6, new int[] { 9, 13, 18 });
                oszlopRendezés.Add(7, new int[] { 6, 14, 21 });
                oszlopRendezés.Add(8, new int[] { 3, 15, 24 });

                int i = 1;
                for (int s = 1; s < 9; s++)
                {
                    for (int o = 1; o < 4; o++)
                    {
                        int oszlop = oszlopRendezés.Where(x => x.Value.Contains(i)).ToArray()[0].Key;
                        pontok.Add(new Pont(i, $"p{i}", s, oszlop, bábúk[0]));
                        i++;
                    }
                }
            }

            public static void TáblaKiírás()
            {
                //Segédlet
                Console.WriteLine($"Malom Játék {Environment.NewLine}{Environment.NewLine}Segédlet:{Environment.NewLine}");
                Console.WriteLine($"{játékosok[0].név} bábúja: {játékosok[0].bábú}, bábúinak száma: {játékosok[0].bábúkSzáma}");
                Console.WriteLine($"{játékosok[1].név} bábúja: {játékosok[1].bábú}, bábúinak száma: {játékosok[1].bábúkSzáma} {Environment.NewLine}");
                Console.WriteLine("1-------------------2--------------------3");
                Console.WriteLine("|                   |                    |");
                Console.WriteLine("|    4--------------5--------------6     |");
                Console.WriteLine("|    |              |              |     |");
                Console.WriteLine("|    |       7------8------9       |     |");
                Console.WriteLine("|    |       |             |       |     |");
                Console.WriteLine("10---11------12            13------14----15");
                Console.WriteLine("|    |       |             |       |     |");
                Console.WriteLine("|    |       16-----17-----18      |     |");
                Console.WriteLine("|    |              |              |     |");
                Console.WriteLine("|    19-------------20-------------21    |");
                Console.WriteLine("|                   |                    |");
                Console.WriteLine("22------------------23------------------24");
                //Játékfelület
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}Játéktábla:{Environment.NewLine}");
                Console.WriteLine($"{pontok[0].érték}-------------------{pontok[1].érték}--------------------{pontok[2].érték}");
                Console.WriteLine($"|                   |                    |");
                Console.WriteLine($"|    {pontok[3].érték}--------------{pontok[4].érték}--------------{pontok[5].érték}     |");
                Console.WriteLine($"|    |              |              |     |");
                Console.WriteLine($"|    |       {pontok[6].érték}------{pontok[7].érték}------{pontok[8].érték}       |     |");
                Console.WriteLine($"|    |       |             |       |     |");
                Console.WriteLine($"{pontok[9].érték}----{pontok[10].érték}-------{pontok[11].érték}             {pontok[12].érték}-------{pontok[13].érték}-----{pontok[14].érték}");
                Console.WriteLine($"|    |       |             |       |     |");
                Console.WriteLine($"|    |       {pontok[15].érték}------{pontok[16].érték}------{pontok[17].érték}       |     |");
                Console.WriteLine($"|    |              |              |     |");
                Console.WriteLine($"|    {pontok[18].érték}--------------{pontok[19].érték}--------------{pontok[20].érték}     |");
                Console.WriteLine($"|                   |                    |");
                Console.WriteLine($"{pontok[21].érték}-------------------{pontok[22].érték}--------------------{pontok[23].érték}" + Environment.NewLine);
                Console.ResetColor();
            }
        }

        public class Játék
        {
            public static void Köszöntő()
            {
                MessageBox.Show("A játék engedélyezi a csiki csuki módszert, de nem javasoljuk használatát." + Environment.NewLine + "Jó játékot kívánunk! A továbbhaladáshoz nyomj meg egy gombot."); //Domi to customize
            }

            public static void JátékosokLétrehozása()
            {
                for (int i = 1; i < 3; i++)
                {
                    string név;
                    using (InputNameForm inputNameForm = new InputNameForm())
                    {
                        if (inputNameForm.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
                        {
                            név = inputNameForm.Név;
                            játékosok.Add(new Játékos(név, bábúk[i], 0));
                            malmok.Add(játékosok[i - 1], new List<string[]>());
                        } 
                    };  
                }
            }
        }



        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Játék.Köszöntő();
            Játék.JátékosokLétrehozása();
            Tábla.PontokLétrehozása();
            Tábla.TáblaKiírás();
        }
    }
}
