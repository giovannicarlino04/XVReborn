using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static XVReborn.Shared.Xenoverse;
using XVReborn.Properties;

namespace XVReborn
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void SetLanguage()
        {
            var languageMap = new Dictionary<CheckBox, string>
            {
                { checkBox1, Language.Catalan },
                { checkBox2, Language.German },
                { checkBox3, Language.English },
                { checkBox4, Language.Spanish },
                { checkBox5, Language.French },
                { checkBox6, Language.Italian },
                { checkBox7, Language.Polish },
                { checkBox8, Language.Portuguese },
                { checkBox9, Language.Russian }
            };

            foreach (var entry in languageMap)
            {
                if (entry.Key.Checked)
                {
                    Settings.Default.language = entry.Value;
                    Settings.Default.Save(); // Salva subito dopo aver impostato
                    this.Close();
                    return; // Uscire subito dalla funzione
                }
            }

            // Se nessuna lingua è selezionata, mostra un messaggio all'utente
            MessageBox.Show("Please select a language", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetLanguage();
        }
    }
}
