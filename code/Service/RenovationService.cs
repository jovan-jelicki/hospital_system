

using bolnica.Service;
using Model.Director;
using Repository;
using System;
using System.Collections.Generic;

namespace Service
{
   public class RenovationService : IRenovationService
   {
        private readonly IRenovationRepository _repository;

        public RenovationService(IRenovationRepository repository)
        {
            _repository = repository;
        }

        public void Delete(Renovation entity)
        {
            _repository.Delete(entity);
        }

        public void DeleteRenovationByRoom(Room room)
        {
            foreach (Renovation renovation in GetAll())
            {
                if (renovation.Room.Id == room.Id)
                    Delete(renovation);
            }
        }

        public void Edit(Renovation entity)
        {
            _repository.Edit(entity);
        }

        public Renovation Get(long id)
        {
            return _repository.GetEager(id);
        }

        public IEnumerable<Renovation> GetAll()
        {
            return _repository.GetAllEager();
        }

        public Renovation Save(Renovation entity)
        {
            if (validateDates(entity))
                return _repository.Save(entity);
            else
                return null;
        }

        private bool validateDates(Renovation entity)
        {
            if (DateTime.Compare(entity.Period.StartDate, entity.Period.EndDate) >= 0)
                return false;
            return true;
        }
    }
}