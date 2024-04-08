using Automation_Project.Constitution_Seed_Automation;
using Automation_Project.TransformAndGenrateSQL;
using System.Text;
using System.Text.RegularExpressions;

namespace Automation_Project
{
    internal class Program
    {
        public static void Main(string[] args)
        {

            //#region Constitution
            //var constitution = new Constitution();
            //string constitionSqlFilePath = "C:\\Users\\someo\\OneDrive\\PREV FILES\\Documents\\Visual Studio 2022\\Automation\\Automation Project\\sqlfile.sql";
            //constitution.Seed(constitionSqlFilePath);
            //#endregion

            string sqlFilePath = "C:\\Users\\someo\\OneDrive\\PREV FILES\\Documents\\Visual Studio 2022\\Automation\\Automation Project\\TransformAndGenrateSQL\\PharmacyDetails.sql";

            TransformSql.TransformAndGenerateSQL(sqlFilePath);
        }
    }
}