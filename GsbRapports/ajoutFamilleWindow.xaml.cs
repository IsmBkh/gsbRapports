using dllRapportVisites;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GsbRapports
{
    /// <summary>
    /// Logique d'interaction pour ajoutFamilleWindow.xaml
    /// </summary>
    public partial class ajoutFamilleWindow : Window
    {
        private string site;
        private WebClient wb;
        private Secretaire laSecretaire;

        public ajoutFamilleWindow(WebClient wb, Secretaire secretaire, string site)
        {
            InitializeComponent();
            this.wb = wb;
            this.site = site;
            this.laSecretaire = secretaire;
        }

        private void btnValider_Click(object sender, RoutedEventArgs e)
        {
            InitializeComponent();
            this.wb = wb;
            this.site = site;
            this.laSecretaire = laSecretaire;
            getFamilles();

        }
        private async void getFamilles()
        {
            try
            {
                string hash = this.laSecretaire.getHashTicketMdp();
                string url = this.site + "familles?ticket=" + hash;
                string reponse = await this.wb.DownloadStringTaskAsync(url);
                dynamic d = JsonConvert.DeserializeObject(reponse);
                this.laSecretaire.ticket = d.ticket;
                string lesFamilles = d.familles.ToString();
                List<Famille> familles = JsonConvert.DeserializeObject<List<Famille>>(lesFamilles);
                this.dtgFamilles.ItemsSource = familles;
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse)
                    MessageBox.Show(((HttpWebResponse)ex.Response).StatusCode.ToString());

            }
        }
    }
}
