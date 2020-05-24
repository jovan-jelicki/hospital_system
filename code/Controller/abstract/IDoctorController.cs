﻿using Controller;
using Model.Doctor;
using Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bolnica.Controller
{
   public interface IDoctorController : IController<Doctor, long>
{
         List<Doctor> GetDoctorsBySpeciality(Specialty specialty);
         Boolean ChangeSpeciality(Specialty speciality, Doctor doctor);

        DoctorGrade GiveGrade(DoctorGrade doctorGrade);
    }
}
