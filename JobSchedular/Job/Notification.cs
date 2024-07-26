using JobSchedular.Controllers;
using JobSchedular.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using Quartz;
using System.Configuration;

namespace JobSchedular.Job
{
    public class Notification:Controller,IJob
    {
        private readonly IConfiguration configuration;
        public Notification(IConfiguration config)
        {
            configuration = config;
        }
        //private readonly ILogger<Notification> _logger;
        //public Notification(ILogger<Notification> logger)
        //{
        //    this._logger = logger;
                
        // }
        public Task Execute(IJobExecutionContext context) {

            Import();
            //_logger.LogInformation($"Notify the date {DateTime.Now} and Employees Data {EmployeesController.Import()}");
            return Task.CompletedTask;
        //throw new NotImplementedException();
        }
        public void  Import()
        {

            var pathfile = "D:\\224\\DotNetPractice\\MVC projects\\Excel\\EmployeeFile.xlsx";
            var List = new List<Employees>();
            using (var package = new ExcelPackage(pathfile))
            {                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                var RowCount = worksheet.Dimension.Rows;
                string connectionstring = configuration.GetConnectionString("JobSchedularContext");
                SqlConnection connection = new SqlConnection(connectionstring);
                connection.Open();
                SqlCommand cmd = new SqlCommand("truncate table Employees", connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                for (int row = 2; row <= RowCount; row++)
                {
                    List.Add(new Employees
                    {
                        EmpId = Convert.ToInt32(worksheet.Cells[row, 1].Value.ToString().Trim()),
                        JobName = worksheet.Cells[row, 2].Value.ToString().Trim(),
                        EmpName = worksheet.Cells[row, 3].Value.ToString().Trim(),
                        EmpAge = Convert.ToInt32(worksheet.Cells[row, 4].Value.ToString().Trim()),
                        Gender = worksheet.Cells[row, 5].Value.ToString().Trim(),
                        EmpEmail = worksheet.Cells[row, 6].Value.ToString().Trim(),
                        EmpPhone = worksheet.Cells[row, 7].Value.ToString().Trim(),
                        EmpAddress = worksheet.Cells[row, 8].Value.ToString().Trim(),
                    });
                    Console.WriteLine(List.Last().EmpId);
                    connectionstring = configuration.GetConnectionString("JobSchedularContext");
                    connection = new SqlConnection(connectionstring);
                    connection.Open();
                    cmd = new SqlCommand("insert into Employees (JobName,EmpName,EmpAge,Gender,EmpEmail,EmpPhone,EmpAddress)" + "VALUES(@jobName,@empName,@empAge,@gender,@empEmail,@empPhone,@empAddress)", connection);
                    //cmd.Parameters.AddWithValue("@empid", List.Last().EmpId);
                    cmd.Parameters.AddWithValue("@jobName", List.Last().JobName);
                    cmd.Parameters.AddWithValue("@empName", List.Last().EmpName);
                    cmd.Parameters.AddWithValue("@empAge", List.Last().EmpAge);
                    cmd.Parameters.AddWithValue("@gender", List.Last().Gender);
                    cmd.Parameters.AddWithValue("@empEmail", List.Last().EmpEmail);
                    cmd.Parameters.AddWithValue("@empPhone", List.Last().EmpPhone);
                    cmd.Parameters.AddWithValue("@empAddress", List.Last().EmpAddress);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }

        }
    }
}
