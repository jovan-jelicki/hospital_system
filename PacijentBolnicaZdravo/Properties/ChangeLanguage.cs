﻿using PacijentBolnicaZdravo.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PacijentBolnicaZdravo.Properties
{


    public  class ChangeLanguage
    {
        

        public  void changeMainWindow(MainWindow currentWindow)
        {

            Console.WriteLine("Ovdesam");
            Console.WriteLine(currentWindow);
                currentWindow.DarkModeLabel.Text = MyProject.Language.Resources.DarkMode;
                currentWindow.LogOutButton.Content = MyProject.Language.Resources.LogOut;
                currentWindow.Articles.Header = MyProject.Language.Resources.TabArticles;
                currentWindow.TabExamination.Header = MyProject.Language.Resources.TabExemination;
                currentWindow.ChooseDoctor.Text = MyProject.Language.Resources.ChooseDoctor;
                currentWindow.ChooseDate.Text = MyProject.Language.Resources.ChooseDate;
                currentWindow.Schedule.Content = MyProject.Language.Resources.Schedule;
                currentWindow.TabCancel.Header = MyProject.Language.Resources.TabCancel;
                currentWindow.CancelCondition.Text = MyProject.Language.Resources.CancelCondition;
                currentWindow.Doctor.Text = MyProject.Language.Resources.Doctor;
                currentWindow.Date.Text = MyProject.Language.Resources.Date;
                currentWindow.Time.Text = MyProject.Language.Resources.Time;
                currentWindow.Ordination.Text = MyProject.Language.Resources.Ordination;
                currentWindow.Cancel.Content = MyProject.Language.Resources.Cancel;
                currentWindow.TabFile.Header = MyProject.Language.Resources.TabFile;
                currentWindow.CurrentTherapyLabel.Text = MyProject.Language.Resources.CurrentTherapy;
                currentWindow.TabAccount.Header = MyProject.Language.Resources.Tabaccount;
                currentWindow.BasicInf.Text = MyProject.Language.Resources.BasicInf;
                currentWindow.NameLabel.Text = MyProject.Language.Resources.Name;
                currentWindow.SurnameLabel.Text = MyProject.Language.Resources.Surname;
                currentWindow.IDLabel.Text = MyProject.Language.Resources.ID;
                currentWindow.DateBirthLabel.Text = MyProject.Language.Resources.DateBirth;
                currentWindow.AdressLabel.Text = MyProject.Language.Resources.Adress;
                currentWindow.PhoneLabel.Text = MyProject.Language.Resources.NumberPhone;
                currentWindow.UpdateData.Content = MyProject.Language.Resources.UpdateData;
                currentWindow.ChoosePhotoButton.Content = MyProject.Language.Resources.Photo;
                currentWindow.PwLabel.Text = MyProject.Language.Resources.ChangePw;
                currentWindow.CurrentPw.Text = MyProject.Language.Resources.CurrentPw;
                currentWindow.NewPw.Text = MyProject.Language.Resources.NewPw;
                currentWindow.ConfirmPw.Text = MyProject.Language.Resources.ConfirmPw;
                currentWindow.UpdatePwButton.Content = MyProject.Language.Resources.UpdatePw;
                currentWindow.FeedbackHeader.Header = MyProject.Language.Resources.FeedBack;
                currentWindow.Opinion.Text = MyProject.Language.Resources.Opinion;
                currentWindow.OpinionName.Text = MyProject.Language.Resources.OpinionDesc;
                currentWindow.FeedBackButton.Content = MyProject.Language.Resources.Send;
                currentWindow.Title = MyProject.Language.Resources.Title;
        }   

        public void changeLogInWindow(WindowLogIn currentWindow)
        {
            currentWindow.DarkModeLabel.Text = MyProject.Language.Resources.DarkMode;
            currentWindow.UsernameLabel.Text = MyProject.Language.Resources.Username;
            currentWindow.PasswordLabel.Text = MyProject.Language.Resources.Password;
            currentWindow.LogIn.Content = MyProject.Language.Resources.LogIn;
            currentWindow.QuestionReg.Text = MyProject.Language.Resources.LabelRegistration;
            currentWindow.RegisterButton.Content = MyProject.Language.Resources.Register;

        }

        public void changeRegistrationWindow(Registration regWindow)
        {
            regWindow.BasicInf.Text = MyProject.Language.Resources.BasicInf;
            regWindow.NameLabel.Text = MyProject.Language.Resources.Name;
            regWindow.SurnameLabel.Text = MyProject.Language.Resources.Surname;
            regWindow.IDLabel.Text = MyProject.Language.Resources.ID;
            regWindow.DateBirthLabel.Text = MyProject.Language.Resources.DateBirth;
            regWindow.AdressLabel.Text = MyProject.Language.Resources.Adress;
            regWindow.PhoneLabel.Text = MyProject.Language.Resources.NumberPhone;
            regWindow.UpdateData.Content = MyProject.Language.Resources.UpdateData;
            regWindow.ChoosePhotoButton.Content = MyProject.Language.Resources.Photo;
            regWindow.NewPw.Text = MyProject.Language.Resources.NewPw;
            regWindow.ConfirmPw.Text = MyProject.Language.Resources.ConfirmPw;
            regWindow.CancelRegistration.Content = MyProject.Language.Resources.Cancel;

        }

    }
}



