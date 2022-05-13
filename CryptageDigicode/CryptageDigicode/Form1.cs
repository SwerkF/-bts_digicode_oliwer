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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string DateExp(string x) // Fonction pour voir si la date est arrivé à expiration
        {
            string z = " "; // Element à retrourner
            var file = new System.IO.StreamReader(File.OpenRead("digicod_secure.csv")); // On ouvre le fichier csv pour la lecture
            List<string> Mois = new List<string>(); // On créer une liste pour la colonne de la date expiration
            List<string> Auth = new List<string>(); // On créer une liste pour la colonne des autorisations

            while (!file.EndOfStream) // Pour les deux types de colonnes, on ajoute les valeurs dans les listes respectives
            {
                var line = file.ReadLine();
                var values = line.Split(';', '\n');
                Mois.Add(values[2]);
                Auth.Add(values[0]);
            }
            int nbrAut = 0; // On initialise le nombre de batiment à 0

            for (int k = 0; k < Auth.Count; k++)
            {
                if (Auth[k] == x.ToUpper())
                {
                    nbrAut = nbrAut + 1; // A chaque fois que l'autorisation du batiment est la lettre x , on rajoute 1
                }

            }
            int j = 0; // On initialise j, qui nous permettera de regarder la position de l'autorisation dans la liste
            DateTime ExpireDate = DateTime.Now; // On mets la date d'expiration à la date actuelle

            for (int i = 0; i < nbrAut; i++) // Pour I = 0, tant que i n'est pas égal au nombre de fois ou x apparait, on rajoute 1
            {
                while (Auth[j] != x.ToUpper())
                {
                    j = j + 1; // On récupère la premiere position de la lettre X
                }

                if (Auth[j] != x.ToUpper()) // Au cas ou, il y ait des mélanges sur la suite de la liste (E,E,I,E); on rajoute 1 a la position de x et on enlève
                                            // une fois i, car il y'a j fois la lettre, si on n'enlève pas une fois i, on ratera une lettre x
                {
                    j = j + 1;
                    i = i - 1;
                }
                else

                {

                    if (ExpireDate < DateTime.Parse(Mois[j]) && Auth[j] == x.ToUpper()) // Ici on compare chaque position de la lettre avec la date d'expiration
                    {

                        ExpireDate = DateTime.Parse(Mois[j]); // Si la date d'expiration précédente est plus ancienne que la suivante, on mets cette nouvelle date
                                                              // en ExpireDate
                    }
                }
                j = j + 1;
                if(ExpireDate.AddDays(-3) > DateTime.Now) // Si la date d'expiration est supérieur 
                {
                    z = "Le mot de passe " + x + " a déjà été généré ce mois ci. La génération d'un nouveau mot de passe n'est pas nécessaire.";
                    
                } else { 
                    z = "Expiration du mot de passe pour l'autorisation" + x + ". Il faut générer un nouveau mot de passe à chaque début de mois.";
                    
                }
            }
            file.Close(); // On ferme le fichier csv puisque on ne l'utilise plus
            return z;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            btn_connexion.Enabled = false;

        }

        private void tb_mdp_TextChanged(object sender, EventArgs e)
        {
            if (tb_mdp.Text != "") // On vérifie si le mot de passe a été rentré
            {
                btn_connexion.Enabled = true; // Si oui, on active le bouton
            }
            else
            {
                btn_connexion.Enabled = false; // Si non, on le désactive
            }
        }
        private void tb_matricule_TextChanged(object sender, EventArgs e)
        {
            if (tb_mdp.Text != "" && tb_matricule.Text != "") // On vérifie si le mot de passe a été rentré
            {
                btn_connexion.Enabled = true; // Si oui, on active le bouton
            }
            else
            {
                btn_connexion.Enabled = false; // Si non, on le désactive
            }
        }

        private void btn_connexion_Click(object sender, EventArgs e) // Après avoir cliqué sur le bouton
        {

            var file = new System.IO.StreamReader(File.OpenRead("digicod_perso.csv")); // On ouvre le fichier CSV avec le matricule
            var line = file.ReadToEnd();
            var values = line.Split(';','\n');
            for (int i = 0; i < values.Length; i++) // On parcours le fichier CSV
            {
                if(values[i] == "5874") // Si le matricule 5874 est trouvé:
                {
                    var file2 = new System.IO.StreamReader(File.OpenRead("digicod_secure.csv")); // On ouvre le fichier CSV contenant les mots de passes
                    var line2 = file2.ReadToEnd();
                    var values2 = line2.Split(';', '\r','\n');
                    for (int k = 0; k < values2.Length; k++) // On parcours le fichier CSV
                    {
                        if(values2[k] == "T") // Si l'autorisation T est trouvé :
                        {
                            if (values2[k+3] == tb_mdp.Text && tb_matricule.Text == "5874") //Si le mot de passe auquel est associé T est le même que celui qui est rentré
                                                                                                         // et que le matricule est 5874 :
                            {
                                MessageBox.Show("Identification reussite."); // On identifie la personne
                                MessageBox.Show(DateExp("E")); // On verifie grâce à la fonction si l'autorisation pour le batiment E nécessite une nouvelle génération
                                MessageBox.Show(DateExp("I")); // On verifie grâce à la fonction si l'autorisation pour le batiment I nécessite une nouvelle génération
                                file.Close(); // On ferme les deux fichier CSV
                                file2.Close();
                                var Form2 = new Form2(); // On décharge la Form1 et on charge la Form2
                                Form2.Show();
                                this.Hide();
                            } else
                            {
                                MessageBox.Show("L'identification a échoué. Vérifiez votre matricule ou mot de passe."); // Si elle échoue on renvoie une erreure
                            }
                        }
                    }

                }
            }

        }

        private void btn_quit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
