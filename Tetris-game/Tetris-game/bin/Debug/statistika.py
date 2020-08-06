import clr
clr.AddReference('System.Windows.Forms') #koristenje reference iz c#
clr.AddReference('System.Drawing')
from System.Windows.Forms import *
from System.Drawing import Point, Font

brojac_linija=-1 #broji ako se vise linija odjednom ponistilo, sluzi za bodovanje
linija=-1 #broji koliko se sveukupno linija ponistilo
suma_bodova=-5
razina=0

form = Form(Text="Game over!", Height = 200, Width=200)
label = Label(Text='Upisi svoje ime:', Font=Font('Arial',12), Location = Point(30, 30), Height = 20, Width=120)
textbox = TextBox(Location = Point(30, 60),Height = 20, Width=120)
button = Button(Text="Potvrdi", Location = Point(30, 90), Height = 30, Width=120)

def linije(frm):
    global linija
    global brojac_linija

    linija=linija+1
    brojac_linija=brojac_linija+1
    frm.label_linija.Text = linija.ToString();

def razine(frm):
    global linija
    global razina  

    razina=linija/10
    frm.label_razina.Text = razina.ToString();

def bodovi_od_linija(frm):
    global brojac_linija
    global suma_bodova
    global razina

    if brojac_linija==0:
        suma_bodova=suma_bodova+5
        brojac_linija=0
    if brojac_linija==1:
        suma_bodova=suma_bodova+(40*(razina+1))
        brojac_linija=0
    if brojac_linija==2:
        suma_bodova=suma_bodova+(100*(razina+1))
        brojac_linija=0
    if brojac_linija==3:
        suma_bodova=suma_bodova+(300*(razina+1))
        brojac_linija=0
    if brojac_linija==4:
        suma_bodova=suma_bodova+(1200*(razina+1))
        brojac_linija=0
    frm.label_bodovi.Text = suma_bodova.ToString()


def postavi_vrijeme(frm):
    global razina
    if razina < 11:
        frm.timer_game.Interval = 1000 - (80 * razina)
    if razina>10 and razina < 14:
        frm.timer_game.Interval = 200
    if razina > 13 and razina < 16:
        frm.timer_game.Interval = 150
    if razina > 15 and razina < 19:
        frm.timer_game.Interval = 120
    if razina > 18 and razina < 29:
        frm.timer_game.Interval = 90
    if razina > 28:
        frm.timer_game.Interval = 60

def reset():
    global brojac_linija
    global linija
    global suma_bodova
    global razina

    brojac_linija=-1
    linija=-1
    suma_bodova=-5
    razina=0

def top_igrac(frm):
    FileName = ("top_player_lista.txt")
    data=file(FileName).readlines()
    data = sorted(data, key=lambda x: int(x.split(' ')[0]))

    for red_teksta in data:
        lista=red_teksta.split(' ')

    frm.label_top.Text= lista[1] + '\n' + lista[0]

def upisi_igraca(sender, args):
    with open('top_player_lista.txt', 'a') as f:
        f.write('\n'+ suma_bodova.ToString() + ' '+textbox.Text+ ' ')
    form.Close()

def spremi_igraca():
    button.Click += upisi_igraca
    form.Controls.Add(label)
    form.Controls.Add(textbox)
    form.Controls.Add(button)
    form.ShowDialog()

def top_lista():
    string=''
    FileName = ("top_player_lista.txt")
    data=file(FileName).readlines()
    data = sorted(data, key=lambda x: int(x.split(' ')[0]))
    for red_teksta in data:
        lista=red_teksta.split(' ')
        string=lista[1] + ' ' + lista[0] + '\n' + string  
    return string

