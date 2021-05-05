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

        #region Hibaüzenetek
        public static string e1 = "Hibás érték - Nem számot adtál meg!";
        public static string e2 = "Hibás érték - Adj meg egy számot 1 és 24 között!";
        public static string e3 = "Hibás érték - Kiütéshez használj az ellenfél által elfoglalt mezőt";
        public static string e4 = "Hibás érték - A mező már foglalt";
        public static string e5 = "Hibás érték - Ellépni csak a saját bábúddal tudsz";
        public static string e6 = "Hibás érték - Lépni csak azonos sorban vagy oszlopban lehet szomszédos helyre";
        public static string e7 = "Hibás érték - Nem lehet a kiindulási helyre lépni!";
        public static string e8 = "Hibás érték - Lépni csak szomszédos helyre lehet";
        public static string e9 = "Hibás érték - Az adott pontról nem lehet hova ellépni";
        #endregion

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

            public void LerakásKérdezz(bool kiütés)
            {
                int válasz;
                string üzenet = kiütés ? $"{this.név}, adj meg egy számot kiütéshez" : $"{this.név}, adj meg egy számot";
                MainForm mainForm = new MainForm();
                mainForm.kérdés = üzenet;

                if (mainForm.ShowDialog() == DialogResult.OK)
                {
                    string input = mainForm.textBox1.Text;
                    válasz = VálaszValidálás(kiütés, false, false, input);
                    pontok[válasz - 1].érték = kiütés ? bábúk[0] : this.bábú; //Tábla frissítése a játékos bábújával
                    if (kiütés && malmok[this].Any(malom => malom.Contains(pontok[válasz - 1].név))) { this.malomRekordTörlés(válasz); }
                }
            }

            public int VálaszValidálás(bool kiütés, bool lépegetés, bool másodikÉrték, string input, int honnan = 0)
            {
                bool számRendben;
                int válasz;
                do
                {
                    számRendben = int.TryParse(input, out válasz);
                    if (!számRendben) { Console.WriteLine(e1); }
                    if (számRendben && (válasz <= 0 || válasz >= 25)) { Console.WriteLine(e2); számRendben = false; }
                    if (számRendben && (kiütés && (pontok[válasz - 1].érték == "$" || pontok[válasz - 1].érték == this.bábú))) { Console.WriteLine(e3); számRendben = false; }
                    if (számRendben && !lépegetés && ((!kiütés) && pontok[válasz - 1].érték != "$")) { Console.WriteLine(e4); számRendben = false; }
                    if (számRendben && lépegetés && pontok[válasz - 1].érték != this.bábú) { Console.WriteLine(e5); számRendben = false; }
                    if (számRendben && !lépegetés && másodikÉrték)
                    {
                        int honnanSor = pontok[honnan - 1].sor;
                        int honnanOszlop = pontok[honnan - 1].oszlop;
                        int hovaSor = pontok[válasz - 1].sor;
                        int hovaOszlop = pontok[válasz - 1].oszlop;
                        if (honnanSor != hovaSor && honnanOszlop != hovaOszlop) { Console.WriteLine(e6); számRendben = false; }
                        if (honnanSor == hovaSor && honnanOszlop == hovaOszlop) { Console.WriteLine(e7); számRendben = false; }
                        bool vízszintesLépésRendben = true;
                        bool függőlegesLépésRendben = true;

                        #region-Vízszintes-lépés
                        if (honnanSor == hovaSor)
                        {
                            //vízszintesen léptünk
                            Pont[] sajátSorPontjai = pontok.Where(pont => pont.sor == pontok[honnan - 1].sor).ToArray();
                            int s = Array.IndexOf(sajátSorPontjai, pontok[honnan - 1]);
                            switch (s)
                            {
                                case 0:
                                    if (sajátSorPontjai[1].id != pontok[válasz - 1].id) { vízszintesLépésRendben = false; }
                                    break;
                                case 1:
                                    if (sajátSorPontjai[0].id != pontok[válasz - 1].id && sajátSorPontjai[2].id != pontok[válasz - 1].id) { vízszintesLépésRendben = false; }
                                    break;
                                case 2:
                                    if (sajátSorPontjai[1].id != pontok[válasz - 1].id) { vízszintesLépésRendben = false; }
                                    break;
                            }
                        }
                        #endregion

                        #region-Függőleges-lépés
                        if (honnanOszlop == hovaOszlop)
                        {
                            //Függőlegesen léptünk
                            Pont[] sajátOszlopPontjai = pontok.Where(pont => pont.oszlop == pontok[honnan - 1].oszlop).ToArray();
                            int s = Array.IndexOf(sajátOszlopPontjai, pontok[honnan - 1]);
                            switch (s)
                            {
                                case 0:
                                    if (sajátOszlopPontjai[1].id != pontok[válasz - 1].id) { függőlegesLépésRendben = false; }
                                    break;
                                case 1:
                                    if (sajátOszlopPontjai[0].id != pontok[válasz - 1].id && sajátOszlopPontjai[2].id != pontok[válasz - 1].id) { függőlegesLépésRendben = false; }
                                    break;
                                case 2:
                                    if (sajátOszlopPontjai[1].id != pontok[válasz - 1].id) { függőlegesLépésRendben = false; }
                                    break;
                            }
                        }
                        #endregion

                        if ((!függőlegesLépésRendben) || (!vízszintesLépésRendben)) { Console.WriteLine(e8); számRendben = false; }
                    }

                    #region-Van-E-Valid-Opció
                    if (számRendben && lépegetés && !másodikÉrték)
                    {
                        //Megnézzük hogy van-e bármilyen lehetséges mozgás a lépegetés kiindulópontjából
                        Pont[] sajátSorPontjai = pontok.Where(pont => pont.sor == pontok[válasz - 1].sor).ToArray();
                        int s = Array.IndexOf(sajátSorPontjai, pontok[válasz - 1]);
                        bool nemLehetSorbanLépni = false;
                        switch (s)
                        {
                            case 0:
                                if (sajátSorPontjai[1].érték != bábúk[0]) { nemLehetSorbanLépni = true; }
                                break;
                            case 1:
                                if (sajátSorPontjai[0].érték != bábúk[0] && sajátSorPontjai[2].érték != bábúk[0]) { nemLehetSorbanLépni = true; }
                                break;
                            case 2:
                                if (sajátSorPontjai[1].érték != bábúk[0]) { nemLehetSorbanLépni = true; }
                                break;
                        }

                        Pont[] sajátOszlopPontjai = pontok.Where(pont => pont.oszlop == pontok[válasz - 1].oszlop).ToArray();
                        int o = Array.IndexOf(sajátOszlopPontjai, pontok[válasz - 1]);
                        bool nemLehetOszlopbanLépni = false;
                        switch (o)
                        {
                            case 0:
                                if (sajátOszlopPontjai[1].érték != bábúk[0]) { nemLehetOszlopbanLépni = true; }
                                break;
                            case 1:
                                if (sajátOszlopPontjai[0].érték != bábúk[0] && sajátOszlopPontjai[2].érték != bábúk[0]) { nemLehetOszlopbanLépni = true; }
                                break;
                            case 2:
                                if (sajátOszlopPontjai[1].érték != bábúk[0]) { nemLehetOszlopbanLépni = true; }
                                break;
                        }
                        if (nemLehetSorbanLépni && nemLehetOszlopbanLépni) { Console.WriteLine(e9); számRendben = false; }
                    }
                    #endregion

                } while (!számRendben);
                return válasz;
            }

            public void malomRekordTörlés(int válasz)
            {
                int index = Array.FindIndex(malmok[this].ToArray(), malom => malom.Contains(pontok[válasz - 1].név));
                malmok[this].Remove(malmok[this][index]);
            }

            public void MalomEllenőrzés()
            {
                //Minden sort és oszlopot megnézünk hogy van e 3 ugyanolyan érték
                for (int i = 1; i < 9; i++)
                {
                    IEnumerable<Pont> sorPontjai = pontok.Where(pont => pont.sor == i);
                    IEnumerable<Pont> oszlopPontjai = pontok.Where(pont => pont.oszlop == i);
                    BelsőEllenőrzés(sorPontjai);
                    BelsőEllenőrzés(oszlopPontjai);
                }

                void BelsőEllenőrzés(IEnumerable<Pont> pontok)
                {
                    string[] értékek = pontok.Select(pont => pont.érték).ToArray();       //Deklarálunk egy string tömböt az adott sor/oszlop pontjainak értékére
                    if (!(értékek.Any(érték => érték != értékek[0])) && this.bábú == értékek[0])
                    {
                        string[] malomPontok = pontok.Select(pont => pont.név).ToArray(); //Megvizsgáljuk hogy bármelyik érték a fent létrehozott tömbben eltér-e a 0.dik elemtől és hogy az adott játékoshoz tartozik e.
                        if (!(malmok[this].Any(malom => malom.SequenceEqual(malomPontok))))
                        {
                            Console.WriteLine($"{this.név}, malmod van! ({String.Join(" - ", malomPontok)})");
                            malmok[this].Add(malomPontok);
                            this.LerakásKérdezz(true); //Kiütés
                        }
                    }
                }
            }

            public void SzámláljBábút()
            {
                this.bábúkSzáma = pontok.Where(pont => pont.érték == this.bábú).Count();
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

            public static void Lerakás(List<Játékos> játékosok)
            {
                for (int i = 1; i <= 18; i++)
                {
                    int idx = ((i % 2 != 0) ? 0 : 1);
                    játékosok[idx].LerakásKérdezz(false);
                    játékosok[idx].MalomEllenőrzés();
                    játékosok.ForEach(játékos => játékos.SzámláljBábút());
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
            Játék.Lerakás(játékosok);
        }
    }
}
