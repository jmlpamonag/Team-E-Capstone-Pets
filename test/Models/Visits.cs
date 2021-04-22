using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test.Models {
	public class Visits {

        // Visit 
        public DateTime dtmOfVisit { get; set; }
        public string strVisitReason { get; set; }
        public string strService { get; set; }
        public decimal dblPrice { get; set; }
        public string strMedication { get; set; }
        public string strMedDesc { get; set; }
        public int intQuantity { get; set; }
        public decimal dblMedPrice { get; set; }
        public string strServiceType { get; set; }


        // Pet 
        public string strPetName { get; set; }
        public string strPetNumber { get; set; }
        public string strBreed { get; set; }
        public string strAge { get; set; }
        public double dblWeight { get; set; }
        public string strGender { get; set; }


        // Owner 
        public string strOwnerName { get; set; }
        public string strAddress { get; set; }
        public string strCity { get; set; }
        public string strState { get; set; }
        public string strZip { get; set; }
        public string strPhone { get; set; }
        public string strEmail { get; set; }
        public int intOwnerNumber { get; set; }





        // Tables
        public IEnumerable<TVisit> Visit { get; set; }

        public IEnumerable<TVisitReason> VisitReason { get; set; }

        public IEnumerable<TPet> Pet { get; set; }

        public IEnumerable<TBreed> Breed { get; set; }

        public IEnumerable<TGender> Gender { get; set; }

        public IEnumerable<TOwner> Owner { get; set; }

        public IEnumerable<TVisitService> VisitService { get; set; }

        public IEnumerable<TVisitMedication> VisitMed { get; set; }

        public IEnumerable<TServiceType> ServType { get; set; }

    }

}
