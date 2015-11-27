using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nominas
{
    class Nomina{
		private int horas;

		public Nomina(){
			
			this.horas = null;
		}
	
		//Fragmento estándar
		public int Horas{
			
			set{
				this.horas = value;
			}

			get{
				return horas;
			}
		}
    }
}
