using BitirmeEntity.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BitirmeService.Services;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using BitirmeEntity.Models;

namespace BitirmeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeApiController : ControllerBase
    {
        private readonly ILogger<HomeApiController> _logger;

        public HomeApiController(ILogger<HomeApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetDoctors")]
        [Route("[action]")]
        [EnableCors("myAppCors")]
        [AllowAnonymous]
        public string GetDoctors()
        {
            var userList = new List<DoctorProfilesViewModel>();
            using (var db = new DbService())
            {
                userList = db.Query<DoctorProfilesViewModel>(@"
                            Select
                            dp.Id,
                            dp.DoctorName,
                            dp.DoctorEmail,
                            dp.DoctorHospital,
                            hp.HospitalName as [DoctorHospitalName],
                            dp.DoctorImageLink,
                            dp.DoctorPoliclinic,
                            pc.PoliclinicName as [DoctorPoliclinicName],
                            dp.DoctorTitle,
                            tl.TitleName as [DoctorTitleName],
                            dp.RowStateId
                            from DoctorProfiles as dp
                            Inner Join Policlinics as pc 
                            on pc.Id=dp.DoctorPoliclinic
                            Inner Join Titles as tl
                            on tl.Id = dp.DoctorTitle
                            Inner Join Hospitals as hp
                            on hp.Id = dp.DoctorHospital

                            ");
            }

            var userListData = JsonConvert.SerializeObject(userList);

            return userListData;
        }
        [HttpGet(Name = "GetPoliclinics")]
        [Route("[action]")]
        [EnableCors("myAppCors")]
        [AllowAnonymous]
        public string GetPoliclinics()
        {
            var policlinicList = new List<PoliclinicsViewModel>();
            using (var db = new DbService())
            {
                policlinicList = db.Query<PoliclinicsViewModel>(@"
                                    Select
                                    dp.Id,
                                    dp.PoliclinicName
                                    from Policlinics as dp
                                    ");
            }

            var policlinicListData = JsonConvert.SerializeObject(policlinicList);

            return policlinicListData;
        }

        [HttpGet(Name = "GetTitles")]
        [Route("[action]")]
        [AllowAnonymous]
        public string GetTitles()
        {
            var titleList = new List<TitlesViewModel>();
            using (var db = new DbService())
            {
                titleList = db.Query<TitlesViewModel>(@"
                                    Select
                                    dp.Id,
                                    dp.TitleName
                                    from Titles as dp
                                    ");
            }

            var titleListData = JsonConvert.SerializeObject(titleList);

            return titleListData;
        }

        [HttpGet(Name = "GetHospitals")]
        [Route("[action]")]
        [AllowAnonymous]
        public string GetHospitals()
        {
            var hospitalList = new List<HospitalsViewModel>();
            using (var db = new DbService())
            {
                hospitalList = db.Query<HospitalsViewModel>(@"
                                        Select
                                        dp.Id,
                                        dp.HospitalName
                                        from Hospitals as dp
                                        ");
            }

            var hospitalListData = JsonConvert.SerializeObject(hospitalList);

            return hospitalListData;
        }

        [HttpPost(Name = "GetDoctorsWithFilter")]
        [Route("[action]")]
        [Authorize]
        public string GetDoctorsWithFilter(int hospitalId, int policlinicId, int titleId)
        {

            var doctorList = new List<DoctorProfilesViewModel>();
            using (var db = new DbService())
            {
                var hospitalFilter = hospitalId != 0 ? $" and dp.DoctorHospital = {hospitalId}" : "";
                var policlinicFilter = policlinicId != 0 ? $" and dp.DoctorPoliclinic = {policlinicId}" : "";
                var titleFilter = titleId != 0 ? $" and dp.DoctorTitle = {titleId}" : "";
                doctorList = db.Query<DoctorProfilesViewModel>($@"
                            Select
                            dp.Id,
                            dp.DoctorName,
                            dp.DoctorEmail,
                            dp.DoctorHospital,
                            dp.DoctorImageLink,
                            dp.DoctorPoliclinic,
                            pc.PoliclinicName as [DoctorPoliclinicName],
                            dp.DoctorTitle,
                            tl.TitleName as [DoctorTitleName]
                            from DoctorProfiles as dp
                            Inner Join Policlinics as pc 
                            on pc.Id=dp.DoctorPoliclinic
                            Inner Join Titles as tl
                            on tl.Id = dp.DoctorTitle
                            where 1=1
                            {hospitalFilter}
                            {policlinicFilter}
                            {titleFilter}"
                            );
            }

            var doctorListData = JsonConvert.SerializeObject(doctorList);

            return doctorListData;
        }

        [HttpPost()]
        [Route("[action]")]
        [Authorize]
        public void UpdateDoctor([FromBody] DoctorProfilesViewModel model)
        {
            using (var db = new DbService())
            {
                var dbModel = db.FirstOrDefault<DoctorProfiles>($"{nameof(DoctorProfiles.Id)}={model.Id}");
                dbModel.DoctorEmail = model.DoctorEmail;
                dbModel.DoctorPoliclinic = model.DoctorPoliclinic;
                dbModel.DoctorImageLink = model.DoctorImageLink;
                dbModel.DoctorName = model.DoctorName;
                dbModel.DoctorHospital = model.DoctorHospital;
                dbModel.DoctorTitle = model.DoctorTitle;
                db.AddOrUpdateEntity<DoctorProfiles>(dbModel);
            }
        }

        [HttpGet(Name = "RunMedicanaHospitalWorker")]
        [Route("[action]")]
        [Authorize]
        public void RunMedicanaHospitalWorker()
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd";
            process.StartInfo.WorkingDirectory = @"C:\Users\Cagat\source\repos\BitirmeWebApi\MedicanaHospitalWorker\bin\Debug\net7.0";

            process.StartInfo.Arguments = "/c \"" + "MedicanaHospitalWorker.exe" + "\"";
            process.Start();
        }
        [HttpGet(Name = "RunHayatHospitalWorker")]
        [Route("[action]")]
        [Authorize]
        public void RunHayatHospitalWorker()
        {
            HayatHospitalWorker.Program deneme = new HayatHospitalWorker.Program();
            Process process = new Process();
            process.StartInfo.FileName = "cmd";
            process.StartInfo.WorkingDirectory = @"C:\Users\Cagat\source\repos\BitirmeWebApi\HayatHospitalWorker\bin\Debug\net7.0";

            process.StartInfo.Arguments = "/c \"" + "HayatHospitalWorker.exe" + "\"";
            process.Start();
        }
        [HttpGet(Name = "RunAcibademHospitalWorker")]
        [Route("[action]")]
        [Authorize]
        public void RunAcibademHospitalWorker()
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd";
            process.StartInfo.WorkingDirectory = @"C:\Users\Cagat\source\repos\BitirmeWebApi\AcibademHospitalWorker\bin\Debug\net7.0";

            process.StartInfo.Arguments = "/c \"" + "AcibademHospitalWorker.exe" + "\"";
            process.Start();
        }
        [HttpGet(Name = "RunDenemeConsole")]
        [Route("[action]")]
        [Authorize]
        public void RunDenemeConsole()
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.WorkingDirectory = @"C:\Users\Cagat\source\repos\BitirmeWebApi\DenemeConsole\bin\Debug\net7.0";
            process.StartInfo.ArgumentList.Add("/c DenemeConsole.exe");
            process.StartInfo.ArgumentList.Add("https://stackoverflow.com/questions/1012409/system-diagnostics-process-arguments");
            process.Start();
        }
    }
}
