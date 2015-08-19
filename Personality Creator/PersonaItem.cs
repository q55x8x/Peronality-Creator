using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personality_Creator
{
    public abstract class PersonaItem
    {
        #region capsuled fields
        private Folder parent;
        #endregion

        #region properties
        public Folder Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
            }
        }
        #endregion
    }
}
