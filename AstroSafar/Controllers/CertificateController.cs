

using AstroSafar.Migrations;
using AstroSafar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;


public class CertificateController : Controller
{
    private readonly SpaceLearningDBContext _context;

    public CertificateController(SpaceLearningDBContext context)
    {
        _context = context;
    }

    //public IActionResult CertificatePrompt()
    //{
    //    // Retrieve the score from TempData
    //    int score = 0;
    //    if (TempData["Score"] != null)
    //    {
    //        score = Convert.ToInt32(TempData["Score"]);
    //    }
    //    else
    //    {
    //        // If TempData["Score"] is null, try to get the score from the database
    //        // Get the enrollment ID (that you stored in TempData["enId"])
    //        if (TempData["enId"] != null)
    //        {
    //            int enrollmentId = Convert.ToInt32(TempData["enId"]);

    //            // Get the latest exam result for this enrollment
    //            var examResult = _context.ExamResults
    //                .Where(r => r.EnrollmentId == enrollmentId)
    //                .OrderByDescending(r => r.CompletedAt)
    //                .FirstOrDefault();

    //            if (examResult != null)
    //            {
    //                score = examResult.Score;
    //            }
    //        }
    //    }

    //    // Create a view model to pass to the view
    //    var viewModel = new CertificateViewModel
    //    {
    //        Score = score
    //        // Add other properties as needed
    //    };

    //    return View(viewModel);
    //}

    public IActionResult CertificatePrompt()
    {
        // Retrieve the score from TempData
        int score = 0;
        if (TempData["Score"] != null)
        {
            score = Convert.ToInt32(TempData["Score"]);
        }
        else
        {
            // If TempData["Score"] is null, try to get the score from the database
            if (TempData["enId"] != null)
            {
                int enrollmentId = Convert.ToInt32(TempData["enId"]);

                // Try to determine which type of enrollment it is based on TempData
                string enrollmentType = TempData["enrollmentType"]?.ToString() ?? "primary";

                // Get the latest exam result for this enrollment
                var examResult = GetExamResultsByEnrollmentType(enrollmentId, enrollmentType);
                if (examResult != null)
                {
                    score = examResult.Score;
                }
            }
        }

        // Create a view model to pass to the view
        var viewModel = new CertificateViewModel
        {
            Score = score
            // Add other properties as needed
        };

        return View(viewModel);
    }

    private ExamResult GetExamResultsByEnrollmentType(int enrollmentId, string enrollmentType)
    {
        switch (enrollmentType.ToLower())
        {
            case "secondary":
                return _context.ExamResults
                    .Where(r => r.SecondaryEnrollId == enrollmentId)
                    .OrderByDescending(r => r.CompletedAt)
                    .FirstOrDefault();

            case "highersecondary":
                return _context.ExamResults
                    .Where(r => r.HigherSecondaryEnrollId == enrollmentId)
                    .OrderByDescending(r => r.CompletedAt)
                    .FirstOrDefault();

            default: // Primary enrollment
                return _context.ExamResults
                    .Where(r => r.EnrollmentId == enrollmentId)
                    .OrderByDescending(r => r.CompletedAt)
                    .FirstOrDefault();
        }
    }

    //  View certificate success message with buttons
    public IActionResult CertificateGenerated(int enrollmentId)
    {
        if (TempData["Message"] != null)
        {
            ViewBag.Message = TempData["Message"];
        }

        var certificate = _context.Certificates
            .Include(c => c.Enrollment)
                .ThenInclude(e => e.CourseAdmin)
            .FirstOrDefault(c => c.EnrollmentId == enrollmentId);

        if (certificate == null)
        {
            return RedirectToAction("Error", "Home");
        }

        return View(certificate); // Pass certificate as model ✅
    }

    public IActionResult ViewCertificate(int id)
    {
        var certificate = _context.Certificates
            .Include(c => c.Enrollment)
                .ThenInclude(e => e.CourseAdmin)
            .FirstOrDefault(c => c.Id == id);

        if (certificate == null) return NotFound();

        return View("CertificateTemplate", certificate); // Use HTML certificate template
    }





    // Download PDF using Rotativa
    public IActionResult DownloadCertificate(int id)
    {
        var certificate = _context.Certificates
            .Include(c => c.Enrollment)
            .ThenInclude(e => e.CourseAdmin)
            .FirstOrDefault(c => c.Id == id);

        if (certificate == null)
        {
            return NotFound();
        }

        return new ViewAsPdf("CertificatePdf", certificate)
        {
            FileName = $"Certificate_{certificate.Enrollment.FullName}.pdf", // 👈 this triggers download
            PageSize = Rotativa.AspNetCore.Options.Size.A4,
            PageMargins = new Rotativa.AspNetCore.Options.Margins(0, 0, 0, 0),
            CustomSwitches = "--disable-smart-shrinking"
        };

    }


}


