/***********************************************************************
 * Module:  ISecretaryRepository.cs
 * Author:  david
 * Purpose: Definition of the Interface Repository.ISecretaryRepository
 ***********************************************************************/

using System;

namespace Repository
{
   public interface ISecretaryRepository : IRepository
   {
      Secretary GetSecretaryByUsername(String username);
   }
}