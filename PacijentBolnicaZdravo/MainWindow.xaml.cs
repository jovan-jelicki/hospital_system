﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.ComponentModel;
using MahApps.Metro.Controls;
using MahApps.Metro;
using Model.Dto;
using Model.Users;
using Model.PatientSecretary;
using Model.Director;
using System.Windows.Media.Animation;
using System.Windows.Forms;
using Model.Doctor;
using Application = System.Windows.Application;
using System.Linq;
using bolnica.Service;
using bolnica.Model.Dto;

namespace PacijentBolnicaZdravo
{

    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {

        public List<ExaminationDTO> scheduledExaminations { get; set; }
        public List<ExaminationDTO> upcomingExaminations { get; set; }
        public List<Doctor> listOfDoctors { get; set; }
        public List<Article> ListOfArticles { get; set; }

        public List<Doctor> doctorsForGrade { get; set; }
        public Patient _patient { get; set; }
        public List<State> States { get; set; }
        public List<Town> Towns { get; set; }
        public List<Address> Addresses { get; set; }
        public static int Theme = 0;

        public MainWindow(Patient patient)
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _patient = patient;

            scheduledExaminations = getScheduledExaminations();
            upcomingExaminations = new List<ExaminationDTO>();

            InitializeComponent();
            
            Country.DisplayMemberPath = "Name";
            Country.SelectedValuePath = "Id";
            App app = Application.Current as App;
            States = app.StateController.GetAll().ToList();
            States.Sort((x, y) => x.Name.CompareTo(y.Name));
            Country.ItemsSource = States;
            Town.DisplayMemberPath = "Name";
            Town.SelectedValuePath = "Id";
            Addressessss.DisplayMemberPath = "FullAddress";
            Addressessss.SelectedValuePath = "Id";
            Country.SelectedValue = _patient.Address.Town.State.GetId();
            Town.SelectedValue = _patient.Address.Town.GetId();
            Addressessss.SelectedValue = _patient.Address.GetId();



            PriorityBox.SelectedIndex = 0;
            Picker2.Visibility = Visibility.Hidden;
            if(_patient.Guest == true)
            {
                TabExamination.Visibility = Visibility.Hidden;
                TabFile.Visibility = Visibility.Hidden;
                FeedbackHeader.Visibility = Visibility.Hidden;
            }

            fillData();
        //    Proba();
            PasswordValidation2.password2 = _patient.Password;
           
            DoctorsForFeedback.DisplayMemberPath = "FullName";
            DoctorsForExaminations.DisplayMemberPath = "FullName";
           
            this.DataContext = this;
           
            if (Theme == 1)
            {
                ThemeManager.ChangeAppStyle(this,
                                   ThemeManager.GetAccent("Teal"),
                                   ThemeManager.GetAppTheme("BaseDark"));
                DarkMode.Value = DarkMode.Maximum;
            }else
            {
                ThemeManager.ChangeAppStyle(this,
                                   ThemeManager.GetAccent("Blue"),
                                   ThemeManager.GetAppTheme("BaseLight"));
                DarkMode.Value = DarkMode.Minimum;
            }

        }

   /*     private void Proba()
        {
            Doctor doctor = listOfDoctors[0];
            Period period = new Period(new DateTime(2020, 10, 04));

            BusinessDayDTO day = new BusinessDayDTO(doctor, period);
            var app = Application.Current as App;
            app.BusinessDayService.OperationSearch(day, 180);
        }*/
        private void UpdateTownAddress(object sender, RoutedEventArgs e)
        {
            State state = Country.SelectedItem as State;
            Towns = state.GetTown();
            Towns.Sort((x, y) => x.Name.CompareTo(y.Name));
            Town.ItemsSource = Towns;
            Addressessss.ItemsSource = null;


        }

        private void UpdateAddress(object sender, RoutedEventArgs e)
        {
            Town town = Town.SelectedItem as Town;
            if (town == null)
                return;
            Addresses = town.GetAddress();
            Addresses.Sort((x, y) => x.FullAddress.CompareTo(y.FullAddress));
            Addressessss.ItemsSource = Addresses;
        }

        private void fillData()
        {
            var app = Application.Current as App;
            listOfDoctors = app.DoctorController.GetDoctorsBySpeciality(new Speciality("Opsta praksa"));
            ListOfArticles = app.ArticleController.GetAll().ToList();
            doctorsForGrade = new List<Doctor>();
            upcomingExaminations = new List<ExaminationDTO>();
            List<Examination> examinations = _patient.patientFile.Examination;
            if(examinations != null)
            {
                foreach(Examination exam in examinations)
                {
                    if(!doctorsForGrade.Contains(exam.Doctor))
                          doctorsForGrade.Add(exam.Doctor);
                }
            }
            FillAccountData(_patient);
            setExaminations();
            setOperation();
            setHospitalizations();
            setArticle(ListOfArticles);
        }


        private void setExaminations()
        {
            var app = Application.Current as App;
            List<Examination> examinations = new List<Examination>();
            examinations = _patient.patientFile.Examination;
            if(examinations == null)
            {
                return;
            }
            foreach (var examination in examinations) 
            {
                Border b = new Border();
                b.BorderThickness = new Thickness(2);
                b.CornerRadius = new CornerRadius(3);
                b.BorderBrush = Brushes.LightBlue;
                b.Margin = new Thickness(10, 10, 10, 10);

                StackPanel stackPanelExamination = new StackPanel();
                TextBlock doctor = new TextBlock();
                TextBlock period = new TextBlock();
                TextBlock prescription = new TextBlock();
                TextBlock refferal = new TextBlock();
                TextBlock Anamnesis = new TextBlock();
                TextBlock Diagnosis = new TextBlock();
                TextBlock therapy = new TextBlock();

                doctor.FontSize = 15;
                doctor.Inlines.Add(new Run("Doktor:  ") { FontWeight = FontWeights.Bold });
                doctor.Inlines.Add( examination.Doctor.FullName);
                doctor.Margin = new Thickness(10, 10, 10, 10);
                stackPanelExamination.Children.Add(doctor);
                //
                period.Inlines.Add(new Run("Datum:  ") { FontWeight = FontWeights.Bold });
                period.FontSize = 15;
                period.Inlines.Add( examination.Period.StartDate.ToString());
                period.Margin = new Thickness(10, 10, 10, 10);
                stackPanelExamination.Children.Add(period);

                //
                Anamnesis.FontSize = 15;
                Anamnesis.Inlines.Add(new Run("Anamnesis:  ") { FontWeight = FontWeights.Bold });
                Anamnesis.TextWrapping = TextWrapping.Wrap;
                Anamnesis.Margin = new Thickness(10, 10, 10, 10);
                Anamnesis.Inlines.Add( examination.Anemnesis.Text);
                stackPanelExamination.Children.Add(Anamnesis);

                //
                Diagnosis.FontSize = 15;
                Diagnosis.TextWrapping = TextWrapping.Wrap;
                Diagnosis.Inlines.Add(new Run("Diagnoza:  ") { FontWeight = FontWeights.Bold });
                Diagnosis.Margin = new Thickness(10, 10, 10, 10);
                Diagnosis.Inlines.Add( examination.Diagnosis.Name);
                stackPanelExamination.Children.Add(Diagnosis);

                //

                prescription.FontSize = 15;
                prescription.TextWrapping = TextWrapping.Wrap;
                prescription.Margin = new Thickness(10, 10, 10, 10);
                prescription.Inlines.Add(new Run("Recept: ") { FontWeight = FontWeights.Bold });
                foreach(Prescription pr in examination.Prescription)
                {
                    foreach (Drug dr in pr.Drug)
                        prescription.Inlines.Add( dr.Name);
                }
                stackPanelExamination.Children.Add(prescription);

                therapy.FontSize = 15;
                therapy.TextWrapping = TextWrapping.Wrap;
                therapy.Margin = new Thickness(10, 10, 10, 10);
                therapy.Inlines.Add(new Run("Terapija:  ") { FontWeight = FontWeights.Bold });
                therapy.Inlines.Add(examination.Therapy.Note);
                stackPanelExamination.Children.Add(therapy);
                if (examination.Refferal != null)
                {
                    refferal.FontSize = 15;
                    refferal.Margin = new Thickness(10, 10, 10, 10);
                    refferal.Inlines.Add(new Run("Uput:  ") { FontWeight = FontWeights.Bold });
                    refferal.Inlines.Add("pacijent se upućuje na dateljniji pregled kod lekara " + examination.Refferal.Doctor.FullName + " datuma " + examination.Refferal.Period.StartDate.ToString());
                    stackPanelExamination.Children.Add(refferal);
                }

                    b.Child = stackPanelExamination;

                Exeminations.Children.Add(b);
            }
        }

        private void setHospitalizations()
        {
            var app = Application.Current as App;
            List<Hospitalization> hospitalizations = new List<Hospitalization>();
            hospitalizations = _patient.patientFile.Hospitalization;
            if (hospitalizations == null)
            {
                return;
            }
            foreach (var hospitalization in hospitalizations)
            {
                Border b = new Border();
                b.BorderThickness = new Thickness(2);
                b.CornerRadius = new CornerRadius(3);
                b.BorderBrush = Brushes.LightBlue;
                b.Margin = new Thickness(10, 10, 10, 10);

                StackPanel stackPanelExamination = new StackPanel();
                TextBlock period = new TextBlock();
                TextBlock room = new TextBlock();

                //
                period.Inlines.Add(new Run("Datum:  ") { FontWeight = FontWeights.Bold });
                period.FontSize = 15;
                period.Inlines.Add(hospitalization.Period.StartDate.ToString());
                period.Margin = new Thickness(10, 10, 10, 10);
                stackPanelExamination.Children.Add(period);

                //
                room.Inlines.Add(new Run("Prostorija: ") { FontWeight = FontWeights.Bold });
                room.FontSize = 15;
                room.Inlines.Add(hospitalization.Room.RoomCode);
                room.Margin = new Thickness(10);
                stackPanelExamination.Children.Add(room);

                b.Child = stackPanelExamination;

                Hospit.Children.Add(b);
            }
        }


        private void setOperation()
        {
            var app = Application.Current as App;
            List<Operation> operations = new List<Operation>();
            operations = _patient.patientFile.Operation;
            if (operations == null)
            {
                return;
            }
            foreach (var operation in operations)
            {
                Border b = new Border();
                b.BorderThickness = new Thickness(2);
                b.CornerRadius = new CornerRadius(3);
                b.BorderBrush = Brushes.LightBlue;
                b.Margin = new Thickness(10, 10, 10, 10);

                StackPanel stackPanelExamination = new StackPanel();
                TextBlock doctor = new TextBlock();
                TextBlock period = new TextBlock();
                TextBlock room = new TextBlock();
                TextBlock description = new TextBlock();

                doctor.FontSize = 15;
                doctor.Inlines.Add(new Run("Doktor:  ") { FontWeight = FontWeights.Bold });
                doctor.Inlines.Add(operation.Doctor.FullName);
                doctor.Margin = new Thickness(10, 10, 10, 10);
                stackPanelExamination.Children.Add(doctor);
                //
                period.Inlines.Add(new Run("Datum:  ") { FontWeight = FontWeights.Bold });
                period.FontSize = 15;
                period.Inlines.Add(operation.Period.StartDate.ToString());
                period.Margin = new Thickness(10, 10, 10, 10);
                stackPanelExamination.Children.Add(period);

                //
                room.Inlines.Add(new Run("Prostorija: ") { FontWeight = FontWeights.Bold });
                room.FontSize = 15;
                room.Inlines.Add(operation.Room.RoomCode);
                room.Margin = new Thickness(10);
                stackPanelExamination.Children.Add(room);

                //
                description.Inlines.Add(new Run("Opis: ") { FontWeight = FontWeights.Bold });
                description.FontSize = 15;
                description.Inlines.Add(operation.Description);
                description.Margin = new Thickness(10);
                stackPanelExamination.Children.Add(description);



                b.Child = stackPanelExamination;

                Operations.Children.Add(b);
            }
        }

        private void setArticle(List<Article> listOfArticles)
        {
            ArticlesPanel.Children.Clear();
            foreach (var article in listOfArticles)
            {
                Border b = new Border();
                b.BorderThickness = new Thickness(2);
                b.CornerRadius = new CornerRadius(3);
                b.BorderBrush = Brushes.LightBlue;
                b.Margin = new Thickness(10, 10, 10, 10);

                StackPanel stackPanelArticle = new StackPanel();
                TextBlock newTopic = new TextBlock();
                TextBlock newText = new TextBlock();
                TextBlock writer = new TextBlock();

                newTopic.TextWrapping = TextWrapping.Wrap;
                newTopic.FontSize = 15;
                newTopic.FontWeight = FontWeights.Bold;
                newTopic.MaxWidth = 700;
                newText.TextWrapping = TextWrapping.Wrap;
                newText.FontSize = 13;
                newText.MaxWidth = 700;
                writer.FontSize = 12;


                newTopic.Text = article.Topic;
                writer.Text = article.Doctor.FirstName + " " + article.Doctor.LastName;
                newText.Text = article.Text;

                stackPanelArticle.Children.Add(newTopic);
                stackPanelArticle.Children.Add(newText);
                stackPanelArticle.Children.Add(writer);

                b.Child = stackPanelArticle;

                ArticlesPanel.Children.Add(b);
            }
        }
        private void FillAccountData(Patient patient)
        {
            Username2.Text = _patient.Username;
            Name2.Text = _patient.FirstName;
            Surname2.Text = _patient.LastName;
            ID2.Text = _patient.Jmbg;
            Adress2.Text = _patient.Address.Street + " " + _patient.Address.Number + "," + " " + _patient.Address.Town.Name + " " + _patient.Address.Town.PostalNumber + "," + " " + _patient.Address.Town.State.Name;
            DateBirthTextBlock.Text = _patient.DateOfBirth.Date.ToString();
            Email2.Text = _patient.Email;
            PhoneNumber2.Text = _patient.Phone;
            

            Ime = _patient.FirstName;
            Prezime = _patient.LastName;
            JMBG = _patient.Jmbg;
            DATETIME = _patient.DateOfBirth;

            if (_patient.Guest)
            {
                UsernameConst.IsEnabled = true;
                USERNAME = "";
                EMAIL = "";
                Phone = "";
            }else
            {
                USERNAME = _patient.Username;
                UsernameConst.IsEnabled = false;
                EMAIL = _patient.Email;
                Phone = _patient.Phone;
            }

            
           
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private List<ExaminationDTO> getScheduledExaminations()
        {
            var app = Application.Current as App;
            List<Examination> upcomingExaminations = app.ExaminationController.GetUpcomingExaminationsByUser(this._patient);
            List<ExaminationDTO> retVal = new List<ExaminationDTO>();
            if(upcomingExaminations == null)
            {
                return retVal;
            }
            foreach(Examination exam in upcomingExaminations)
            {
                ExaminationDTO examinationDTO = new ExaminationDTO();
                examinationDTO.Id = exam.Id;
                examinationDTO.Doctor = exam.Doctor;
                examinationDTO.Period = exam.Period;
                BusinessDay day = app.BusinessDayController.GetExactDay(exam.Doctor, exam.Period.StartDate);
                examinationDTO.Room = day.room;
                retVal.Add(examinationDTO);
            }

            return retVal;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            DeleteExamination delete;
            DialogResult result;
          
            delete = new DeleteExamination("Are you sure you want to log out?", "Yes", "No", "Log out", MainWindow.Theme);
            result = delete.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                App.j = 0;
                WindowLogIn wl = new WindowLogIn();
                wl.Show();
                this.Close();
            }
        }
        

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if((int) DarkMode.Value == 1)
            {
                ThemeManager.ChangeAppStyle(this,
                                    ThemeManager.GetAccent("Teal"),//teal Steel
                                    ThemeManager.GetAppTheme("BaseDark"));
                Theme = 1;
            }
            else
            {
                ThemeManager.ChangeAppStyle(this,
                                    ThemeManager.GetAccent("Blue"),
                                    ThemeManager.GetAppTheme("BaseLight"));
                Theme = 0;
            }
        }

        private void Language_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
      
        }

     

        private void ChoosePhoto(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
              String fileName = op.FileName;
                ProfileImage.Source = new BitmapImage(new Uri(fileName));
                ProfileImage2.Source = new BitmapImage(new Uri(fileName));
                SuccessUpdatePhoto.Foreground = Brushes.Green;

                SuccessUpdatePhoto.Text = "You have successfully changed photo!";

                Storyboard sb = Resources["sbHideAnimation"] as Storyboard;
                sb.Begin(SuccessUpdatePhoto);
            }
        }

        private void UpdateInfo(object sender, RoutedEventArgs e)
        {

            var app = Application.Current as App;
            int prom = 0;
            var state = Country.SelectedItem as State;
            var town = Town.SelectedItem as Town;
            var selectedAddress = Addressessss.SelectedItem as Address;

            if (town == null || state == null)
            {
                return;
            }


            if (!Name.Text.ToString().Equals(""))
            {
                prom++;
                _patient.FirstName = Name.Text.ToString();
            }
            if (!Surname.Text.ToString().Equals(""))
            {
                prom++;
                _patient.LastName = Surname.Text.ToString();
            }
            if (!ID.Text.ToString().Equals(""))
            {
                prom++;
                _patient.Jmbg = ID.Text.ToString();
            }
           
            _patient.Address = selectedAddress;
            _patient.Address.Town = town;
            _patient.Address.Town.State = state;


            if (!PhoneNumber.Text.ToString().Equals(""))
            {
                prom++;
                _patient.Phone = PhoneNumber.Text.ToString();
            }
            if (!Email.Text.ToString().Equals(""))
            {
                prom++;
                _patient.Email = Email.Text.ToString();
                
            }
            if (!DateBirthPicker.Text.ToString().Equals("") && DateBirthPicker.SelectedDate != DateTime.Today)
            {
                prom++;
                _patient.DateOfBirth = DateTime.Parse(DateBirthPicker.Text);
            }
            Storyboard sb = Resources["sbHideAnimation"] as Storyboard;

            if (_patient.Guest)
            {
                if (UsernameConst.Text.ToString().Equals("") || _patient.Email.Equals("") || _patient.Phone.Equals(""))
                {
                    UsernameConst.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();
                    PhoneNumber.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();
                    Email.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();
                    SuccessUpdateData.Foreground = Brushes.Red;
                    SuccessUpdateData.Text = "All blank fields are required!";
                    sb.Begin(SuccessUpdateData);
                    return;
                }
                _patient.Username = UsernameConst.Text.ToString();
                if (app.UserController.IsUsernamedValid(_patient.Username) == null)
                {
                    _patient.Guest = false;
                    TabExamination.Visibility = Visibility.Visible;
                    TabFile.Visibility = Visibility.Visible;
                    FeedbackHeader.Visibility = Visibility.Visible;
                    SuccessUpdateData.Foreground = Brushes.Green;
                    SuccessUpdateData.Text = "You have successfully changed the data!";
                    sb.Begin(SuccessUpdateData);
                    app.UserController.Edit(_patient);
                    FillAccountData(_patient);
                    return;
                }else
                {
                    SuccessUpdateData.Foreground = Brushes.Red;
                    SuccessUpdateData.Text = "Username already exists!";
                    sb.Begin(SuccessUpdateData);
                    USERNAME = "";
                    UsernameConst.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();
                    return;
                }

            }

      
            if (prom != 0)
            {
                SuccessUpdateData.Foreground = Brushes.Green;


                SuccessUpdateData.Text = "You have successfully changed the data!";
                Console.WriteLine(_patient.patientFile.GetId());
             
                app.UserController.Edit(_patient);
                FillAccountData(_patient);
               
                sb.Begin(SuccessUpdateData);
                return;

            }
            else
            {
                SuccessUpdateData.Foreground = Brushes.Red;
                SuccessUpdateData.Text = "Data required for change!";
                sb.Begin(SuccessUpdateData);
            }
        }

        private void Password(object sender, RoutedEventArgs e)
        {
            PasswordValidation2.password2 = _patient.Password;
            CurrentPassword.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();
        }


        private void PasswordCheck(object sender, RoutedEventArgs e)
        {

            CheckPassword.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();

        }

        private void UpdatePw(object sender, RoutedEventArgs e)
        {
            if(CurrentPassword.Text != "" && NewPassword.Text != "")
            {
                _patient.Password = NewPassword.Text;
                var app = Application.Current as App;
                app.UserController.Edit(_patient);
                Storyboard sb = Resources["sbHideAnimation"] as Storyboard;
                sb.Begin(SuccessUpdatePw);
            }
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == "")
            {
                setArticle(this.ListOfArticles);
            }
            else
            {
                List<Article> searchList = new List<Article>();
                foreach (Article article in this.ListOfArticles)
                {
                    if (article.Topic.ToLower().Contains(SearchBox.Text.ToLower()))
                        searchList.Add(article);
                }
                if (searchList.Count == 0)
                {
                    setArticle(this.ListOfArticles);
                    return;
                }
                setArticle(searchList);
            }
            
        }

        private void CurrentTherapy(object sender, RoutedEventArgs e)
        {
            try
            {
                Process process = new System.Diagnostics.Process();
                String file;
                
                file = "C:\\Users\\jovan\\Desktop\\Faks\\HCI\\EngleskiIzvestaj2.pdf";
                
                process.StartInfo.FileName = file;
                process.Start();
                process.WaitForExit();
            }
            catch
            {
                System.Windows.MessageBox.Show("Could not open the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Feedback(object sender, RoutedEventArgs e)
        {
            
            if(FeedBack.Text.Length != 0)
            {

                FeedBack.Clear();

                FeedbackText.Text = "Thank you for sharing with us your opinion! ";

                Storyboard sb = Resources["sbHideAnimation"] as Storyboard;
                sb.Begin(FeedbackText);
            }

        }

        private void CalendarDateChanged(object sender, RoutedEventArgs e)
        {

        }


        private void GradeADoctorButton_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<String, double> gradesPerQuestion = new Dictionary<string, double>();
            gradesPerQuestion["0"] = Slider1.Value;
            gradesPerQuestion["1"] = Slider2.Value;
            gradesPerQuestion["2"] = Slider3.Value;
            gradesPerQuestion["3"] = Slider4.Value;
            gradesPerQuestion["4"] = Slider5.Value;

            var doctor = (Doctor)DoctorsForFeedback.SelectedItem;
            if (doctor == null)
            {
                FeedbackDOctor.Foreground = Brushes.Red;

                FeedbackDOctor.Text = "You have to pik doctor!";

                Storyboard ssb = Resources["sbHideAnimation"] as Storyboard;
                ssb.Begin(FeedbackDOctor);
                return;
            }

            var app = Application.Current as App;
            app.PatientController.GiveGradeToDoctor(doctor, gradesPerQuestion);
            FeedbackDOctor.Foreground = Brushes.Green;

            FeedbackDOctor.Text = "You have successfully graded the doctor!";



            Storyboard sb = Resources["sbHideAnimation"] as Storyboard;
            sb.Begin(FeedbackDOctor);
        }



        private void Zakazi(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            var selectedItem = scheduleExaminationsGrid.SelectedItem;

            if (selectedItem == null)
            {
                return;
            }
            if (scheduledExaminations.Count >= 3)
            {
                ErrorSchedule.Foreground = Brushes.Red;

                ErrorSchedule.Text = "You have a maximum number of appointments scheduled!";


                Storyboard sb = Resources["sbHideAnimation"] as Storyboard;
                sb.Begin(ErrorSchedule);
                return;
            }

            ErrorSchedule.Foreground = Brushes.Green;
            DeleteExamination delete;
            ExaminationDTO scheduleExam = (ExaminationDTO)selectedItem;

            delete = new DeleteExamination("Schedule examination at the doctor  " +
                                                                        scheduleExam.Doctor.FirstName + " " + scheduleExam.Doctor.LastName + "?", "Yes", "No", "Schedule examination", MainWindow.Theme);
            ErrorSchedule.Text = "You have successfully scheduled an examination!";

            DialogResult result = delete.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Doctor doctor = scheduleExam.Doctor;
                Period period = scheduleExam.Period;

                Examination examination = new Examination(this._patient, doctor, period);
                app.ExaminationController.Save(examination);
                BusinessDay day = app.BusinessDayController.GetExactDay(doctor, period.StartDate);
                app.BusinessDayController.MarkAsOccupied(period, day);

                scheduledExaminations = getScheduledExaminations();
                scheduledExaminationsGrid.ItemsSource = scheduledExaminations;
                upcomingExaminations = new List<ExaminationDTO>();
                scheduleExaminationsGrid.ItemsSource = upcomingExaminations;

                Storyboard sb = Resources["sbHideAnimation"] as Storyboard;
                sb.Begin(ErrorSchedule);
            }

        }

        private void Otkazi(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            var selectedItem = scheduledExaminationsGrid.SelectedItem;
            if (selectedItem == null)
            {
                return;
            }
            ErrorCancel.Foreground = Brushes.Green;
            DeleteExamination delete;
            ExaminationDTO deleteExam = (ExaminationDTO)selectedItem;
            delete = new DeleteExamination("Are you sure you want to cancel the examination at the doctor  " +
                                                                        deleteExam.Doctor.FirstName + " " + deleteExam.Doctor.LastName + "?", "Yes", "No", "Delete examination", MainWindow.Theme);
            ErrorCancel.Text = "You have successfully canceled the appointment!";

            DialogResult result = delete.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Examination toDelete = new Examination(deleteExam.Id);
                app.ExaminationController.Delete(toDelete);
                BusinessDay selectedDay = app.BusinessDayController.GetExactDay(deleteExam.Doctor, deleteExam.Period.StartDate);
                app.BusinessDayController.FreePeriod(selectedDay, deleteExam.Period.StartDate);

                scheduledExaminations = getScheduledExaminations();
                scheduledExaminationsGrid.ItemsSource = scheduledExaminations;

                Storyboard sb = Resources["sbHideAnimation"] as Storyboard;
                sb.Begin(ErrorCancel);
            }
        }


        private void SearchPeriods(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;

            if (DoctorsForExaminations.SelectedItem == null)
                return;
            if(PriorityBox.SelectedIndex == 0)
            {
                if (Picker.SelectedDate == null)
                    return;

                app.BusinessDayService._searchPeriods = new NoPrioritySearch();
                Doctor doctor = (Doctor)DoctorsForExaminations.SelectedItem;
                Period period = new Period();
                period.StartDate = DateTime.Parse(Picker.Text);
                BusinessDayDTO businessDayDTO = new BusinessDayDTO(doctor,period);
                businessDayDTO.PatientScheduling = true;
                upcomingExaminations = app.BusinessDayController.Search(businessDayDTO);
                scheduleExaminationsGrid.ItemsSource = upcomingExaminations;
            }else if(PriorityBox.SelectedIndex == 1)
            {
                if (Picker.SelectedDate == null || Picker2.SelectedDate == null)
                    return;

                Period period = new Period();
                period.StartDate = DateTime.Parse(Picker.Text);
                period.EndDate = DateTime.Parse(Picker2.Text);

                if (period.StartDate >= period.EndDate)
                    return;

                app.BusinessDayService._searchPeriods = new DoctorPrioritySearch();
                Doctor doctor = (Doctor)DoctorsForExaminations.SelectedItem;
                BusinessDayDTO businessDayDTO = new BusinessDayDTO(doctor, period);
                businessDayDTO.PatientScheduling = true;
                upcomingExaminations = app.BusinessDayController.Search(businessDayDTO);
                scheduleExaminationsGrid.ItemsSource = upcomingExaminations;
            }else
            {
                if (Picker.SelectedDate == null || Picker2.SelectedDate == null)
                    return;

                Period period = new Period();
                period.StartDate = DateTime.Parse(Picker.Text);
                period.EndDate = DateTime.Parse(Picker2.Text);
                if (period.StartDate >= period.EndDate)
                    return;

                app.BusinessDayService._searchPeriods = new DatePrioritySearch();
                Doctor doctor = (Doctor)DoctorsForExaminations.SelectedItem;
                BusinessDayDTO businessDayDTO = new BusinessDayDTO(doctor, period);
                businessDayDTO.PatientScheduling = true;
                upcomingExaminations = app.BusinessDayController.Search(businessDayDTO);
                scheduleExaminationsGrid.ItemsSource = upcomingExaminations;
            }
        }

        private void PriorityBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PriorityBox.SelectedIndex != 0)
            {
                Picker2.Visibility = Visibility.Visible;
            }
            
        }



        private String _ime;
        public String Ime
        {
            get
            {
                return _ime;
            }
            set
            {
                if (value != _ime)
                {
                    _ime = value;
                    OnPropertyChanged("Ime");
                }
            }
        }
        private String _password;
        public String Password1
        {
            get
            {
                return _password;
            }
            set
            {
                if (value != _password)
                {
                    _password = value;
                    OnPropertyChanged("Password");
                }
            }
        }
        private String _password2;
        public String Password2
        {
            get
            {
                return _password2;
            }
            set
            {
                if (value != _password2)
                {
                    _password2 = value;
                    OnPropertyChanged("Password");
                }
            }
        }
        private String _password3;
        public String Password3
        {
            get
            {
                return _password3;
            }
            set
            {
                if (value != _password3)
                {
                    _password3 = value;
                    OnPropertyChanged("Password");
                }
            }
        }



        private DateTime _dateTime = DateTime.Today;
        public DateTime DATETIME
        {
            get
            {
                return _dateTime;
            }
            set
            {
                if (value != _dateTime)
                {
                    _dateTime = value;
                    OnPropertyChanged("DATETIME");
                }
            }
        }


        private String _prezime;
        public String Prezime
        {
            get
            {
                return _prezime;
            }
            set
            {
                if (value != _prezime)
                {
                    _prezime = value;
                    OnPropertyChanged("Prezime");
                }
            }
        }
        private String _jmbg;
        public String JMBG
        {
            get
            {
                return _jmbg;
            }
            set
            {
                if (value != _jmbg)
                {
                    _jmbg = value;
                    OnPropertyChanged("JMBG");
                }
            }
        }

        private String _username;
        public String USERNAME
        {
            get
            {
                return _username;
            }
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged("USERNAME");
                }
            }
        }
        private String _email;
        public String EMAIL
        {
            get
            {
                return _email;
            }
            set
            {
                if (value != _email)
                {
                    _email = value;
                    OnPropertyChanged("EMAIL");
                }
            }
        }
        private String _adress;
        public String ADRESS
        {
            get
            {
                return _adress;
            }
            set
            {
                if (value != _adress)
                {
                    _adress = value;
                    OnPropertyChanged("ADRESS");
                }
            }
        }

        private String _phone;
        public String Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                if (value != _phone)
                {
                    _phone = value;
                    OnPropertyChanged("Phone");
                }
            }
        }


    }
}