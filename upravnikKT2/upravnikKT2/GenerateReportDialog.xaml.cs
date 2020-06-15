﻿using bolnica.Controller;
using Controller;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace upravnikKT2
{
    /// <summary>
    /// Interaction logic for GenerateReportDialog.xaml
    /// </summary>
    public partial class GenerateReportDialog : Window
    {
        private readonly IRoomController roomController;
        public GenerateReportDialog()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.DataContext = this;

            var app = Application.Current as App;
            roomController = app.RoomController;
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Button_Click_OK(object sender, RoutedEventArgs e)
        {
            //Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            //dlg.FileName = "Izvestaj"; // Default file name
            //dlg.DefaultExt = ".txt"; // Default file extension
            //dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            //// Show save file dialog box
            //Nullable<bool> result = dlg.ShowDialog();

            //// Process save file dialog box results
            //if (result == true)
            //{
            //    // Save document
            //    string filename = dlg.FileName;
            //    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
            //    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream("Test.pdf", FileMode.Create));
            //    doc.Open();

            //    iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph("This is my first line using Paragraph");
            //    doc.Add(paragraph);
            //    doc.Close();
            //}

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream("Test1.pdf", FileMode.Create));
            doc.Open();

            iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph("This is my first line using Paragraph");
            doc.Add(paragraph);
            doc.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //ObservableCollection<RoomMockup> DataGridRooms = new ObservableCollection<RoomMockup>();
            //DataGridRooms.Add(new RoomMockup { Sifra = "1243"});
            //DataGridRooms.Add(new RoomMockup { Sifra = "6475"});
            //DataGridRooms.Add(new RoomMockup { Sifra = "9876"});
            //DataGridRooms.Add(new RoomMockup { Sifra = "8674"});
            //DataGridRooms.Add(new RoomMockup { Sifra = "5532"});
            //DataGridRooms.Add(new RoomMockup { Sifra = "7684" });
            //this.DataGridProstorije.ItemsSource = DataGridRooms;

            comboRooms.ItemsSource = roomController.GetAll().ToList();
            comboRooms.DisplayMemberPath = "RoomCode";
            comboRooms.SelectedValuePath = "Id";
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                this.Close();
            }

            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (OKBtn.IsEnabled)
                {
                    Button_Click_OK(sender, e);
                    e.Handled = true;
                }
            }
        }
    }
}
