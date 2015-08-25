using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Personality_Creator.PersonaFiles.Scripts
{
    public class Vocabfile : Script
    {
        private string keyword;

        public string Keyword
        {
            get
            {
                return keyword;
            }

            set
            {
                keyword = value;
            }
        }

        public Vocabfile(FileInfo file) : base(file)
        {
            this.Keyword = file.Name.Replace(file.Extension, "");
        }

        public void Save(string[] content)
        {
            System.IO.File.WriteAllLines(this.File.FullName, content);
        }

        public string[] Read()
        {
            return System.IO.File.ReadAllLines(this.File.FullName);
        }
    }
}
