using EQC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignatureWeb.Data;
using SignatureWeb.Shared.Models;
using System.Data;

namespace SignatureWeb.Services
{
    public class ConstCheckSignatureService
    {
        private readonly IDbContextFactory<MyDbContext> _db;
        MySqlConnection mySqlConnection;
        public ConstCheckSignatureService(IDbContextFactory<MyDbContext> db, IConfiguration configuration)
        {
            _db = db;
            mySqlConnection = new MySqlConnection(configuration);
        }

        public async Task uploadImage(ConstCheckSignature m)
        {
            try
            {


                using (var context = _db.CreateDbContext())
                {
                    var deleteSignature = context.constCheckSignatures.Where(row => row.CreateTime.Value.AddHours(1) < DateTime.Now && row.ConstCheckSeq == 0);
                    context.constCheckSignatures.RemoveRange(deleteSignature);
                    context.SaveChanges();

                    var originSignature = context.constCheckSignatures.Where(
                        r => r.ConstCheckSeq == 0 && r.Token == m.Token && r.SignatureVal == m.SignatureVal
                    ).FirstOrDefault();


                    if (originSignature != null)
                    {
                        m.Seq = originSignature.Seq;
                        m.CreateTime = originSignature.CreateTime;
                        context.Entry(originSignature).CurrentValues.SetValues(m);
                        originSignature.ModifyTime = DateTime.Now;
                        
                    }
                    else
                    {
                        m.CreateTime = DateTime.Now;
                        context.constCheckSignatures.Add(m);
                    }

                    context.SaveChanges();
                }
            }
            catch(Exception e)
            {

            }
        }
        public async Task<string[] > GetSignatureValOption(int engSeq, int Role)
        {
            using (var context = _db.CreateDbContext())
            {
              

                if(Role == 2 || Role == 3)
                {
                    string sql = @"
                        SELECT 
	                    u.DisplayName
                    FROM EngSupervisor ee
                      inner Join UserMain u on u.Seq =  ee.UserMainSeq
                      where ee.EngMainSeq = @engSeq and ee.UserKind =@Role
                    ";

                    var cmd = mySqlConnection.GetCommand(sql);

                    cmd.Parameters.AddWithValue("@engSeq", engSeq);
                    int targetRole;
                    switch(Role)
                    {
                        case 3: targetRole = 0;break;
                        case 2: targetRole = 1;break;
                        default: return null;
                    }

                    cmd.Parameters.AddWithValue("@Role", targetRole);

                    var arr = mySqlConnection.GetDataTable(cmd)
                    .Rows.Cast<DataRow>()
                    .Select(row => row.Field<string>("DisplayName"))
                    .ToArray();

                    sql = @"
                        SELECT 
	                    u.SupervisorDirector
                      FROM EngMain u
                      where u.Seq = @engSeq and u.SupervisorDirector is not null
                    ";
                    cmd = mySqlConnection.GetCommand(sql);
                    cmd.Parameters.AddWithValue("@engSeq", engSeq);
                    var arr2 = mySqlConnection.GetDataTable(cmd)
                    .Rows.Cast<DataRow>()
                    .Select(row => row.Field<string>("SupervisorDirector"))
                    .ToArray();

                    return arr.Concat(arr2).ToArray();
                }
                //施工廠商
                if(Role == 4)
                {
                    string sql2 = @"
                        SELECT 
	                    u.BuildContractorContact
                      FROM EngMain u
                      where u.Seq = @engSeq and u.BuildContractorContact is not null
                    ";
                    var cmd2 = mySqlConnection.GetCommand(sql2);
                    cmd2.Parameters.AddWithValue("@engSeq", engSeq);
                    return mySqlConnection.GetDataTable(cmd2)
                        .Rows.Cast<DataRow>()
                        .Select(row => row.Field<string>("BuildContractorContact"))
                        .ToArray();
                }
                //抽查者
                if (Role == 1)
                {
                    string sql2 = @"
                        SELECT  top 1
	                    uu.DisplayName
                      FROM EngMain u
                        inner join ConstCheckUser cc on cc.EngSeq = u.Seq
                        inner join UserMain uu on uu.Seq = cc.UserSeq

                      where u.Seq = @engSeq
                    ";
                    var cmd2 = mySqlConnection.GetCommand(sql2);
                    cmd2.Parameters.AddWithValue("@engSeq", engSeq);
                    return mySqlConnection.GetDataTable(cmd2)
                        .Rows.Cast<DataRow>()
                        .Select(row => row.Field<string>("DisplayName"))
                        .ToArray();
                }
                return new string[0];
            }
        }
    }
}
