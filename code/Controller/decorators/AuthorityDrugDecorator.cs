﻿using Model.PatientSecretary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bolnica.Controller.decorators
{
    public class AuthorityDrugDecorator : IDrugController
    {
        private IDrugController DrugController;
        private String Role;
        private Dictionary<String, List<String>> AuthorizedUsers;

        public AuthorityDrugDecorator(IDrugController drugController, String role)
        {
            this.DrugController = drugController;
            this.Role = role;
            AuthorizedUsers = new Dictionary<string, List<string>>();
            AuthorizedUsers["AddAlternativeDrug"] = new List<string>() { "Doctor" };
            AuthorizedUsers["ApproveDrug"] = new List<string>() { "Doctor" };
            AuthorizedUsers["CheckDrugNameUnique"] = new List<string>() { "Director" };
            AuthorizedUsers["Delete"] = new List<string>() { "Director" };
            AuthorizedUsers["Edit"] = new List<string>() { "Director", "Doctor" };
            AuthorizedUsers["Get"] = new List<string>() { "Director", "Doctor" };
            AuthorizedUsers["GetAll"] = new List<string>() { "Director", "Doctor" };
            AuthorizedUsers["GetAlternativeDrugs"] = new List<string>() { "Director" };
            AuthorizedUsers["GetNotApprovedDrugs"] = new List<string>() { "Doctor" };
            AuthorizedUsers["RecommendDrugBasedOnDiagnosis"] = new List<string>() { "Doctor" };
            AuthorizedUsers["Save"] = new List<string>() { "Director" };
        }

        public Drug AddAlternativeDrug(Drug originalDrug, Drug alternativeDrug)
        {
            if (AuthorizedUsers["AddAlternativeDrug"].SingleOrDefault(any => any.Equals(Role)) != null)
                return DrugController.AddAlternativeDrug(originalDrug, alternativeDrug);
            return null;
        }

        public Drug ApproveDrug(Drug drug)
        {
            if (AuthorizedUsers["ApproveDrug"].SingleOrDefault(any => any.Equals(Role)) != null)
                return DrugController.ApproveDrug(drug);
            return null;
        }

        public bool CheckDrugNameUnique(string name)
        {
            if (AuthorizedUsers["CheckDrugNameUnique"].SingleOrDefault(any => any.Equals(Role)) != null)
                return DrugController.CheckDrugNameUnique(name);
            return false;
        }

        public void Delete(Drug entity)
        {
            if (AuthorizedUsers["Delete"].SingleOrDefault(any => any.Equals(Role)) != null)
                DrugController.Delete(entity);
        }

        public void Edit(Drug entity)
        {
            if (AuthorizedUsers["Edit"].SingleOrDefault(any => any.Equals(Role)) != null)
                DrugController.Edit(entity);
        }

        public Drug Get(long id)
        {
            if (AuthorizedUsers["Get"].SingleOrDefault(any => any.Equals(Role)) != null)
                return DrugController.Get(id);
            return null;
        }

        public IEnumerable<Drug> GetAll()
        {
            if (AuthorizedUsers["GetAll"].SingleOrDefault(any => any.Equals(Role)) != null)
                return DrugController.GetAll();
            return null;
        }

        public List<Drug> GetAlternativeDrugs(Drug drug)
        {
            if (AuthorizedUsers["GetAlternativeDrugs"].SingleOrDefault(any => any.Equals(Role)) != null)
                return DrugController.GetAlternativeDrugs(drug);
            return null;
        }

        public List<Drug> GetNotApprovedDrugs()
        {
            if (AuthorizedUsers["GetNotApprovedDrugs"].SingleOrDefault(any => any.Equals(Role)) != null)
                return DrugController.GetNotApprovedDrugs();
            return null;
        }

        public Drug RecommendDrugBasedOnDiagnosis(Diagnosis diagnosis)
        {
            if (AuthorizedUsers["RecommendDrugBasedOnDiagnosis"].SingleOrDefault(any => any.Equals(Role)) != null)
                return DrugController.RecommendDrugBasedOnDiagnosis(diagnosis);
            return null;
        }

        public Drug Save(Drug entity)
        {
            if (AuthorizedUsers["Save"].SingleOrDefault(any => any.Equals(Role)) != null)
                return DrugController.Save(entity);
            return null;
        }
    }
}
