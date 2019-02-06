using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Navegador
{
    public partial class Navegador : Form
    {
        public Navegador()
        {
            InitializeComponent();
            
            webBrowser.CanGoBackChanged +=
                new EventHandler(webBrowser_CanGoBackChanged);
            webBrowser.CanGoForwardChanged +=
                new EventHandler(webBrowser_CanGoForwardChanged);
            webBrowser.DocumentTitleChanged +=
                new EventHandler(webBrowser_DocumentTitleChanged);
            webBrowser.StatusTextChanged +=
                new EventHandler(webBrowser_StatusTextChanged);
            tsTbDirection.KeyPress += 
                new KeyPressEventHandler(tsTbDirection_KeyPress);

            home = ValidUrl("www.google.es");
            webBrowser.Navigate(home);
        }

        // Creacion de Variables
        private Uri home;
        private Uri search;
        private List<string> favoritos = new List<string>();
        private List<string> historial = new List<string>();

        // Cambiar Pestañas
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            webBrowser = (WebBrowser)tabControl.SelectedTab.Controls[0];
            this.Text = webBrowser.DocumentTitle;
            if(webBrowser.Url == null)
            {
                this.Text = "Nueva Pestaña";
                tsTbDirection.Text = "about:blank";
            }
            else
            {
                tsTbDirection.Text = webBrowser.Url.ToString();
            }
            
        }

        // Comprobar si URL es valida
        private static Uri ValidUrl(string testUrl)
        {
            try
            {
                if (testUrl.Equals("about:blank"))
                    return null;

                if (!testUrl.StartsWith("http://") && !testUrl.StartsWith("https://"))
                {
                    testUrl = "http://" + testUrl;
                }
                return new Uri(testUrl);
            }
            catch (UriFormatException)
            {
                return null;
            }
        }

        // Cambiar titulo del documento y titulo del Tab segun la pagina
        private void webBrowser_DocumentTitleChanged(object sender, EventArgs e)
        {
            try
            {
                this.Text = webBrowser.DocumentTitle;
                tabControl.SelectedTab.Text = webBrowser.DocumentTitle;
                tsTbDirection.Text = webBrowser.Url.ToString();

                // Guardar pagina en el historial (max. 10)
                if (historial.Count == 0)
                {
                    historial.Add(tsTbDirection.Text);
                    tsBtnHistory.DropDownItems.Add(tsTbDirection.Text);
                }

                if (historial.Count > 10)
                {
                    historial.RemoveAt(0);
                    tsBtnHistory.DropDownItems.RemoveAt(0);
                }

                if (!historial.Last().Equals(tsTbDirection.Text))
                {
                    historial.Add(tsTbDirection.Text);
                    tsBtnHistory.DropDownItems.Add(tsTbDirection.Text);
                }
            } catch (Exception exc) { }
            
        }

        // --ARCHIVO--

        // Guardar Como
        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser.ShowSaveAsDialog();
        }

        // Vista Previa
        private void vistaPreviaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser.ShowPrintPreviewDialog();
        }

        // Configurar Pagina
        private void configurarPáginaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser.ShowPageSetupDialog();
        }

        // Imprimir
        private void imprimirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser.ShowPrintDialog();
        }

        // Salir
        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // --CONFIGURACION--
        
        // Pagina de Inicio
        private void paginaDeInicioToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string texto = Microsoft.VisualBasic.Interaction.InputBox("Establece la Página de Inicio:", "Página de Inicio");
            if(ValidUrl(texto) != null)
            {
                home = ValidUrl(texto);
            }
            else
            {
                MessageBox.Show("Establezca una página de inicio válida", "Página de Inicio no válida", 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        // Busqueda
        private void búsquedaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string texto = Microsoft.VisualBasic.Interaction.InputBox("Establece la Página de Búsqueda:", "Página de Búsqueda");
            if (ValidUrl(texto) != null)
            {
                search = ValidUrl(texto);
            }
            else
            {
                MessageBox.Show("Establezca una página de inicio válida", "Página de Inicio no válida",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        // Propiedades
        private void propiedadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser.ShowPropertiesDialog();
        }

        // --TOOLSTRIP--

        // Direccion
        private void tsTbDirection_KeyPress(object sender, KeyPressEventArgs e)
        {
            if((int)e.KeyChar == (int)Keys.Enter)
            {
                e.Handled = true;
                if(ValidUrl(tsTbDirection.Text) != null)
                {
                    webBrowser.Navigate(ValidUrl(tsTbDirection.Text));
                }
            }
        }

        // Favoritos
        private void tsBtnFav_ButtonClick(object sender, EventArgs e)
        {
            favoritos.Add(tsTbDirection.Text);
            tsBtnFav.DropDownItems.Add(tsTbDirection.Text);
        }

        private void tsBtnFav_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            webBrowser.Navigate(e.ClickedItem.Text);
        }

        // --TOOLSTRIP 2 (BOTONES)--

        // Deshabilita el boton Atras en la primera pagina
        private void webBrowser_CanGoBackChanged(object sender, EventArgs e)
        {
            tsBtnBack.Enabled = webBrowser.CanGoBack;
        }

        // Atras
        private void tsBtnBack_Click(object sender, EventArgs e)
        {
            webBrowser.GoBack();
        }

        // Deshabilita el boton Siguiente en la ultima pagina
        private void webBrowser_CanGoForwardChanged(object sender, EventArgs e)
        {
            tsBtnForward.Enabled = webBrowser.CanGoForward;
        }

        // Siguiente
        private void tsBtnForward_Click(object sender, EventArgs e)
        {
            webBrowser.GoForward();
        }

        // Actualizar
        private void tsBtnRefresh_Click(object sender, EventArgs e)
        {
            webBrowser.Refresh();
        }

        // Inicio
        private void tsBtnHome_Click(object sender, EventArgs e)
        {
            if(home != null)
            {
                webBrowser.Navigate(home);
            }
            else
            {
                webBrowser.GoHome();
            }
        }

        // Busqueda
        private void tsBtnSearch_Click(object sender, EventArgs e)
        {
            if (search != null)
            {
                webBrowser.Navigate(search);
            }
            else
            {
                webBrowser.GoSearch();
            }
        }

        // Nueva Pestaña
        private void tsBtnAdd_Click(object sender, EventArgs e)
        {
            tabControl.TabPages.Add("Nueva Pestaña");
            webBrowser = new WebBrowser();
            tsTbDirection.Text = "about:blank";
            tabControl.TabPages[tabControl.TabPages.Count-1].Controls.Add(webBrowser);
            webBrowser.Dock = DockStyle.Fill;
            tabControl.SelectTab(tabControl.TabCount-1);
            webBrowser.CanGoBackChanged +=
                new EventHandler(webBrowser_CanGoBackChanged);
            webBrowser.CanGoForwardChanged +=
                new EventHandler(webBrowser_CanGoForwardChanged);
            webBrowser.DocumentTitleChanged +=
                new EventHandler(webBrowser_DocumentTitleChanged);
            webBrowser.StatusTextChanged +=
                new EventHandler(webBrowser_StatusTextChanged);
        }

        // Cerrar Pestaña
        private void tsBtnClosePage_Click(object sender, EventArgs e)
        {
            if(tabControl.TabPages.Count == 1)
            {
                Application.Exit();
            }
            else
            {
                tabControl.TabPages.Remove(tabControl.SelectedTab);
                tabControl.SelectTab(tabControl.TabPages.Count - 1);
            }
        }

        // Historial
        private void tsBtnHistory_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            webBrowser.Navigate(tsBtnHistory.DropDown.Text);
        }

        // Imprimir
        private void tsBtnPrint_Click(object sender, EventArgs e)
        {
            webBrowser.ShowPrintDialog();
        }

        // --STATUS STRIP--

        // Status Label
        private void webBrowser_StatusTextChanged(object sender, EventArgs e)
        {
            try
            {
                tsStatusLabel.Text = webBrowser.StatusText;
            }
            catch { }
        }
    }
}
