using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace test.Models
{
	public class PetOwnerImage {
		//Owner Information
		public int intOwnerID { get; set; }
		//public string strOwnerName { get; set; }
		public string strFirstName { get; set; }
		public string strLastName { get; set; }

		//Pet Information
		public int intPetID { get; set; }
		public string strPetName { get; set; }

		//Pet Images Information
		public int intPetImageID { get; set; }

	}

}