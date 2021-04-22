using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace test.Models {
	public class Reasons {

		[Key]
		public int intVisitReasonID {
			get;
			set;
		}

		public string strVisitReason {
			get;
			set;

		}


	}
}