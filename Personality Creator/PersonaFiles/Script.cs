using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Personality_Creator.PersonaFiles
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

        public Script(FileInfo file) : base (file)
        { }

        public void Save(string content)
        {
            System.IO.File.WriteAllText(this.File.FullName, content);
        }

        public string Read()
        {
            return System.IO.File.ReadAllText(this.File.FullName);
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
