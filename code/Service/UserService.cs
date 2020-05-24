/***********************************************************************
 * Module:  UserService.cs
 * Author:  Asus
 * Purpose: Definition of the Class Service.UserService
 ***********************************************************************/

using bolnica.Service;
using Model.PatientSecretary;
using Model.Users;
using Repository;
using System;
using System.Collections.Generic;

namespace Service
{
    public class UserService : IUserService
    {
        //private IService _doctorService;
        //private IService _patientService;
        //private IService _secretaryService;
        //private IService _directorService;
        
        //TODO : u klas dijagram dodati ove veze ako vam se ovo svidja
        private readonly IPatientRepository _patientRepo;
        private readonly IDoctorRepository _doctorRepo;
        private readonly ISecretaryRepository _secretaryRepo;
        private readonly IDirectorRepository _directorRepo;

        private readonly IPatientFileRepository _patientFileRepo;


        public UserService(IPatientRepository patientRepo, IPatientFileRepository fileRepo)
        {
            this._patientRepo = patientRepo;
            this._patientFileRepo = fileRepo;
        }

        public User Save(User entity)
        {
            try
            {
                Patient patient = (Patient)entity;
                if (_patientRepo.GetPatientByUsername(patient.Username).Equals(null)){
                    return null;
                }
                patient.patientFile = _patientFileRepo.Save(new PatientFile(-1));
                return _patientRepo.Save(patient);

            }
            catch
            {
                Doctor docktor = (Doctor)entity;
                // TODO : imam jos nekih pitanja oko doktora 
                return null;
            }
        }
        public bool BlockUser(string username)
        {
            throw new NotImplementedException();
        }

        public object Delete()
        {
            throw new NotImplementedException();
        }

        public object Edit()
        {
            throw new NotImplementedException();
        }

        public object GetAll()
        {
            throw new NotImplementedException();
        }

        public bool IsPasswordValid(User user, String password)
        {
            if (user.Password != password)
                return false;
            else
                return true;
        }

        // TODO: Zasto svaka rola ima svoju get<>ByID, Zasto to nije u Useru??
        public User IsUsernameValid(string username)
        {
            User user = null;
            if ((user = _patientRepo.GetPatientByUsername(username)) != null)
                return user;
            else if ((user = _secretaryRepo.GetSecretaryByUsername(username)) != null)
                return user;
            else if ((user = _directorRepo.GetDirectorByUsername(username)) != null)
                return user;
            else if ((user = _doctorRepo.GetDoctorByUsername(username)) != null)
                return user;
            return user;
        }

        public User Login(string username, string password)
        {
            User user = IsUsernameValid(username);
            if (user != null)
                return IsPasswordValid(user, password) ? user : null;
            else
                return null;
        }

        public bool Logout(User user)
        {
            throw new NotImplementedException();
        }


        public Feedback SendFeedback(string feedback)
        {
            throw new NotImplementedException();
        }

        public void Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public void Edit(User entity)
        {
            throw new NotImplementedException();
        }

        IEnumerable<User> IService<User, long>.GetAll()
        {
            throw new NotImplementedException();
        }

        public User Get(long id)
        {
            throw new NotImplementedException();
        }
    }
}