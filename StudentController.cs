using DevExpress.Xpo;
using DevExpress.XtraPrinting.Native;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using peopleuniv_report.Models;
using peopleuniv_report.Services;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Reflection.Emit;
using System.Web;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace peopleuniv_report.Controllers
{
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;    
        private readonly IHttpContextAccessor _contextAccessor;
        private IHostingEnvironment Environment;
        private IConfiguration Configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly dbServiceStudent _dbServiceStudent;

        public StudentController(ILogger<StudentController> logger, IHttpContextAccessor contextAccessor, IHostingEnvironment _environment, IConfiguration _configuration, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _contextAccessor = contextAccessor;
            Environment = _environment;
            Configuration = _configuration;
            _dbServiceStudent  = new dbServiceStudent(_configuration);
            _webHostEnvironment = webHostEnvironment;
        }
        [Route("StudentIndex")]
        public IActionResult StudentIndex()
        {
            return View();
        }

        [Route("StudProfile")]
        public IActionResult StudProfile()
        {
            return View();
        }

       

        [Route("StudExamForm")]
        public IActionResult StudExamForm()
        {
            return View();
        }

        [Route("StudPayment")]
        public IActionResult StudentPayment()
        {
            return View();
        }

        [Route("StudExamPayment")]
        public IActionResult StudExamPayment()
        {
            return View();
        }

        [Route("ExamSchedule")]
        public IActionResult ExamSchedule()
        {
            return View();
        }

        [HttpGet]
        public async Task<IEnumerable<ExamScheduleModel>> GetExamSchedule(string ExamSeries, string Sem, string ExamType)
        {
            var enrollmentNo = _contextAccessor.HttpContext.Session.GetString("UserName");
            var result = await _dbServiceStudent.GetExamSchedule(ExamSeries, Sem,enrollmentNo, ExamType);
            return result;
        }

        [HttpGet]
        public async Task<IEnumerable<ExamScheduleModel>> GetsampleExamSchedule()
        {
            var enrollmentNo = _contextAccessor.HttpContext.Session.GetString("UserName");
            var result = await _dbServiceStudent.GetsampleExamSchedule(enrollmentNo);
            return result;
        }

        [HttpGet]
        public async Task<studNamesModel> GetLoginStudentDetails()
        {
            string UserName = _contextAccessor.HttpContext.Session.GetString("UserName");
            //dbServiceStudent.GetLoginStudentDetails(UserName)
            var result = await _dbServiceStudent.GetLoginStudentDetails(UserName);
            return result;
        }

        [HttpGet]
        public async Task<StudentMasterModel> GetStudentDetails()
       {
            string UserName = _contextAccessor.HttpContext.Session.GetString("UserName");
            //dbServiceStudent.GetLoginStudentDetails(UserName)
            var result = await _dbServiceStudent.GetStudentDetails(UserName);
            return result;
        }

        [HttpPost]
        public string UpdateStudentDetails(StudentMasterModel JsonInput)
        {
            var UserName = _contextAccessor.HttpContext.Session.GetString("UserName");
            var res = _dbServiceStudent.UpdateStudentDetails(JsonInput, UserName);
            return JsonConvert.SerializeObject(res.Result);
        }

        //[HttpGet]
        //public List<PaymentDemandNote> GetRegularStudentDemandNotes()
        //{
        //    var UserName = _contextAccessor.HttpContext.Session.GetString("UserName");
        //    var result = _dbServiceStudent.GetRegularStudentDemandNotes(UserName);
        //    return result;
        //}

        [HttpGet]
        public async Task<IEnumerable<PaymentDemandNote>> GetRegularStudentDemandNotes()
        {
            var enrollmentNo = _contextAccessor.HttpContext.Session.GetString("UserName");
            var result = await _dbServiceStudent.GetRegularStudentDemandNotes(enrollmentNo);
            return result;
        }

        [Route("StudRevalutionNew")]
        public IActionResult RevaluationNew()
        {
            return View();
        }

        [Route("register-reval-students")]
        public async Task<IActionResult> Revaluation()
        {
            var enroll = _contextAccessor.HttpContext.Session.GetString("UserName");
            var std = await _dbServiceStudent.GetBatchReguSemMainEx(enroll);
            var subjList = await _dbServiceStudent.getSubjectsbyBatch(std.Sem, std.Regulation, std.CollegeCode, enroll);
            ViewBag.SubjList = subjList;
            var regpcode = await _dbServiceStudent.getRevaluSubjs(enroll);
            if (regpcode != null && regpcode.Count() > 0)
            {
                ViewBag.selectedSubjs = regpcode;
            }
            return View(std);
        }

        //[Route("exam-form-students")]
        //public async Task<IActionResult> StudExamForm(string I, string II)
        //{
        //    string enroll= _contextAccessor.HttpContext.Session.GetString("UserName");
        //    string sem="I";
        //    string prog = enroll.Substring(10, 3);
        //    string br = "MBBS";
        //    //string sem = "I";
        //    switch (prog)
        //    {
        //        case "03C":
        //            br = "DDH";
        //            break;
        //        case "04C":
        //            br = "DDM";
        //            break;
        //        case "01A":
        //            br = "MBBS";
        //            break;
        //        case "02A":
        //            br = "BDS";
        //            break;
        //        case "04B":
        //            br = "MDS";
        //            break;
        //        case "01B":
        //            br = "MDMS";
        //            break;
        //        case "03B":
        //        case "02B":
        //        case "10B":
        //            br = "MSCMED";
        //            break;
        //    }
        //    /*var s = _studentService.findStuCurExamSem(enroll, br).Result;
        //    if(s.Count() > 0)
        //    {
        //        sem = s.FirstOrDefault().Sem;
        //    }*/
        //    var lis = await _dbServiceStudent.ExamFormData(enroll, br, sem);
        //    var std = await _dbServiceStudent.getProfileDetails(enroll);
        //    ViewBag.std = std;
        //    return View(lis);
        //}

        [Route("exam-form-student")]
        public async Task<IActionResult> StudExamForm(string ExamSeries, string sem, string ExamType)
        {
            string EnrollmentNo = _contextAccessor.HttpContext.Session.GetString("UserName");
            //string sem="I";
            //string prog = enroll.Substring(10, 3);
            //string br = "MBBS";
            ////string sem = "I";
            //switch (prog)
            //{
            //    case "03C":
            //        br = "DDH";
            //        break;
            //    case "04C":
            //        br = "DDM";
            //        break;
            //    case "01A":
            //        br = "MBBS";
            //        break;
            //    case "02A":
            //        br = "BDS";
            //        break;
            //    case "04B":
            //        br = "MDS";
            //        break;
            //    case "01B":
            //        br = "MDMS";
            //        break;
            //    case "03B":
            //    case "02B":
            //    case "10B":
            //        br = "MSCMED";
            //        break;
            //}
            /*var s = _studentService.findStuCurExamSem(enroll, br).Result;
            if(s.Count() > 0)
            {
                sem = s.FirstOrDefault().Sem;
            }*/
            var lis = await _dbServiceStudent.ExamFormData(ExamSeries, sem, ExamType, EnrollmentNo);
            var std = await _dbServiceStudent.getProfileDetails(EnrollmentNo, ExamSeries);
            ViewBag.std = std;
            return View(lis);
            //return View("~/Views/Student/StudExamForm.cshtml");

        }

        [Route("insert-reval-students")]
        [HttpPost]
        public async Task<bool> RevaluationDBUpdate(string enroll, string colcode, string prog, string amnt, string pcodes)
        {
            var s = await _dbServiceStudent.updateRevaluTable(enroll, colcode, prog, amnt, pcodes);
            return (s);
        }


        [Route("pay-reval-online-paytm")]
        public IActionResult PaymentGateway()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StudentPhotoUpload(StudentMasterModel student)
        {
            var UserName = _contextAccessor.HttpContext.Session.GetString("UserName");
            if (student.Filepath != null)
            {
                var fileExtension = Path.GetExtension(student.Filepath.FileName).ToLower();

                if (fileExtension == ".jpg")
                { 
                var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "image/StudentImages");
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }
               

                //var fileExtension = Path.GetExtension(student.Filepath.FileName);
                var fileName = UserName + fileExtension;
                var filePath = Path.Combine(uploads, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await student.Filepath.CopyToAsync(fileStream);
                }

                student.StudentPhoto = fileName;
                    //employee.EmployeePhoto = "image/Employeephoto/" + fileName;

                }
                else
                {
                    return Json(new { status = "failed" });
                }
            }

            var result = await _dbServiceStudent.StudentPhotoUpload(student,UserName);
            if (result)
            {
                return Json(new { status = "success" });
            }

            return Json(new { status = "failed" });
        }


        [HttpGet]
        public async Task<List<ExamScheduleModel>> GetExamSchedulePayment(string Sem, string ExamSeries, string ExamType)
        {
            string EnrollmentNo = _contextAccessor.HttpContext.Session.GetString("UserName");
            var res = await _dbServiceStudent.GetExamSchedulePayment(Sem, ExamSeries, EnrollmentNo,ExamType);
            return res.ToList();
        }

        [HttpPost]
        public async Task<string> MakeExamPayment(string FeesAmount,string CollegeCode, string ExamSeries, string SemYear)
        {
            var enroll = _contextAccessor.HttpContext.Session.GetString("UserName");
            var Makepay = await _dbServiceStudent.MakeExamPayment(enroll, FeesAmount,CollegeCode, ExamSeries, SemYear);
            return Makepay.ToString();
        }

        [HttpGet]
        public async Task<string> GetPaymentStatus(string ExamSeries, string Sem,string ExamType)
        {
            var enroll = _contextAccessor.HttpContext.Session.GetString("UserName");
            var Makepay = await _dbServiceStudent.GetPaymentStatus(enroll, ExamSeries, Sem,ExamType);
            return Makepay.ToString();
        }

    }
}
