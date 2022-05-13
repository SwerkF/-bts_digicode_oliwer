using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using CsvHelper;
using System.Globalization;

namespace CryptageDigicode
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        string mdpcode = ""; // Le mot de passe codé est rien pour le moment
        private void tb_mdp_TextChanged(object sender, EventArgs e)
        {
            string text = tb_mdp.Text; // On crée un string text qui est égale à notre textbox mdp pour voir si il y'a un chiffre.
            bool isNumber = false; // On admet que il n'y a pas de chiffre

            for (int i = 0; i < text.Length; i++) // On parcours la variable text
            {
                if (char.IsDigit(text[i])) // Si un charactères un un nombre
                {
                    isNumber = true; // On mets le booléan isNumber a true
                }
            }
            if(isNumber)
            {
                btn_valid.Enabled = false; // Si il y'a un nombre on désactive le bouton et on affiche un message
                MessageBox.Show("Il y'a un chiffre dans le mot de passe.");
                tb_mdp.Clear();

            } else
            {
                if (tb_mdp.Text == "" && tb_autor.Text == "") // Si les deux textebox n'ont aucun contenu, on désactive le boutton
                {
                    btn_valid.Enabled = false; 
                }
                else 
                {
                    if (tb_mdp.Text.Length == 6 && tb_autor.Text.ToUpper() == "E" || tb_mdp.Text.Length == 6 && tb_autor.Text.ToUpper() == "I" ) // Si la longueur du mot de passe est de 6 et l'autorisation du batiment est égale à I ou E
                    {
                        btn_valid.Enabled = true; // On active le boutton
                    }
                    else
                    {
                        btn_valid.Enabled = false; // Sinon on désactive le boutton
                    }
                }
            }
            
        }

        private void tb_autor_TextChanged(object sender, EventArgs e)
        {
            string text = tb_mdp.Text; // On crée un string text qui est égale à notre textbox mdp pour voir si il y'a un chiffre.
            bool isNumber = false; // On admet que il n'y a pas de chiffre

            for (int i = 0; i < text.Length; i++) // On parcours la variable text
            {
                if (char.IsDigit(text[i])) // Si un charactères un un nombre
                {
                    isNumber = true; // On mets le booléan isNumber a true
                }
            }
            if (isNumber)
            {
                btn_valid.Enabled = false; // Si il y'a un nombre on désactive le bouton et on affiche un message
                MessageBox.Show("Il y'a un chiffre dans le mot de passe.");
                tb_mdp.Clear();
            }
            else
            {
                if (tb_mdp.Text == "" && tb_autor.Text == "") // Si les deux textebox n'ont aucun contenu, on désactive le boutton
                {
                    btn_valid.Enabled = false;
                }
                else
                {
                    if (tb_mdp.Text.Length == 6 && tb_autor.Text.ToUpper() == "E" || tb_mdp.Text.Length == 6 && tb_autor.Text.ToUpper() == "I") // Si la longueur du mot de passe est de 6 et l'autorisation du batiment est égale à I ou E
                    {
                        btn_valid.Enabled = true; // On active le boutton
                    }
                    else
                    {
                        btn_valid.Enabled = false; // Sinon on désactive le boutton
                    }
                }
            }

        }

        private void btn_valid_Click(object sender, EventArgs e)
        {
                string alphabet = "abcdefghijklmnopqrstuvwxyz"; // On crée l'alphabet comme variable
                
            if(tb_autor.Text.ToUpper() == "E") // Si l'autorisation est E
            {
                for (int i = 0; i < tb_mdp.Text.Length; i++) // On chiffre le mot de passe avec la fonction donné
                {
                    string motACoder = tb_mdp.Text[i].ToString().ToLower(); // On converti la lettre i du mot de passe en minuscule
                    int pos = alphabet.IndexOf(motACoder); // On regarde la position de la lettre dans l'alphabet
                    int motcoder = pos + 10 % 26; // On effectue la fonction avec la position de la lettre
                    while (motcoder > 25) // Si le resultat est plus grand que 25
                    {
                        motcoder = motcoder - 26; // On retire 26
                    }
                    mdpcode = mdpcode + alphabet[motcoder]; // Ensuite, on regarde la position du resultat dans l'alphabet et on l'ajoute au motcoder

                }
            } else if(tb_autor.Text.ToUpper() == "I") // On refait la meme chose si la lettre est I
            {
                for (int i = 0; i < tb_mdp.Text.Length; i++)
                {
                    string motACoder = tb_mdp.Text[i].ToString().ToLower();
                    int pos = alphabet.IndexOf(motACoder);
                    int motcoder = 33 * pos + 1%26;
                    while (motcoder > 25)
                    {
                        motcoder = motcoder - 26;
                    }
                    while (motcoder < 0)
                    {
                        motcoder = motcoder + 25;
                    }
                    mdpcode = mdpcode + alphabet[motcoder];

                }
                

            }
            var Date1 = DateTime.Now; // On initilise la Date1 à la date actuelle
            var Start = Date1.Date.ToString("01-MM-yyyy"); // Le mot de passe sera valide à partir du 1er du mois actuel
            var Expire = Date1.ToString(DateTime.DaysInMonth(Date1.Year, Date1.Month) + "-MM-yyyy") ; // et il finira au denrier jour du mois

            var file = new System.IO.StreamReader(File.OpenRead("digicod_secure.csv")); // On ouvre le fichier CSV
            List<string> DateCSV = new List<string>(); // On créer une liste pour la colonne de la date expiration
            List<string> Auth = new List<string>();// On créer une liste pour la colonne des autorisations

            while (!file.EndOfStream) // Pour les deux types de colonnes, on ajoute les valeurs dans les listes respectives
            {
                    var line = file.ReadLine();
                    var values = line.Split(';', '\n');
                    DateCSV.Add(values[2]);
                    Auth.Add(values[0]);
            }

            int nbrAut = 0;  // On initialise le nombre de batiment à 0

            for (int k = 0; k < Auth.Count; k++)
            {
            if(Auth[k] == tb_autor.Text.ToUpper())
            {
                    nbrAut = nbrAut + 1; // A chaque fois que l'autorisation du batiment est la lettre x , on rajoute 1
                }

            }
            int j = 0; // On initialise j, qui nous permettera de regarder la position de l'autorisation dans la liste
            DateTime ExpireDate = DateTime.Now; // On mets la date d'expiration à la date actuelle
            for (int i = 0; i < nbrAut; i++) // Pour I = 0, tant que i n'est pas égal au nombre de fois ou x apparait, on rajoute 1
            {
                    while (Auth[j] != tb_autor.Text.ToUpper())
                {
                    j = j + 1; // On récupère la premiere position de la lettre X
                }
                if(Auth[j] != tb_autor.Text.ToUpper()) // Au cas ou, il y ait des mélanges sur la suite de la liste (E,E,I,E); on rajoute 1 a la position de x et on enlève
                                                     // une fois i, car il y'a j fois la lettre, si on n'enlève pas une fois i, on ratera une lettre x
                {
                    j = j + 1;
                    i = i - 1;
                } else
                
                {
                    int MoisPlus = Date1.Month + 1;
                    
                    if (ExpireDate < DateTime.Parse(DateCSV[j]) && Auth[j] == tb_autor.Text.ToUpper())// Ici on compare chaque position de la lettre avec la date d'expiration
                    {

                        ExpireDate = DateTime.Parse(DateCSV[j]); // Si la date d'expiration précédente est plus ancienne que la suivante, on mets cette nouvelle date
                                                                 // en ExpireDate
                    }
                }
                j = j + 1; 
                }
            if(ExpireDate <= DateTime.Now) // Si la date d'expiration est plus ancienne ou égale à la date actuelle
            {
                file.Close(); // On ferme la lecture du fichier
                using (System.IO.StreamWriter file2 = new System.IO.StreamWriter("digicod_secure.csv", true)) // On lit le fichier CSV
                {
                    MessageBox.Show("Nouveau mot de passe généré pour le bâtiment " + tb_autor.Text.ToUpper()); // On confirme la création du nouveau mot de passe
                    file2.WriteLine(tb_autor.Text.ToUpper() + ";" + Start + ";" + Expire + ";" + mdpcode.ToUpper()); // On écrire le nouveau mot de passe à la suite dans le fichier
                    mdpcode = ""; // On réinitialise le mot de passe à rien
                    return;
                }
            } else if(ExpireDate >= DateTime.Now)
            {
                MessageBox.Show("Un mot de passe a déjà été généré ce mois ci. Il sera possible d'en générer un nouveau le mois suivant."); // On envoie un message d'erreur
                mdpcode = ""; // On réinitialise le mot de passe à rien
                return;
            }

            

        }

        private void btn_disconnect_Click(object sender, EventArgs e)
        {
            var Form1 = new Form1(); // On charge la Form1 et on décharge la Form2
            Form1.Show();
            this.Close();
        }
    }
}

