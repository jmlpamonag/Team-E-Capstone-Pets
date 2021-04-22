using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ComponentModel.DataAnnotations;


namespace test.Models {
	public class PetContent {
		public Pet pet;


		public bool CurrentUserIsOwner {
			get {
				//if (Event == null) return false;
				//if (Event.User == null) return false;
				//if (User == null) return false;
				//if (User.UID != Event.User.UID) return false;
				return true;
			}
		}
	}
}
