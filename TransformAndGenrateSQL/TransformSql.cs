using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Automation_Project.TransformAndGenrateSQL
{
    public class TransformSql
    {
        public static void TransformAndGenerateSQL(string sqlFilePath)
        {
            string sqlContent = File.ReadAllText(sqlFilePath);

            string[] sqlStatements = sqlContent.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            List<string> pharmacySQL = new();
            List<string> pharmacyDetailSQL = new();

            pharmacySQL.Add("SET IDENTITY_INSERT [dbo].[Pharmacy] ON ");

            // Dictionary to store PharmacyName and its corresponding PharmacyId
            Dictionary<string, int> pharmacyDict = new();

            foreach (string sqlStatement in sqlStatements)
            {
                if (sqlStatement.StartsWith("INSERT"))
                {
                    // Extract values from the INSERT statement
                    string values = sqlStatement.Substring(sqlStatement.IndexOf("VALUES") + 6).Trim();
                    values = values.Trim('(', ')');

                    // Split values by comma
                    string[] valuesArray = values.Split(',');

                    // Extract PharmacyName and PharmacyId from values
                    string pharmacyName = valuesArray[2].Trim().Trim('\'');
                    int pharmacyId;

                    // Check if PharmacyName already exists in the dictionary
                    if (!pharmacyDict.ContainsKey(pharmacyName))
                    {
                        // If not, insert it into the Pharmacy table
                        pharmacyId = pharmacyDict.Count + 1;
                        string pharmacyInsertSQL = $"INSERT INTO Pharmacy (PharmacyId, PharmacyName) VALUES ({pharmacyId}, '{pharmacyName}');";
                        pharmacySQL.Add(pharmacyInsertSQL);

                        // Add PharmacyName and PharmacyId to the dictionary
                        pharmacyDict.Add(pharmacyName, pharmacyId);
                    }
                    else
                    {
                        // If PharmacyName exists, retrieve its corresponding PharmacyId
                        pharmacyId = pharmacyDict[pharmacyName];
                    }

                    // Generate SQL for updating PharmacyDetails table
                    string pharmacyDetailUpdateSQL = $"UPDATE PharmacyDetails SET PharmacyId = {pharmacyId}, BillNumber = {valuesArray[1]}, BillNepaliDate = {valuesArray[4]}, BillEnglishDate = {valuesArray[5]}, BranchId = {valuesArray[6]}, PharmacyType = {valuesArray[7]} WHERE PharmacyDetailId = {valuesArray[0]};";
                    pharmacyDetailSQL.Add(pharmacyDetailUpdateSQL);
                }
            }

            pharmacySQL.Add("SET IDENTITY_INSERT [dbo].[Pharmacy] OFF ");

            File.WriteAllLines("C:\\Users\\someo\\OneDrive\\PREV FILES\\Documents\\Visual Studio 2022\\Automation\\Automation Project\\pharmacy_table.sql", pharmacySQL);
            File.WriteAllLines("C:\\Users\\someo\\OneDrive\\PREV FILES\\Documents\\Visual Studio 2022\\Automation\\Automation Project\\pharmacy_details_update.sql", pharmacyDetailSQL);

            Console.WriteLine("Transformation complete. Pharmacy table SQL file generated.");

        }
    }
}
