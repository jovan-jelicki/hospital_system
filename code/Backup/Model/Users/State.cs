/***********************************************************************
 * Module:  State.cs
 * Author:  Asus
 * Purpose: Definition of the Class Users.State
 ***********************************************************************/

using System;

namespace Model.Users
{
   public class State
   {
      public Town[] town;
   
      private String Name;
      private String Code;
      
      private System.Collections.ArrayList town;
      
      /// <pdGenerated>default getter</pdGenerated>
      public System.Collections.ArrayList GetTown()
      {
         if (town == null)
            town = new System.Collections.ArrayList();
         return town;
      }
      
      /// <pdGenerated>default setter</pdGenerated>
      public void SetTown(System.Collections.ArrayList newTown)
      {
         RemoveAllTown();
         foreach (Town oTown in newTown)
            AddTown(oTown);
      }
      
      /// <pdGenerated>default Add</pdGenerated>
      public void AddTown(Town newTown)
      {
         if (newTown == null)
            return;
         if (this.town == null)
            this.town = new System.Collections.ArrayList();
         if (!this.town.Contains(newTown))
         {
            this.town.Add(newTown);
            newTown.SetState(this);      
         }
      }
      
      /// <pdGenerated>default Remove</pdGenerated>
      public void RemoveTown(Town oldTown)
      {
         if (oldTown == null)
            return;
         if (this.town != null)
            if (this.town.Contains(oldTown))
            {
               this.town.Remove(oldTown);
               oldTown.SetState((State)null);
            }
      }
      
      /// <pdGenerated>default removeAll</pdGenerated>
      public void RemoveAllTown()
      {
         if (town != null)
         {
            System.Collections.ArrayList tmpTown = new System.Collections.ArrayList();
            foreach (Town oldTown in town)
               tmpTown.Add(oldTown);
            town.Clear();
            foreach (Town oldTown in tmpTown)
               oldTown.SetState((State)null);
            tmpTown.Clear();
         }
      }
   
   }
}