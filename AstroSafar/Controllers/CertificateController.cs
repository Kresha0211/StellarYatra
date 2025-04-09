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
    public IActionResult CertificatePrompt()
    {
        return View("~/Views/Certificate/CertificatePrompt.cshtml");
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



    //public IActionResult CertificateGenerated(int enrollmentId)
    //{
    //    var certificate = _context.Certificates
    //        .Include(c => c.Enrollment)
    //        .ThenInclude(e => e.CourseAdmin)
    //        .FirstOrDefault(c => c.EnrollmentId == enrollmentId);

    //    if (certificate == null)
    //    {
    //        return NotFound("Certificate not found.");
    //    }

    //    return new ViewAsPdf("CertificatePdf", certificate)
    //    {
    //        FileName = $"Certificate_{enrollmentId}.pdf",
    //        PageSize = Rotativa.AspNetCore.Options.Size.A4,
    //        PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
    //    };
    //}


    // View certificate in HTML
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
            FileName = $"Certificate_{certificate.Enrollment.FullName}.pdf",
            PageSize = Rotativa.AspNetCore.Options.Size.A4,
            PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
        };

    }
}