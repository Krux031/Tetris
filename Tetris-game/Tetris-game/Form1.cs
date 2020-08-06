using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace Tetris_game
{
    public partial class Form1 : Form
    {
        ScriptEngine pyEngine = null;
        ScriptScope pyScope = null;

        Graphics g;
        Graphics f;
        Kockica check; //kontrolna kockica
        Kockica ponisti; //kontrolna kockica

        //svaki objekt se sastoji od cetiri kockice a,b,c,d
        Kockica a; 
        Kockica b;
        Kockica c;
        Kockica d;
        //svaka kockica ima svoju poziciju
        int a_position; 
        int b_position;
        int c_position;
        int d_position;

        //objekti koji se crtaju kako bi se prikazao sljedeci objekt
        Kockica a_s;
        Kockica b_s;
        Kockica c_s;
        Kockica d_s;
        //svaka kockica sljedeceg oblika ima svoju poziciju
        int a_Sposition;
        int b_Sposition;
        int c_Sposition;
        int d_Sposition;

        //kontrolne pozicije kockica a,b,c,d
        int a_lista;
        int b_lista;
        int c_lista;
        int d_lista;

        int insert_ponisti; //kontrolna pozicija

        //svaki red ima svoju listu koja sluzi kako bi se znalo jeli red popunjen
        List<int> ponisti_1 = new List<int>();
        List<int> ponisti_2 = new List<int>();
        List<int> ponisti_3 = new List<int>();
        List<int> ponisti_4 = new List<int>();
        List<int> ponisti_5 = new List<int>();
        List<int> ponisti_6 = new List<int>();
        List<int> ponisti_7 = new List<int>();
        List<int> ponisti_8 = new List<int>();
        List<int> ponisti_9 = new List<int>();
        List<int> ponisti_10 = new List<int>();
        List<int> ponisti_11 = new List<int>();
        List<int> ponisti_12 = new List<int>();
        List<int> ponisti_13 = new List<int>();
        List<int> ponisti_14 = new List<int>();
        List<int> ponisti_15 = new List<int>();
        List<int> ponisti_16 = new List<int>();
        List<int> ponisti_17 = new List<int>();
        List<int> ponisti_18 = new List<int>();
        List<int> ponisti_19 = new List<int>();
        List<int> ponisti_20 = new List<int>();

        //lista kockica koje su predstavljene kao rubne kockice
        List<int> rub = new List<int> { 1, 12, 13, 24, 25, 36, 37, 48, 49, 60, 61, 72, 73, 84, 85, 96, 97, 108, 109, 120, 121, 132, 133, 144, 145, 156, 157, 168, 169, 180, 181, 192, 193, 204, 205, 216, 217, 228, 229, 240, 241, 252, 253, 264, 265, 266, 267, 268, 269, 270, 271, 272, 273, 274, 275, 276 };
        List<Kockica> kockica_lista = new List<Kockica>(); //lista svih kockica, ukljucujuci i rubne
        List<Kockica> kockica_lista_sljedeci = new List<Kockica>(); //lista kockica koji pokazaju na sljedeci element
        List<string> lista_oblika =new List<string> { "ravna", "kvadrat", "t_oblik", "z_oblik", "s_oblik", "j_oblik", "l_oblik" };

        Random randOblik=new Random();

        const int sirina = 10; //sirina polja
        const int visina = 20; //visina polja
        int brojac = 1; //pomocna varijabla
        int stanje; //pomocna varijabla koja govori o rotaciji oblika
        int p_stanje = 0; //stanje koje se korisiti za pauzu igre
        int ponisti_event = 0;
        int randBroj; //pomocna varijabla
        string oblik; //varijabla u koju se sprema informacije o trenutnom obliku
        string sljedeci_oblik; //varijabla u koju se sprema informacije o sljedecem obliku

        //varijable potrebne za game over:
        Font gm_font = new Font("Arial", 24);
        Brush gm_brush = new SolidBrush(Color.Black);
        PointF gm_pointF = new PointF(10, 180);
        bool gm = true;

        //funkcije cija je definicja napisana u pythonu
        dynamic iznos_bodova;
        dynamic iznos_razine;
        dynamic iznos_vrijeme;
        dynamic dodaj_liniju;
        dynamic reset_statistika;
        dynamic najbolji_igrac;
        dynamic dohvati_listu;
        dynamic dodaj_igraca;

        public Form1()
        {
            InitializeComponent();
            pyEngine = Python.CreateEngine();
            pyScope = pyEngine.CreateScope();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //kreiranje Graphics po kojem ce se crtati objekti i polje tetrisa
            g = pb_polje.CreateGraphics();
            //kreiranje Graphics po kojem se crta sljedeci oblik po redu
            f = pb_sljedeci.CreateGraphics();
            //inicijalizacija svih kockica u polju po kojem ce se kretati objekti
            inicijalizacija_polja();

            ScriptSource ss = pyEngine.CreateScriptSourceFromFile("statistika.py");
            ss.Execute(pyScope);

            //dodjela definicije funkcije iz python-a funkciji u C#
            iznos_bodova = pyScope.GetVariable("bodovi_od_linija");
            iznos_razine = pyScope.GetVariable("razine");
            iznos_vrijeme = pyScope.GetVariable("postavi_vrijeme");
            dodaj_liniju = pyScope.GetVariable("linije");
            reset_statistika = pyScope.GetVariable("reset");
            najbolji_igrac = pyScope.GetVariable("top_igrac");
            dohvati_listu = pyScope.GetVariable("top_lista");
            dodaj_igraca = pyScope.GetVariable("spremi_igraca");

            //pozivanje funkcije iz statistika.py za ispis najboljeg igraca
            najbolji_igrac(this);
        }

        private void btn_nova_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White); //ciscenje prethodnog Graphics-a kako bi se nacrtao novi

            //u slucaju reset igrice ili game over-a potrebno je sljedece:
            reset_statistika();
            ponistiSVE_Iznadliste(20); //ponistavaju se sve spremljene kockice u listi
            foreach (Kockica kockica in kockica_lista) //svim kockicama se ispunjnost stavlja na false
            {
                kockica.ispunjen = false;
            }

            foreach(Kockica kockica in kockica_lista_sljedeci)
            {
                kockica.ispunjen = false;
            }

            gm = true;
            brojac = 0;

            crtanje_polja(); //crtanje kockica u polju
            uzmi_sljedeci_oblik(); //uzimanje sljedeceg oblika
            postavi_oblik(); //kako je to prvi oblik odma cemo ga staviti u polje
            uzmi_sljedeci_oblik(); //potrebno je uzeti sljedeci oblik kako bi prikazali sljedeci oblik
            nacrtaj_sljedeci_oblik(); //crtanje objekta u odredenu kockicu za sljedeci oblik
            nacrtaj_oblik(); //crtanje objekta u odredenu kockicu

            btn_nova.Enabled = false; //onemogucivanje buttona "nova igra" i "top" kako bi se mogli kretati strelicama
            btn_top.Enabled = false;
            timer_game.Enabled = true; //pokretanje timer-a kako bi objekti padali u zadanom vremenu            
        }

        private void btn_top_Click(object sender, EventArgs e)
        {
            //ispis liste igraca sa njihovim rezultatima preko Message Box-a
            string string_top_igraca;
            string_top_igraca = dohvati_listu();
            MessageBox.Show(string_top_igraca, "Top Lista");
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.P)
            { //pauziranje igre
                p_stanje++; //pocetna vrijednost je 0
                if(p_stanje%2==0)
                {
                    timer_game.Start(); //pokretanje timera nakon pauze
                    btn_nova.Enabled = false; //onemogucivanje koristenje buttona
                    btn_top.Enabled = false;
                }

                else{
                    timer_game.Stop(); //zaustavljanje timera zbog pauze
                    btn_nova.Enabled = true; //mogucnost koristenja buttona
                    btn_top.Enabled = true;
                }
            }

            if (gm == true) //ako varijabla poprimi false vrijednost to znaci da je GAME OVER, pa s time iskljucuje komande za kretanje
            {
                if (e.KeyCode == Keys.Up)
                {
                    rotiraj(); //strelica prema gore sluzi kako bi se objekt rotirao za 90°
                }

                if (e.KeyCode == Keys.Down)
                {
                    if (provjera_kretanja(a_position + 12) || provjera_kretanja(b_position + 12) || provjera_kretanja(c_position + 12) || provjera_kretanja(d_position + 12))
                    { //ovim if uvjetom se provjerava moze li objekt (svaka njegova kockica) ici prema dolje. Gornja kockica ima vrijednost ID=position, dok kockica ispod iste ima vrijednost ID=position+12
                      //ako objekt ne moze ici prema dolje, svaka njegova kockica se oznacava sa true, odnosno da je ispunjena.
                        a.ispunjen = true;
                        b.ispunjen = true;
                        c.ispunjen = true;
                        d.ispunjen = true;
                        /*
                         Kako se taj objekt više ne može kretati(osim lijevo i desno u roku 1 sekunde), poziva se funkcija ponisti_linije() kako bi se provjerilo
                         postoji li ispunjeni red. Ako postoji taj red se briše dok se svi redovi iznad spuštaju.
                        */
                        ponisti_event = 2;
                        Thread.Sleep(200);
                        ponisti_linije();
                        postavi_oblik();
                        uzmi_sljedeci_oblik();
                        nacrtaj_sljedeci_oblik();
                        nacrtaj_oblik();
                    }
                    else pomak(12); // u slucaju da se objekt moze kretati prema dolje, poziva se funkcija pomak(int value);
                }
                if (e.KeyCode == Keys.Left)
                {
                    if (provjera_kretanja(a_position - 1) || provjera_kretanja(b_position - 1) || provjera_kretanja(c_position - 1) || provjera_kretanja(d_position - 1))
                    {
                        //provjera, moze li se objekt kretati prema lijevo. Ako ne moze, treba ostaviti objekt u trenutnom polozaju
                    }
                    else pomak(-1); //ako moze ici prema desno, sve kockice objekta treba pomaknuti za ID=polozaj-1
                }
                if (e.KeyCode == Keys.Right)
                {
                    if (provjera_kretanja(a_position + 1) || provjera_kretanja(b_position + 1) || provjera_kretanja(c_position + 1) || provjera_kretanja(d_position + 1))
                    {
                        //provjera, moze li se objekt kretati prema desno. Ako ne moze, treba ostaviti objekt u trenutnom polozaju
                    }
                    else pomak(1); //ako moze ici prema lijevo, sve kockice objekta treba pomaknuti za ID=polozaj+1
                }
            }            
        }

        private void inicijalizacija_polja()
        {
            /* Za visinu, kreće se od -40 da se inicijalizira dva reda iznad vidljivog Graphics-a na formi, 
             kako bi se omogucila rotacija objekta pri samom vrhu. Takodjer se uzima još jedan red pri dn
             (ispod vidljivog Graphics-a) kako bi se znalo dokle mogu padati kockice objekta.
             
             Za sirinu, uzima se jedan red za lijevu i desnu stranu (nevidljivog dijela) Graphics-a 
             kako bi se oznacio zid dokle se mogu kretati kockice objekta.
             */
            for (int i = -40; i <= (visina * 20); i += 20)
            { 
                for (int j = -20; j <= (sirina * 20); j += 20)
                {
                    Kockica kockica = new Kockica(brojac, j, i); //kreiranje kockice polja po kojem se krecu objekti
                    kockica_lista.Add(kockica); // stavljanje svake kockice u listu koja sluzi za rad u aplikaciji
                    brojac++; //brojac sluzi za oznacavanje ID-a svake kockice
                }
            }

            brojac = 17; //krece se od ID=17 kako bi bile iste pozicije oblika u Graphics-u "f" i tijekom kreiranja oblika u Graphics-u "g"
            //na slican nacin se radi kockice koje ce pokazaivati sljedeci element u Graphics-u "f"
            for (int i = 0; i <= 60; i += 20)
            {
                for (int j = 0; j <= 60; j += 20)
                {
                    Kockica kockica = new Kockica(brojac, j, i);
                    kockica_lista_sljedeci.Add(kockica);
                    brojac++;
                }
                brojac += 8;
            }
            brojac = 0;
        }

        private void crtanje_polja()
        {
            foreach (Kockica kockica in kockica_lista)
            {
                kockica.nacrtaj_rub(g); //crtanje rubova svake kockice iz liste
            }
        }

        private bool provjera_kretanja(int value)
        {
            check = kockica_lista.Where(x => x.ID == value).FirstOrDefault(); //pronalazi kockicu za zadanu poziciju (predtsavljna varijablom value)
            if (check.ispunjen == true || rub.Contains(value)) return true; //ukoliko je ta kockica vec ispunjena ili je rubna kockica vraca true vrijednost, što znaci da se naš objekt ne moze kretati u tom smjeru
            else return false;
        }

        private void rotiraj()
        {
            int a_probna = a_position;
            int b_probna = b_position;
            int c_probna = c_position;
            int d_probna = d_position;

            //svaki objekt ima svoj nacin rotiranja

            #region Ravna
            if (oblik == "ravna")
            {
                switch (stanje)
                {
                    case 1: //u horizontalnom položaju
                        if (provjera_kretanja(a_probna - 22) == false && provjera_kretanja(b_probna - 11) == false && provjera_kretanja(d_probna + 11) == false)
                        {
                            pomak_rotiranja(-22, -11, 0, 11);
                            stanje = 2;
                            break;
                        }
                        if (provjera_kretanja(a_probna - 23) == false && provjera_kretanja(b_probna - 12) == false && provjera_kretanja(d_probna + 10) == false)
                        { //kompenzacija ako se ne moze izvrsiti prvi if
                            pomak_rotiranja(-23, -12, -1, 10);
                            stanje = 2;
                            break;
                        }
                        if (provjera_kretanja(d_probna + 11) == true)
                        { //ako je pri dnu
                            if (provjera_kretanja(a_probna - 34) == false && provjera_kretanja(b_probna - 23) == false && provjera_kretanja(b_probna-12)==false && provjera_kretanja(d_probna -1) == false)
                            {
                                pomak_rotiranja(-34, -23, -12, -1);
                                stanje = 2;
                                break;
                            }
                        }
                        break;

                    case 2: //u vertikalnom položaju
                        if (provjera_kretanja(a_probna + 22) == false && provjera_kretanja(b_probna + 11) == false && provjera_kretanja(d_probna - 11) == false)
                        {
                            pomak_rotiranja(22, 11, 0, -11);
                            stanje = 1;
                            break;
                        }

                        if (provjera_kretanja(a_probna + 22) == true) //ako a nije dobar
                        {
                            if (provjera_kretanja(a_probna + 23) == false && provjera_kretanja(b_probna + 12) == false && provjera_kretanja(c_probna + 1) == false && provjera_kretanja(d_probna - 10) == false)
                            { //ako je udaljen od lijevog zida za jednu kockicu, probaj napraviti kompenzaciju
                                stanje = 1;
                                pomak_rotiranja(23, 12, 1, -10);
                                break;
                            }
                            if (provjera_kretanja(a_probna + 24) == false && provjera_kretanja(b_probna + 13) == false && provjera_kretanja(c_probna + 2) == false && provjera_kretanja(d_probna - 9) == false)
                            { //ako je uz lijevi zid, probaj napraviti kompenzaciju
                                stanje = 1;
                                pomak_rotiranja(24, 13, 2, -9);
                                break;
                            }
                        }

                        if (provjera_kretanja(b_probna + 11) == true) //ako b nije dobar
                        {
                            if (provjera_kretanja(a_probna + 24) == false && provjera_kretanja(b_probna + 13) == false && provjera_kretanja(c_probna + 2) == false && provjera_kretanja(d_probna - 9) == false)
                            { //ako je uz lijevi predmet, probaj napraviti kompenzaciju
                                stanje = 1;
                                pomak_rotiranja(24, 13, 2, -9);
                                break;
                            }
                        }
                        if (provjera_kretanja(b_probna - 11) == true) //ako d nije dobar
                        {
                            if (provjera_kretanja(a_probna + 21) == false && provjera_kretanja(b_probna + 10) == false && provjera_kretanja(c_probna -1) == false && provjera_kretanja(d_probna - 12) == false)
                            { //ako je uz desni zid
                                stanje = 1;
                                pomak_rotiranja(21, 10, -1, -12);
                            }
                        }
                        break;
                }

            }
            #endregion

            #region T oblik
            if (oblik == "t_oblik")
            {
                switch (stanje)
                {
                    case 1: //gleda prema dolje
                        if (provjera_kretanja(c_probna - 13) == false)
                        {
                            rotacija_posebna_za_T(-13);
                            stanje = 2;
                            break;
                        }
                        break;
                    case 2: //gleda prema desno
                        if (provjera_kretanja(c_probna + 11) == false)
                        {
                            rotacija_posebna_za_T(11);
                            stanje = 3;
                            break;
                        }
                        if (provjera_kretanja(c_probna + 12) == false)
                        {
                            rotacija_posebna_za_T(12);
                            pomak_rotiranja(1, 1, 0, 1);
                            stanje = 3;
                            break;
                        }
                        break;
                    case 3: //gleda prema gore
                        if (provjera_kretanja(c_probna + 13) == false)
                        {
                            rotacija_posebna_za_T(13);
                            stanje = 4;
                            break;
                        }
                        if (provjera_kretanja(c_probna - 12) == false && provjera_kretanja(d_probna - 12) == false)
                        {
                            rotacija_posebna_za_T(1);
                            pomak_rotiranja(-12, -12, 0, -12);
                            stanje = 4;
                            break;
                        }
                        break;
                    case 4: //gleda prema lijevo
                        if (provjera_kretanja(c_probna - 11) == false)
                        {
                            rotacija_posebna_za_T(-11);
                            stanje = 1;
                            break;
                        }
                        if (provjera_kretanja(c_probna - 12) == false)
                        {
                            rotacija_posebna_za_T(-12);
                            pomak_rotiranja(-1, -1, 0, -1);
                            stanje = 1;
                            break;
                        }
                        break;
                }
            }
            #endregion

            #region Z oblik
            if (oblik == "z_oblik")
            {
                switch (stanje)
                {
                    case 1: //horizontalno
                        if(provjera_kretanja(a_probna + 2) == false && provjera_kretanja(d_probna + 11) == false)
                        {
                            pomak_rotiranja(2, 13, 0, 11);
                            stanje = 2;
                            break;
                        }
                        if (provjera_kretanja(a_probna - 10) == false && provjera_kretanja(b_probna + 1) == false)
                        { //uz pod
                            pomak_rotiranja(-10, 1, -12, -1);
                            stanje = 2;
                            break;
                        }
                        break;
                    case 2: //vertikalno
                        if (provjera_kretanja(a_probna - 2) == false && provjera_kretanja(b_probna - 13) == false)
                        {
                            pomak_rotiranja(-2, -13, 0, -11);
                            stanje = 1;
                            break;
                        }
                        if (provjera_kretanja(a_probna - 1) == false && provjera_kretanja(d_probna - 10) == false)
                        { //uz zid
                            pomak_rotiranja(-1, -12, 1, -10);
                            stanje = 1;
                            break;
                        }
                        if (provjera_kretanja(a_probna + 10) == false && provjera_kretanja(d_probna + 1) == false)
                        { //pri dnu
                            pomak_rotiranja(10, -1, 12, 1);
                            stanje = 1;
                            break;
                        }
                        break;
                }
            }
            #endregion

            #region S oblik
            if (oblik == "s_oblik")
            {
                switch (stanje)
                {
                    case 1: //horizontalno
                        if (provjera_kretanja(a_probna + 13) == false && provjera_kretanja(d_probna - 2) == false)
                        {
                            pomak_rotiranja(13, 0, 11, -2);
                            stanje = 2;
                            break;
                        }
                        if (provjera_kretanja(c_probna - 1) == false && provjera_kretanja(d_probna - 14) == false)
                        {
                            pomak_rotiranja(1, -12, -1, -14);
                            stanje = 2;
                            break;
                        }
                        break;
                    case 2: //vertikalno
                        if (provjera_kretanja(c_probna - 11) == false && provjera_kretanja(d_probna + 2) == false)
                        {
                            pomak_rotiranja(-13, 0, -11, 2);
                            stanje = 1;
                            break;
                        }
                        if (provjera_kretanja(a_probna - 14) == false && provjera_kretanja(d_probna + 1) == false)
                        {
                            pomak_rotiranja(-14, -1, -12, 1);
                            stanje = 1;
                            break;
                        }
                        break;
                }
            }
            #endregion

            #region J oblik
            if (oblik == "j_oblik")
            {
                switch (stanje)
                {
                    case 1:
                        if(provjera_kretanja(a_probna+13)==false && provjera_kretanja(c_probna-13)==false && provjera_kretanja(d_probna - 24) == false)
                        { //horizontalno prema dolje
                            pomak_rotiranja(13, 0, -13, -24);
                            stanje = 2;
                            break;
                        }
                        break;
                    case 2:
                        if (provjera_kretanja(a_probna - 11) == false && provjera_kretanja(c_probna + 11) == false && provjera_kretanja(d_probna - 2) == false)
                        { //vertikalno prema desno
                            pomak_rotiranja(-11, 0, 11, -2);
                            stanje = 3;
                            break;
                        }
                        if(provjera_kretanja(a_probna-10)==false && provjera_kretanja(b_probna+1)==false && provjera_kretanja(d_probna - 1) == false)
                        {
                            pomak_rotiranja(-10, 1, 12, -1);
                            stanje = 3;
                            break;
                        }
                        break;
                    case 3:
                        if (provjera_kretanja(a_probna - 13) == false && provjera_kretanja(c_probna + 13) == false && provjera_kretanja(d_probna + 24) == false)
                        { //horizontalno prema gore
                            pomak_rotiranja(-13, 0, 13, 24);
                            stanje = 4;
                            break;
                        }
                        if (provjera_kretanja(a_probna - 25) == false && provjera_kretanja(b_probna - 12) == false)
                        { 
                            pomak_rotiranja(-25, -12, 1, 12);
                            stanje = 4;
                            break;
                        }
                        break;
                    case 4:
                        if (provjera_kretanja(a_probna + 11) == false && provjera_kretanja(c_probna - 11) == false && provjera_kretanja(d_probna + 2) == false)
                        { //vertikalno prema lijevo
                            pomak_rotiranja(11, 0, -11, 2);
                            stanje = 1;
                            break;
                        }
                        if (provjera_kretanja(a_probna + 10) == false && provjera_kretanja(b_probna - 1) == false && provjera_kretanja(d_probna + 1) == false)
                        { 
                            pomak_rotiranja(10, -1, -12, 1);
                            stanje = 1;
                            break;
                        }
                        break;
                }
            }
            #endregion

            #region L oblik
            if (oblik == "l_oblik")
            {
                switch (stanje)
                {
                    case 1:
                        if (provjera_kretanja(a_probna - 13) == false && provjera_kretanja(c_probna + 13) == false && provjera_kretanja(d_probna + 2) == false)
                        { //horizontalno prema dolje
                            pomak_rotiranja(-13, 0, 13, 2);
                            stanje = 2;
                            break;
                        }
                        break;
                    case 2:
                        if (provjera_kretanja(a_probna + 11) == false && provjera_kretanja(c_probna - 11) == false && provjera_kretanja(d_probna - 24) == false)
                        { //vertikalno prema desno
                            pomak_rotiranja(11, 0, -11, -24);
                            stanje = 3;
                            break;
                        }
                        if (provjera_kretanja(b_probna + 1) == false && provjera_kretanja(c_probna - 10) == false && provjera_kretanja(d_probna - 23) == false)
                        {
                            pomak_rotiranja(12, 1, -10, -23);
                            stanje = 3;
                            break;
                        }
                        break;
                    case 3:
                        if (provjera_kretanja(a_probna + 13) == false && provjera_kretanja(c_probna - 13) == false && provjera_kretanja(d_probna - 2) == false)
                        { //horizontalno prema gore
                            pomak_rotiranja(13, 0, -13, -2);
                            stanje = 4;
                            break;
                        }
                        if (provjera_kretanja(b_probna - 12) == false && provjera_kretanja(c_probna - 25) == false && provjera_kretanja(d_probna - 14) == false)
                        {
                            pomak_rotiranja(1, -12, -25, -14);
                            stanje = 4;
                            break;
                        }
                        break;
                    case 4:
                        if (provjera_kretanja(a_probna - 11) == false && provjera_kretanja(c_probna + 11) == false && provjera_kretanja(d_probna + 24) == false)
                        { //vertikalno prema lijevo
                            pomak_rotiranja(-11, 0, 11, 24);
                            stanje = 1;
                            break;
                        }
                        if (provjera_kretanja(b_probna - 1) == false && provjera_kretanja(c_probna + 10) == false && provjera_kretanja(d_probna + 23) == false)
                        {
                            pomak_rotiranja(-12, -1, 10, 23);
                            stanje = 1;
                            break;
                        }
                        break;
                }
            }
            #endregion
        }

        private void rotacija_posebna_za_T(int c_value)
        { //pomak islican kao funkcija pomak_rotiranja()
            a.isprazni(g);
            c.isprazni(g);
            d.isprazni(g);

            a.nacrtaj_rub(g);
            c.nacrtaj_rub(g);
            d.nacrtaj_rub(g);

            a_position = d_position;
            d_position = c_position;
            c_position += c_value;

            nacrtaj_oblik();
        }

        private void pomak_rotiranja(int a_value, int b_value, int c_value, int d_value)
        { //slicno kao i funkcija pomak()
            a.isprazni(g);
            b.isprazni(g);
            c.isprazni(g);
            d.isprazni(g);

            a.nacrtaj_rub(g);
            b.nacrtaj_rub(g);
            c.nacrtaj_rub(g);
            d.nacrtaj_rub(g);

            a_position += a_value;
            b_position += b_value;
            c_position += c_value;
            d_position += d_value;

            nacrtaj_oblik();
        }

        private void pomak(int value)
        {
            if (ponisti_event > 0) //sluzi kako oblici nebi samo padali, odnosno da kod stvaranja oblika u glavnom polju zaustavi ih na kratko
            {
                ponisti_event--;
            }
            else
            {
                //kockica se ispuni bijelom bojom
                a.isprazni(g);
                b.isprazni(g);
                c.isprazni(g);
                d.isprazni(g);

                //nacrta se rub kockice
                a.nacrtaj_rub(g);
                b.nacrtaj_rub(g);
                c.nacrtaj_rub(g);
                d.nacrtaj_rub(g);

                //svakoj kokckici se promjeni vrijednost position za vrijednost value
                a_position += value;
                b_position += value;
                c_position += value;
                d_position += value;

                //crtaju se nove kockice sa prethodno dobivenim vrijednostima
                nacrtaj_oblik();
            }

        }

        private void timer_game_Tick(object sender, EventArgs e)
        {
            //ova funkcija se izvršava svakih 1000 ms u 0. razini (kasnije se mijenja vrijeme izvršavanja), a ima iste karakteristike kao i kad se pritisne strelica za dolje
            if (provjera_kretanja(a_position + 12) || provjera_kretanja(b_position + 12) || provjera_kretanja(c_position + 12) || provjera_kretanja(d_position + 12))
            {
                a.ispunjen = true;
                b.ispunjen = true;
                c.ispunjen = true;
                d.ispunjen = true;

                ponisti_linije();
                postavi_oblik();
                uzmi_sljedeci_oblik();
                nacrtaj_sljedeci_oblik();
                nacrtaj_oblik();
            }
            else pomak(12);
        }

        private void uzmi_sljedeci_oblik()
        {
            //random odabir objekta iz liste oblika
            randBroj = randOblik.Next(lista_oblika.Count);
            sljedeci_oblik = lista_oblika[randBroj];
            if(sljedeci_oblik == "ravna")
            {
                a_Sposition = 29;
                b_Sposition = 30;
                c_Sposition = 31;
                d_Sposition = 32;
            }
            if(sljedeci_oblik == "kvadrat")
            {
                a_Sposition = 30;
                b_Sposition = 31;
                c_Sposition = 42;
                d_Sposition = 43;
            }
            if(sljedeci_oblik == "t_oblik")
            {
                a_Sposition = 30;
                b_Sposition = 31;
                c_Sposition = 32;
                d_Sposition = 43;
            }
            if(sljedeci_oblik == "z_oblik")
            {
                a_Sposition = 30;
                b_Sposition = 31;
                c_Sposition = 43;
                d_Sposition = 44;
            }
            if(sljedeci_oblik == "s_oblik")
            {
                a_Sposition = 41;
                b_Sposition = 42;
                c_Sposition = 30;
                d_Sposition = 31;
            }
            if(sljedeci_oblik == "j_oblik")
            {
                a_Sposition = 30;
                b_Sposition = 31;
                c_Sposition = 32;
                d_Sposition = 44;
            }
            if(sljedeci_oblik == "l_oblik")
            {
                a_Sposition = 32;
                b_Sposition = 31;
                c_Sposition = 30;
                d_Sposition = 42;
            }
        }

        private void postavi_oblik()
        {
            //promjena vrijednosti kod bodova, razine i vrijeme spuštanja oblika
            iznos_bodova(this);
            iznos_razine(this);
            iznos_vrijeme(this);

            oblik = sljedeci_oblik;
            stanje = 1; //pocetno stanje oblika sa pocetnim pozicijama kockica

            //predaja pozicija sljedeceg oblika u polje
            a_position = a_Sposition;
            b_position = b_Sposition;
            c_position = c_Sposition;
            d_position = d_Sposition;

            game_over(a_position, b_position, c_position, d_position); //provjera mogu li se objekti nalaziti u pocetnom polozaju, ako ne moze to znaci da je korisnik izgubio
        }

        private void game_over(int agm, int bgm, int cgm, int dgm)
        {
            if(provjera_kretanja(agm) || provjera_kretanja(bgm) || provjera_kretanja(cgm) || provjera_kretanja(dgm))
            {
                g.DrawString("Game over!", gm_font, gm_brush, gm_pointF); //ispisi na ekran game over
                btn_nova.Enabled = true;
                btn_top.Enabled = true;
                timer_game.Enabled = false; //iskljuci vrijeme
                gm = false; //onemoguci komande za kretanje
                dodaj_igraca(); //prikazi Game Over formu za upis igraca u listu
            }
        }

        private void nacrtaj_oblik()
        {
            //pronalazi po poziciji koji kvadratić treba ispuniti bojom
            a = kockica_lista.Where(x => x.ID == a_position).FirstOrDefault();
            b = kockica_lista.Where(x => x.ID == b_position).FirstOrDefault();
            c = kockica_lista.Where(x => x.ID == c_position).FirstOrDefault();
            d = kockica_lista.Where(x => x.ID == d_position).FirstOrDefault();
            //ispunjava bojom kvadratic
            a.ispuni(g);
            b.ispuni(g);
            c.ispuni(g);
            d.ispuni(g);
            //crni rubovi oko svakog iscrtanog kvadratica
            a.nacrtaj_rub(g);
            b.nacrtaj_rub(g);
            c.nacrtaj_rub(g);
            d.nacrtaj_rub(g);
        }

        private void nacrtaj_sljedeci_oblik()
        {
            f.Clear(Color.White); //potrebno je ocistiti prethodni iscrtani objekt
            //pronalazi po poziciji koji kvadratić treba ispuniti bojom
            a_s = kockica_lista_sljedeci.Where(x => x.ID == a_Sposition).FirstOrDefault();
            b_s = kockica_lista_sljedeci.Where(x => x.ID == b_Sposition).FirstOrDefault();
            c_s = kockica_lista_sljedeci.Where(x => x.ID == c_Sposition).FirstOrDefault();
            d_s = kockica_lista_sljedeci.Where(x => x.ID == d_Sposition).FirstOrDefault();
            //ispunjava bojom kvadratic
            a_s.ispuni(f);
            b_s.ispuni(f);
            c_s.ispuni(f);
            d_s.ispuni(f);
            //crni rubovi oko svakog iscrtanog kvadratica
            a_s.nacrtaj_rub(f);
            b_s.nacrtaj_rub(f);
            c_s.nacrtaj_rub(f);
            d_s.nacrtaj_rub(f);         
        }

        private void ponisti_linije()
        {
            /*
             Nakon što se objekt ne može više kretati, potrebno je dodati sve position-e u određenu listu kojoj pripada.

             Svaki redak se satoji od 12 kockica (2 rubna i 10 prostornih). Npr. 3. redak polja se sastoji od
             vrijednosti kockica (25,26,27,28,29,30,31,32,33,34,35 i 36). Kockica 25 i 36 su rubne. Ako se objekt više
             ne može kretati a jedna njegova kockica ostala u kockici npr. 30, ta kockica ce otici u listu ((30/12)-1).
             Odnosno u listu broj 1. jer su prva dva retka izvan Graphics-a (kao što je objašnjeno u funkciji inicijalizacija_polja())
             */

            a_lista = a_position / 12;
            a_lista--;
            b_lista = b_position / 12;
            b_lista--;
            c_lista = c_position / 12;
            c_lista--;
            d_lista = d_position / 12;
            d_lista--;

            dodajUliste(a_lista, a_position);
            dodajUliste(b_lista, b_position);
            dodajUliste(c_lista, c_position);
            dodajUliste(d_lista, d_position);

            //sljedeca funkcija provjerava postoji li ispunjeni red(odnosno neka ispunjena lista)
            provjera_ponisti_liste();        
        }

        private void dodajUliste(int value, int position)
        {
            if (value == 1) ponisti_1.Add(position);
            if (value == 2) ponisti_2.Add(position);
            if (value == 3) ponisti_3.Add(position);
            if (value == 4) ponisti_4.Add(position);
            if (value == 5) ponisti_5.Add(position);
            if (value == 6) ponisti_6.Add(position);
            if (value == 7) ponisti_7.Add(position);
            if (value == 8) ponisti_8.Add(position);
            if (value == 9) ponisti_9.Add(position);
            if (value == 10) ponisti_10.Add(position);
            if (value == 11) ponisti_11.Add(position);
            if (value == 12) ponisti_12.Add(position);
            if (value == 13) ponisti_13.Add(position);
            if (value == 14) ponisti_14.Add(position);
            if (value == 15) ponisti_15.Add(position);
            if (value == 16) ponisti_16.Add(position);
            if (value == 17) ponisti_17.Add(position);
            if (value == 18) ponisti_18.Add(position);
            if (value == 19) ponisti_19.Add(position);
            if (value == 20) ponisti_20.Add(position);
        }

        private void ponistiSVE_Iznadliste(int value)
        {           
            //ova funkcija se poziva kad se neka linija poništi
            dodaj_liniju(this); //funkcija koja mijenja vrijednost ponistenih linija

            if(value == 20)
            {
                ponisti_1.Clear();
                ponisti_2.Clear();
                ponisti_3.Clear();
                ponisti_4.Clear();
                ponisti_5.Clear();
                ponisti_6.Clear();
                ponisti_7.Clear();
                ponisti_8.Clear();
                ponisti_9.Clear();
                ponisti_10.Clear();
                ponisti_11.Clear();
                ponisti_12.Clear();
                ponisti_13.Clear();
                ponisti_14.Clear();
                ponisti_15.Clear();
                ponisti_16.Clear();
                ponisti_17.Clear();
                ponisti_18.Clear();
                ponisti_19.Clear();
                ponisti_20.Clear();
            }
            if (value == 19)
            {
                ponisti_1.Clear();
                ponisti_2.Clear();
                ponisti_3.Clear();
                ponisti_4.Clear();
                ponisti_5.Clear();
                ponisti_6.Clear();
                ponisti_7.Clear();
                ponisti_8.Clear();
                ponisti_9.Clear();
                ponisti_10.Clear();
                ponisti_11.Clear();
                ponisti_12.Clear();
                ponisti_13.Clear();
                ponisti_14.Clear();
                ponisti_15.Clear();
                ponisti_16.Clear();
                ponisti_17.Clear();
                ponisti_18.Clear();
                ponisti_19.Clear();
            }
            if (value == 18)
            {
                ponisti_1.Clear();
                ponisti_2.Clear();
                ponisti_3.Clear();
                ponisti_4.Clear();
                ponisti_5.Clear();
                ponisti_6.Clear();
                ponisti_7.Clear();
                ponisti_8.Clear();
                ponisti_9.Clear();
                ponisti_10.Clear();
                ponisti_11.Clear();
                ponisti_12.Clear();
                ponisti_13.Clear();
                ponisti_14.Clear();
                ponisti_15.Clear();
                ponisti_16.Clear();
                ponisti_17.Clear();
                ponisti_18.Clear();
            }
            if (value == 17)
            {
                ponisti_1.Clear();
                ponisti_2.Clear();
                ponisti_3.Clear();
                ponisti_4.Clear();
                ponisti_5.Clear();
                ponisti_6.Clear();
                ponisti_7.Clear();
                ponisti_8.Clear();
                ponisti_9.Clear();
                ponisti_10.Clear();
                ponisti_11.Clear();
                ponisti_12.Clear();
                ponisti_13.Clear();
                ponisti_14.Clear();
                ponisti_15.Clear();
                ponisti_16.Clear();
                ponisti_17.Clear();
            }
            if (value == 16)
            {
                ponisti_1.Clear();
                ponisti_2.Clear();
                ponisti_3.Clear();
                ponisti_4.Clear();
                ponisti_5.Clear();
                ponisti_6.Clear();
                ponisti_7.Clear();
                ponisti_8.Clear();
                ponisti_9.Clear();
                ponisti_10.Clear();
                ponisti_11.Clear();
                ponisti_12.Clear();
                ponisti_13.Clear();
                ponisti_14.Clear();
                ponisti_15.Clear();
                ponisti_16.Clear();
            }
            if (value == 15)
            {
                ponisti_1.Clear();
                ponisti_2.Clear();
                ponisti_3.Clear();
                ponisti_4.Clear();
                ponisti_5.Clear();
                ponisti_6.Clear();
                ponisti_7.Clear();
                ponisti_8.Clear();
                ponisti_9.Clear();
                ponisti_10.Clear();
                ponisti_11.Clear();
                ponisti_12.Clear();
                ponisti_13.Clear();
                ponisti_14.Clear();
                ponisti_15.Clear();
            }
            if (value == 14)
            {
                ponisti_1.Clear();
                ponisti_2.Clear();
                ponisti_3.Clear();
                ponisti_4.Clear();
                ponisti_5.Clear();
                ponisti_6.Clear();
                ponisti_7.Clear();
                ponisti_8.Clear();
                ponisti_9.Clear();
                ponisti_10.Clear();
                ponisti_11.Clear();
                ponisti_12.Clear();
                ponisti_13.Clear();
                ponisti_14.Clear();
            }
            if (value == 13)
            {
                ponisti_1.Clear();
                ponisti_2.Clear();
                ponisti_3.Clear();
                ponisti_4.Clear();
                ponisti_5.Clear();
                ponisti_6.Clear();
                ponisti_7.Clear();
                ponisti_8.Clear();
                ponisti_9.Clear();
                ponisti_10.Clear();
                ponisti_11.Clear();
                ponisti_12.Clear();
                ponisti_13.Clear();
            }
            if (value == 12)
            {
                ponisti_1.Clear();
                ponisti_2.Clear();
                ponisti_3.Clear();
                ponisti_4.Clear();
                ponisti_5.Clear();
                ponisti_6.Clear();
                ponisti_7.Clear();
                ponisti_8.Clear();
                ponisti_9.Clear();
                ponisti_10.Clear();
                ponisti_11.Clear();
                ponisti_12.Clear();
            }
            if (value == 11)
            {
                ponisti_1.Clear();
                ponisti_2.Clear();
                ponisti_3.Clear();
                ponisti_4.Clear();
                ponisti_5.Clear();
                ponisti_6.Clear();
                ponisti_7.Clear();
                ponisti_8.Clear();
                ponisti_9.Clear();
                ponisti_10.Clear();
                ponisti_11.Clear();
            }
            if (value == 10)
            {
                ponisti_1.Clear();
                ponisti_2.Clear();
                ponisti_3.Clear();
                ponisti_4.Clear();
                ponisti_5.Clear();
                ponisti_6.Clear();
                ponisti_7.Clear();
                ponisti_8.Clear();
                ponisti_9.Clear();
                ponisti_10.Clear();
            }
            if (value == 9)
            {
                ponisti_1.Clear();
                ponisti_2.Clear();
                ponisti_3.Clear();
                ponisti_4.Clear();
                ponisti_5.Clear();
                ponisti_6.Clear();
                ponisti_7.Clear();
                ponisti_8.Clear();
                ponisti_9.Clear();
            }
            if (value == 8)
            {
                ponisti_1.Clear();
                ponisti_2.Clear();
                ponisti_3.Clear();
                ponisti_4.Clear();
                ponisti_5.Clear();
                ponisti_6.Clear();
                ponisti_7.Clear();
                ponisti_8.Clear();
            }
            if (value == 7)
            {
                ponisti_1.Clear();
                ponisti_2.Clear();
                ponisti_3.Clear();
                ponisti_4.Clear();
                ponisti_5.Clear();
                ponisti_6.Clear();
                ponisti_7.Clear();
            }
            if (value == 6)
            {
                ponisti_1.Clear();
                ponisti_2.Clear();
                ponisti_3.Clear();
                ponisti_4.Clear();
                ponisti_5.Clear();
                ponisti_6.Clear();
            }
            if (value == 5)
            {
                ponisti_1.Clear();
                ponisti_2.Clear();
                ponisti_3.Clear();
                ponisti_4.Clear();
                ponisti_5.Clear();
            }
            if (value == 4)
            {
                ponisti_1.Clear();
                ponisti_2.Clear();
                ponisti_3.Clear();
                ponisti_4.Clear();
            }
            if (value == 3)
            {
                ponisti_1.Clear();
                ponisti_2.Clear();
                ponisti_3.Clear();
            }
            if (value == 2)
            {
                ponisti_1.Clear();
                ponisti_2.Clear();
            }
        }

        private void provjera_ponisti_liste()
        {
            //provjerava se od dolje prema gore
            #region 20
            if (ponisti_20.Count == 10) //ako je lista_20 puna lista
            {
                foreach (int a in ponisti_20)
                { //potrebno je obrisati svaki element na Graphics-u
                    obrisi_kockicu(a);
                }
                ponistiSVE_Iznadliste(20); //brišu se sve liste iznad liste_20 (ukljucujuci i nju)
                pomakni_kockice_prema_dolje(251); //pomicu se sve kockice prema dolje koje se nalaze do 20. retka
                provjera_ponisti_liste(); //ponovno se provjerava ako se tijekom pomicanja stvorio novi popunjeni redak
            }
            #endregion
            #region 19
            else if (ponisti_19.Count == 10)
            {
                foreach (int a in ponisti_19)
                {
                    obrisi_kockicu(a);
                }
                ponistiSVE_Iznadliste(19);
                pomakni_kockice_prema_dolje(239);
                provjera_ponisti_liste();
            }
            #endregion
            #region 18
            else if (ponisti_18.Count == 10)
            {
                foreach (int a in ponisti_18)
                {
                    obrisi_kockicu(a);
                }
                ponistiSVE_Iznadliste(18);
                pomakni_kockice_prema_dolje(227);
                provjera_ponisti_liste();
            }
            #endregion
            #region 17
            else if (ponisti_17.Count == 10)
            {
                foreach (int a in ponisti_17)
                {
                    obrisi_kockicu(a);
                }
                ponistiSVE_Iznadliste(17);
                pomakni_kockice_prema_dolje(215);
                provjera_ponisti_liste();
            }
            #endregion
            #region 16
            else if (ponisti_16.Count == 10)
            {
                foreach (int a in ponisti_16)
                {
                    obrisi_kockicu(a);
                }
                ponistiSVE_Iznadliste(16);
                pomakni_kockice_prema_dolje(203);
                provjera_ponisti_liste();
            }
            #endregion
            #region 15
            else if (ponisti_15.Count == 10)
            {
                foreach (int a in ponisti_15)
                {
                    obrisi_kockicu(a);
                }
                ponistiSVE_Iznadliste(15);
                pomakni_kockice_prema_dolje(191);
                provjera_ponisti_liste();
            }
            #endregion
            #region 14
            else if (ponisti_14.Count == 10)
            {
                foreach (int a in ponisti_14)
                {
                    obrisi_kockicu(a);
                }
                ponistiSVE_Iznadliste(14);
                pomakni_kockice_prema_dolje(179);
                provjera_ponisti_liste();
            }
            #endregion
            #region 13
            else if (ponisti_13.Count == 10)
            {
                foreach (int a in ponisti_13)
                {
                    obrisi_kockicu(a);
                }
                ponistiSVE_Iznadliste(13);
                pomakni_kockice_prema_dolje(167);
                provjera_ponisti_liste();
            }
            #endregion
            #region 12
            else if (ponisti_12.Count == 10)
            {
                foreach (int a in ponisti_12)
                {
                    obrisi_kockicu(a);
                }
                ponistiSVE_Iznadliste(12);
                pomakni_kockice_prema_dolje(155);
                provjera_ponisti_liste();
            }
            #endregion
            #region 11
            else if (ponisti_11.Count == 10)
            {
                foreach (int a in ponisti_11)
                {
                    obrisi_kockicu(a);
                }
                ponistiSVE_Iznadliste(11);
                pomakni_kockice_prema_dolje(143);
                provjera_ponisti_liste();
            }
            #endregion
            #region 10
            else if (ponisti_10.Count == 10)
            {
                foreach (int a in ponisti_10)
                {
                    obrisi_kockicu(a);
                }
                ponistiSVE_Iznadliste(10);
                pomakni_kockice_prema_dolje(131);
                provjera_ponisti_liste();
            }
            #endregion
            #region 9
            else if (ponisti_9.Count == 10)
            {
                foreach (int a in ponisti_9)
                {
                    obrisi_kockicu(a);
                }
                ponistiSVE_Iznadliste(9);
                pomakni_kockice_prema_dolje(119);
                provjera_ponisti_liste();
            }
            #endregion
            #region 8
            else if (ponisti_8.Count == 10)
            {
                foreach (int a in ponisti_8)
                {
                    obrisi_kockicu(a);
                }
                ponistiSVE_Iznadliste(8);
                pomakni_kockice_prema_dolje(107);
                provjera_ponisti_liste();
            }
            #endregion
            #region 7
            else if (ponisti_7.Count == 10)
            {
                foreach (int a in ponisti_7)
                {
                    obrisi_kockicu(a);
                }
                ponistiSVE_Iznadliste(7);
                pomakni_kockice_prema_dolje(95);
                provjera_ponisti_liste();
            }
            #endregion
            #region 6
            else if (ponisti_6.Count == 10)
            {
                foreach (int a in ponisti_6)
                {
                    obrisi_kockicu(a);
                }
                ponistiSVE_Iznadliste(6);
                pomakni_kockice_prema_dolje(83);
                provjera_ponisti_liste();
            }
            #endregion
            #region 5
            else if (ponisti_5.Count == 10)
            {
                foreach (int a in ponisti_5)
                {
                    obrisi_kockicu(a);
                }
                ponistiSVE_Iznadliste(5);
                pomakni_kockice_prema_dolje(71);
                provjera_ponisti_liste();
            }
            #endregion
            #region 4
            else if (ponisti_4.Count == 10)
            {
                foreach (int a in ponisti_4)
                {
                    obrisi_kockicu(a);
                }
                ponistiSVE_Iznadliste(4);
                pomakni_kockice_prema_dolje(59);
                provjera_ponisti_liste();
            }
            #endregion
            #region 3
            else if (ponisti_3.Count == 10)
            {
                foreach (int a in ponisti_3)
                {
                    obrisi_kockicu(a);
                }
                ponistiSVE_Iznadliste(3);
                pomakni_kockice_prema_dolje(47);
                provjera_ponisti_liste();
            }
            #endregion
            #region 2
            else if (ponisti_2.Count == 10)
            {
                foreach (int a in ponisti_2)
                {
                    obrisi_kockicu(a);
                }
                ponistiSVE_Iznadliste(2);
                pomakni_kockice_prema_dolje(35);
                provjera_ponisti_liste();
            }
            #endregion
        }

        private void obrisi_kockicu(int value)
        {
            ponisti = kockica_lista.Where(x => x.ID == value).FirstOrDefault();
            ponisti.isprazni(g);
            ponisti.nacrtaj_rub(g);
            ponisti.ispunjen = false;
        }

        private void pomakni_kockice_prema_dolje(int value)
        {
            for (int i = value; i > 25; i--)
            {
                ponisti = kockica_lista.Where(x => x.ID == i).FirstOrDefault();
                if (ponisti.ispunjen == true) //ako je kockica ispunjena, isprazni ju, pomakni ju prema dolje, te dodaj u odredjenu listu.
                {
                    ponisti.isprazni(g);
                    ponisti.nacrtaj_rub(g);
                    ponisti.ispunjen = false;
                    insert_ponisti = i + 12;
                    ponisti = kockica_lista.Where(x => x.ID == insert_ponisti).FirstOrDefault();
                    ponisti.ispuni(g);
                    ponisti.nacrtaj_rub(g);
                    ponisti.ispunjen = true;
                    insert_ponisti = insert_ponisti / 12;
                    insert_ponisti--;
                    dodajUliste(insert_ponisti, (i + 12));
                }
            }
        }
    }
}
