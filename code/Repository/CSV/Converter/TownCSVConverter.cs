﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Users;

namespace bolnica.Repository
{
    class TownCSVConverter : ICSVConverter<Town>
    {
        private readonly string _delimiter;
        private readonly string _arrayDelimiter;

        public TownCSVConverter(string delimiter, string arrayDelimiter)
        {
            _delimiter = delimiter;
            _arrayDelimiter = arrayDelimiter;
        }
        public Town ConvertCSVFormatToEntity(string entityCSVFormat)
        {
            string[] tokens = entityCSVFormat.Split(_delimiter.ToCharArray());
            Town town = new Town(long.Parse(tokens[0]), tokens[1], tokens[2]);
            // TODO Dodati ulice u listu 
            return town;
        }

        public string ConvertEntityToCSVFormat(Town entity)
        {
            StringBuilder sb = new StringBuilder();
            String formated = String.Join(_delimiter, entity.GetId(), entity.Name, entity.PostalNumber, entity.state.GetId());
            sb.Append(formated);
            sb.Append(_delimiter);
            foreach(Address address in entity.GetAddress())
            {
                sb.Append(address.GetId());
                sb.Append(_arrayDelimiter);
            }
            return sb.ToString();
        }
    }
}