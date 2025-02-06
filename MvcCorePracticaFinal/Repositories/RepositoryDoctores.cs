using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using MvcCorePracticaFinal.Models;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MvcCorePracticaFinal.Repositories
{
    #region
//    create procedure SP_INSERTAR_DOCTOR
//    (@hospitalCod int, @apellido nvarchar(50), @especialidad nvarchar(50), @salario int)
//as
//declare @nextId int
//select @nextId = MAX(DOCTOR_NO) + 1 from DOCTOR
//insert into DOCTOR values(@hospitalCod, @nextId, @apellido, @especialidad, @salario)
//go
    #endregion
    public class RepositoryDoctores
    {
        private DataTable tablaDoctores;
        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryDoctores()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Encrypt=True;Trust Server Certificate=True";
            this.tablaDoctores = new DataTable();
            string sql = "select * from DOCTOR";
            SqlDataAdapter ad = new SqlDataAdapter(sql, connectionString);
            ad.Fill(this.tablaDoctores);
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<Doctor> GetDoctores()
        {
            var consulta = from datos in this.tablaDoctores.AsEnumerable()
                           select datos;
            List<Doctor> doctores = new List<Doctor>();
            foreach (var row in consulta)
            {
                Doctor d = new Doctor
                {
                    HospitalCod = row.Field<int>("HOSPITAL_COD"),
                    DoctorNo = row.Field<int>("DOCTOR_NO"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Especialidad = row.Field<string>("ESPECIALIDAD"),
                    Salario = row.Field<int>("SALARIO")

                };
                doctores.Add(d);
            }
            return doctores;
        }

        public Doctor FindDoctor(int numeroDoc)
        {
            var consulta = from datos in this.tablaDoctores.AsEnumerable()
                           where datos.Field<int>("DOCTOR_NO") == numeroDoc
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                var row = consulta.First();
                Doctor d = new Doctor
                {
                    HospitalCod = row.Field<int>("HOSPITAL_COD"),
                    DoctorNo = row.Field<int>("DOCTOR_NO"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Especialidad = row.Field<string>("ESPECIALIDAD"),
                    Salario = row.Field<int>("SALARIO")

                };
                return d;
            }
        }

        public async Task InsertDoctorAsync(int hospitalCod, string apellido, string especialidad, int salario)
        {
            string sql = "SP_INSERTAR_DOCTOR";
            this.com.Parameters.AddWithValue("@hospitalCod", hospitalCod);
            this.com.Parameters.AddWithValue("@apellido", apellido);
            this.com.Parameters.AddWithValue("@especialidad", especialidad);
            this.com.Parameters.AddWithValue("@salario", salario);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public List<string> GetEspecialidadDoctores()
        {
            var consulta = (from datos in this.tablaDoctores.AsEnumerable()
                            select datos.Field<string>("ESPECIALIDAD")).Distinct();
            return consulta.ToList();
        }

        public List<Doctor> GetDoctoresEspecialidad(string especialidad)
        {
            var consulta = from datos in this.tablaDoctores.AsEnumerable()
                           where datos.Field<string>("ESPECIALIDAD") == especialidad
                           select datos;

            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                List<Doctor> doctores = new List<Doctor>();
                foreach (var row in consulta)
                {
                    Doctor d = new Doctor
                    {
                        HospitalCod = row.Field<int>("HOSPITAL_COD"),
                        DoctorNo = row.Field<int>("DOCTOR_NO"),
                        Apellido = row.Field<string>("APELLIDO"),
                        Especialidad = row.Field<string>("ESPECIALIDAD"),
                        Salario = row.Field<int>("SALARIO")

                    };
                    doctores.Add(d);
                }
                return doctores;
            }
        }

        public void DeleteDoctor(int numeroDoc)
        {
            string sql = "delete from DOCTOR where DOCTOR_NO=@numeroDoc";
            this.com.Parameters.AddWithValue("@numeroDoc", numeroDoc);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public async Task UpdateDoctorAsync(int hospitalCod, int doctorNo, string apellido, string especialidad, int salario)
        {
            string sql = "update DOCTOR set HOSPITAL_COD=@hospitalCod "
        + ", DOCTOR_NO=@doctorNo, APELLIDO=@apellido, ESPECIALIDAD=@especialidad, SALARIO=@salario "
        + " where DOCTOR_NO=@doctorNo";
            this.com.Parameters.AddWithValue("@hospitalCod", hospitalCod);
            this.com.Parameters.AddWithValue("@doctorNo", doctorNo);
            this.com.Parameters.AddWithValue("@apellido", apellido);
            this.com.Parameters.AddWithValue("@especialidad", especialidad);
            this.com.Parameters.AddWithValue("@salario", salario);    
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }
    }
}
