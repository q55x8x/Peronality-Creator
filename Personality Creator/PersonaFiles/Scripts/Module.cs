using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Text.RegularExpressions.Regex;
using System.IO;

namespace Personality_Creator.PersonaFiles.Scripts
{
    public class Module : Script
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

        public string ModuleName
        {
            get
            {
                if (this.ScriptDeclension == ScriptDeclensionType.Edging)
                {
                    return this.File.Name.Replace(this.File.Extension, "").Replace($"_{nameof(ScriptDeclensionType.Edging).ToUpper()}", "");
                }
                else if (this.ScriptDeclension == ScriptDeclensionType.Chastity)
                {
                    return this.File.Name.Replace(this.File.Extension, "").Replace($"_{nameof(ScriptDeclensionType.Chastity).ToUpper()}", "");
                }
                else if (this.ScriptDeclension == ScriptDeclensionType.Begging)
                {
                    return this.File.Name.Replace(this.File.Extension, "").Replace($"_{nameof(ScriptDeclensionType.Begging).ToUpper()}", "");
                }
                else
                {
                    return this.File.Name.Replace(this.File.Extension, "");
                }
            }
        }

        #endregion

        public Module(FileInfo file) : base (file)
        {
            string declensionString = Match(this.File.Name, @"(?i)(?<=._)[\w]+(?=\.)").Value;

            if(declensionString == nameof(ScriptDeclensionType.Edging).ToUpper()) //sadly switch case only works with constants
            {
                this.ScriptDeclension = ScriptDeclensionType.Edging;
            }
            else if(declensionString == nameof(ScriptDeclensionType.Chastity).ToUpper())
            {
                this.ScriptDeclension = ScriptDeclensionType.Chastity;
            }
            else if(declensionString == nameof(ScriptDeclensionType.Begging).ToUpper())
            {
                this.ScriptDeclension = ScriptDeclensionType.Begging;
            }
            else
            {
                this.ScriptDeclension = ScriptDeclensionType.Default;
            }
        }

        public Module(string file) : this(new FileInfo(file))
        {
        }
        public void Save(string content)
        {
            System.IO.File.WriteAllText(this.File.FullName, content);
        }

        public string Read()
        {
            return System.IO.File.ReadAllText(this.File.FullName);
        }

        public Module clone(ScriptDeclensionType declension)
        {
            string newFullName;

            switch(declension)
            {
                case ScriptDeclensionType.Edging:
                    newFullName = $@"{this.File.DirectoryName}\{this.ModuleName}_{nameof(ScriptDeclensionType.Edging).ToUpper()}{this.File.Extension}";
                    break;
                case ScriptDeclensionType.Chastity:
                    newFullName = $@"{this.File.DirectoryName}\{this.ModuleName}_{nameof(ScriptDeclensionType.Chastity).ToUpper()}{this.File.Extension}";
                    break;
                case ScriptDeclensionType.Begging:
                    newFullName = $@"{ this.File.DirectoryName}\{this.ModuleName}_{nameof(ScriptDeclensionType.Begging).ToUpper()}{this.File.Extension}";
                    break;
                default:
                    throw new InvalidOperationException("None or no valid declension type specified to clone into!");
            }
            
            this.File.CopyTo(newFullName);

            return new Module(newFullName);
        }
    }
}
