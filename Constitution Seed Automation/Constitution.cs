using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Automation_Project.Constitution_Seed_Automation
{
    public class Constitution
    {
        public int Id { get; set; }
        public string TitleEnglish { get; set; }
        public string TitleNepali { get; set; }
        public string DescriptionEnglish { get; set; }
        public string DescriptionNepali { get; set; }
        public void Seed(string SqlPath)
        {
            string sqlContent = File.ReadAllText(SqlPath);

            string pattern = @"INSERT\s+\[dbo\]\.\[Constitutions\]\s+\(\[Id\],\s+\[TitleEnglish\],\s+\[TitleNepali\],\s+\[DescriptionEnglish\],\s+\[DescriptionNepali\]\)\s+VALUES\s+\((\d+),\s+N'([^']*)',\s+N'([^']*)',\s+N'([^']*)',\s+N'([^']*)'\)";

            var constitutions = new List<Constitution>();
            MatchCollection matches = Regex.Matches(sqlContent, pattern);

            foreach (Match match in matches)
            {
                int id = int.Parse(match.Groups[1].Value);
                string titleEnglish = match.Groups[2].Value;
                string titleNepali = match.Groups[3].Value;
                string descriptionEnglish = match.Groups[4].Value;
                string descriptionNepali = match.Groups[5].Value;

                var constitution = new Constitution()
                {
                    Id = id,
                    TitleEnglish = titleEnglish,
                    TitleNepali = titleNepali,
                    DescriptionEnglish = descriptionEnglish,
                    DescriptionNepali = descriptionNepali
                };

                constitutions.Add(constitution);
            }

            var outputBuilder = new StringBuilder();
            outputBuilder.AppendLine("var Constitutions = new List<Constitution>() {");

            foreach (var constitution in constitutions)
            {
                outputBuilder.AppendLine("    new Constitution()");
                outputBuilder.AppendLine("    {");
                outputBuilder.AppendLine($"        Id = {constitution.Id},");
                outputBuilder.AppendLine($"        TitleEnglish = \"{constitution.TitleEnglish.Replace("\"", "\\\"")}\",");
                outputBuilder.AppendLine($"        TitleNepali = \"{constitution.TitleNepali.Replace("\"", "\\\"")}\",");
                outputBuilder.AppendLine($"        DescriptionEnglish = @\"{constitution.DescriptionEnglish.Replace("\"", "\"\"")}\",");
                outputBuilder.AppendLine($"        DescriptionNepali = @\"{constitution.DescriptionNepali.Replace("\"", "\"\"")}\"");
                outputBuilder.AppendLine("    },");
            }

            outputBuilder.AppendLine("};");

            string outputFilePath = "C:\\Users\\someo\\OneDrive\\PREV FILES\\Documents\\Visual Studio 2022\\Automation\\Automation Project\\outputfile.txt";
            File.WriteAllText(outputFilePath, outputBuilder.ToString());

            Console.WriteLine("Output has been saved to " + outputFilePath);
        }
    }
}
