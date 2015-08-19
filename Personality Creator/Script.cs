using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Personality_Creator
{
    public class Script : PersonaFile
    {
        #region capsuled fields
        private ScriptDeclensionType scriptDeclension;
        #endregion

        #region properties
        public ScriptDeclensionType ScriptDeclension
        {
            get
            {
                return scriptDeclension;
            }

            set
            {
                scriptDeclension = value;
            }
        }
        #endregion

        public Script(FileInfo file) : base (file, PersonaFileType.Script)
        {
            this.File = file;
        }
    }

    public enum ScriptDeclensionType
    {
        Default,
        Chastity,
        Edging,
        Begging
    }
}
