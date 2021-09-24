using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FBS.CompositionsPathGenerator
{
    public partial class FmMain : Form
    {
        public FmMain()
        {
            InitializeComponent();
        }

        private void btGenerate2016_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbBarcode.Text))
            {
                MessageBox.Show("Укажите Barcode");
                return;
            }

            int projecttBatchId = 0;
            if (!Int32.TryParse(tbProjectBatchId.Text, out projecttBatchId))
            {
                MessageBox.Show("ProjectBatchId должен быть целым числом");
                return;
            }

            if (String.IsNullOrEmpty(tbProjectName.Text))
            {
                MessageBox.Show("Укажите ProjectName");
                return;
            }

            int pagesCount = 0;
            if ((cbPagesCount.SelectedIndex == -1) || (!Int32.TryParse(cbPagesCount.SelectedItem.ToString(), out pagesCount)))
            {
                MessageBox.Show("Укажите число страниц");
                return;
            }

            string paths = FBS.Common.CompositionPathsHelper.GetCompositionPaths2016Plus(null, tbBarcode.Text, projecttBatchId, tbProjectName.Text, dtpExamDate.Value, pagesCount);

            tbResult.Text = paths.Replace(";", Environment.NewLine);

            MessageBox.Show("Относительные пути сформированы" + Environment.NewLine + "Не забудьте указать путь к сетевому ресурсу");
        }

        private void btGenerate2015_Click(object sender, EventArgs e)
        {
            Guid participantId;
            if (!Guid.TryParse(tbParticipantId.Text, out participantId))
            {
                MessageBox.Show("ParticipantId должен быть GUID");
                return;
            }

            if (String.IsNullOrEmpty(tbDocument.Text))
            {
                MessageBox.Show("Укажите документ");
                return;
            }

            if (String.IsNullOrEmpty(tbSurname.Text))
            {
                MessageBox.Show("Укажите фамилию");
                return;
            }

            if (String.IsNullOrEmpty(tbName.Text))
            {
                MessageBox.Show("Укажите имя");
                return;
            }

            if (String.IsNullOrEmpty(tbSecondName.Text))
            {
                MessageBox.Show("Укажите отчество");
                return;
            }

            int pagesCount = 0;
            if ((cbPagesCount.SelectedIndex == -1) || (!Int32.TryParse(cbPagesCount.SelectedItem.ToString(), out pagesCount)))
            {
                MessageBox.Show("Укажите число страниц");
                return;
            }

            string paths = FBS.Common.CompositionPathsHelper.GetCompositionPaths2015(null, participantId, tbDocument.Text, tbName.Text, tbSurname.Text, tbSecondName.Text, pagesCount);

            tbResult.Text = paths.Replace(";", Environment.NewLine);

            MessageBox.Show("Относительные пути сформированы" + Environment.NewLine + "Не забудьте указать путь к сетевому ресурсу");
        }
    }
}
