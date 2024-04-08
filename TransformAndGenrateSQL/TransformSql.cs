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

            // Dictionary to store Supplier_PANORVAT and its corresponding PharmacyId
            Dictionary<string, int> pharmacyDict = new Dictionary<string, int>();

            foreach (string sqlStatement in sqlStatements)
            {
                if (sqlStatement.StartsWith("INSERT"))
                {
                    // Extract values from the INSERT statement
                    string values = sqlStatement.Substring(sqlStatement.IndexOf("VALUES") + 6).Trim();
                    values = values.Trim('(', ')');

                    // Split values by comma
                    string[] valuesArray = values.Split(',');

                    // Extract Supplier_PANORVAT and PharmacyId from values
                    string supplierVATorPAN = valuesArray[3].Trim().Trim('\'');
                    int pharmacyId;

                    // Check if Supplier_PANORVAT already exists in the dictionary
                    if (!pharmacyDict.ContainsKey(supplierVATorPAN))
                    {
                        // If not, insert it into the Pharmacy table
                        pharmacyId = pharmacyDict.Count + 1;
                        string pharmacyInsertSQL = $"INSERT INTO Pharmacy (PharmacyId, PharmacyName, SupplierVATorPAN) VALUES ({pharmacyId}, '{valuesArray[2]}', '{supplierVATorPAN}');";
                        pharmacySQL.Add(pharmacyInsertSQL);

                        // Add Supplier_PANORVAT and PharmacyId to the dictionary
                        pharmacyDict.Add(supplierVATorPAN, pharmacyId);
                    }
                    else
                    {
                        // If Supplier_PANORVAT exists, retrieve its corresponding PharmacyId
                        pharmacyId = pharmacyDict[supplierVATorPAN];
                    }

                    // Generate SQL for updating PharmacyDetails table
                    string pharmacyDetailUpdateSQL = $"UPDATE PharmacyDetails SET PharmacyId = {pharmacyId}, BillNumber = {valuesArray[1]}, BillNepaliDate = {valuesArray[4]}, BillEnglishDate = {valuesArray[5]}, BranchId = {valuesArray[6]}, PharmacyType = {valuesArray[7]} WHERE PharmacyDetailId = {valuesArray[0]};";
                    pharmacyDetailSQL.Add(pharmacyDetailUpdateSQL);
                }
            }

            pharmacySQL.Add("SET IDENTITY_INSERT [dbo].[Pharmacy] OFF ");

            File.WriteAllLines("pharmacy_table.sql", pharmacySQL);
            File.WriteAllLines("pharmacy_details_update.sql", pharmacyDetailSQL);

            Console.WriteLine("Transformation complete. Pharmacy table SQL file generated.");

        }
    }
}