using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace test.Models
{
    public class OwnerHome
    {
		public IEnumerable<PetOwnerImage> PetImageData { get; set; }
		//public DbSet<TOwner> TOwners { get; set; }
		//public DbSet<TPet> TPets { get; set; }
		//public DbSet<TPetImage> TPetImages { get; set; }

		//internal int petID;

		//Owner Information
		public int intOwnerID { get; set; }
		//public string strOwnerName { get; set; }
		public string strFirstName { get; set; }
		public string strLastName { get; set; }
		////public string strAddress { get; set; }
		////public string strCity { get; set; }
		////public int intStateID { get; set; }
		////public string strZip { get; set; }
		////public string strPhoneNumber { get; set; }

		////Pet Information
		//public int intPetID { get; set; }
		//public string strPetName { get; set; }
		////public string strPetNumber { get; set; }

		////Pet Images Information
		//public int intPetImageID { get; set; }
		//public string strFileName { get; set; }
		//public string strContentType { get; set; }
		//public byte[] imgContent { get; set; }
		//public string strFileType { get; set; }
		//public virtual IEnumerable<TPet> TPets { get; set; }
		////public IEnumerable<TPetImage> PetImages { get; set; }

		//public IEnumerable<OwnerHome> OwnerHomes { get; set; }

	}

	//public class OwnerHomeListDbContext : DbContext {
	//	public DbSet<OwnerHome> OwnerHomes { get; set; }
	//}
}