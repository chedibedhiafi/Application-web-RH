using ApplicationCore.Domain;
using ApplicationCore.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Numerics;
using System.Reflection.Metadata;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Runtime.Intrinsics.X86;

namespace UI.Web.Controllers
{
    public class AttestationController : Controller
    {
        IServiceAttestation sa;
        IServiceEmployees se;
        IServiceTypeAttestation sta;

       
        // GET: AttestationController

        public AttestationController(IServiceTypeAttestation sta ,IServiceAttestation sa, IServiceEmployees se)
        {
            this.sta = sta;
            this.sa = sa;
            this.se = se;
          
        }
        

        private byte[] GenerateArabicPdfContent(int id, string imagePath)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                XImage image = XImage.FromFile(imagePath);

                // Calculate image dimensions and position
                double imageWidth = page.Width - 20;
                double imageHeight = image.PixelHeight * (imageWidth / image.PixelWidth);
                double imageX = 10; // X-coordinate of the top-left corner
                double imageY = 30; // Y-coordinate of the top-left corner

                gfx.DrawImage(image, imageX, imageY, imageWidth, imageHeight);

                // Save the document to the memory stream
                document.Save(ms);
                document.Close();

                return ms.ToArray();
            }
        }


        private string GenerateArabicTextImage(string arabicText)
        {
            // You'll need to use an appropriate library to create images with Arabic text.
            // For example, using System.Drawing, you can create an image using GDI+.
            Bitmap image = new Bitmap(800, 600); // Set dimensions as needed
            using (Graphics graphics = Graphics.FromImage(image))
            {
                Font arabicFont = new Font("Arial", 12); // Use an appropriate Arabic font

                // Set rendering options for Arabic text
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                format.FormatFlags = StringFormatFlags.DirectionRightToLeft;

                // Draw the Arabic text onto the image
                graphics.DrawString(arabicText, arabicFont, Brushes.Black, new RectangleF(0, 0, image.Width, image.Height), format);
            }

            // Save the image to a temporary file
            string imagePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.png");
            image.Save(imagePath, ImageFormat.Png);

            return imagePath;
        }


        public IActionResult DownloadArabicPdf(int id)
        {
            var attestation = sa.GetById(id);
            if (attestation == null)
            {
                return RedirectToAction(nameof(Index));
            }

            string arabicDescription = attestation.Description; // Use the description from the attestation object

            // Generate an Arabic text image from the description
            string imagePath = GenerateArabicTextImage(arabicDescription);

            byte[] pdfBytes = GenerateArabicPdfContent(id, imagePath);

            try
            {
                // Delete the temporary image file
              //  File.Delete(imagePath);
            }
            catch (Exception ex)
            {
                // Handle the exception here, e.g., log it
                // You can replace "Console.WriteLine" with your preferred logging mechanism
                Console.WriteLine($"Error deleting temporary image file: {ex.Message}");
            }

            string filename = $"Attestation_Arabic{id}.pdf";

            return File(pdfBytes, "application/pdf", filename);
        }




        public IActionResult DownloadPdf(int id)
        {
            // Retrieve the attestation based on the provided ID
            var attestation = sa.GetById(id);
            if (attestation == null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Construct the path to the uploaded signature image
            string signatureImageFilename = attestation.DocumentAttestation; // Assuming the property name
            string signatureImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadsAttestation", signatureImageFilename);

            // Generate the PDF content with the embedded signature
            byte[] pdfBytes = GeneratePdfContent(id, attestation.Description, signatureImagePath);

            // Generate the filename
            string filename = $"Attestation{id}.pdf";

            // Return the PDF file for download
            return File(pdfBytes, "application/pdf", filename);
        }



        private byte[] GeneratePdfContent(int id, string description, string signatureImagePath)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont titleFont = new XFont("Arial", 16, XFontStyle.Bold);
                XFont regularFont = new XFont("Arial", 12);

                // Calculate line height
                double lineHeight = gfx.MeasureString("A", regularFont).Height;

                // Define the rectangle for text
                XRect rect = new XRect(10, 30, page.Width - 20, page.Height - 60);

                // Draw lines to create a border around the page
                double topMargin = 10;
                double leftMargin = 10;
                double rightMargin = page.Width - 10;
                double bottomMargin = page.Height - 10;

                gfx.DrawLine(XPens.Black, new XPoint(leftMargin, topMargin), new XPoint(rightMargin, topMargin)); // Top
                gfx.DrawLine(XPens.Black, new XPoint(leftMargin, topMargin), new XPoint(leftMargin, bottomMargin)); // Left
                gfx.DrawLine(XPens.Black, new XPoint(rightMargin, topMargin), new XPoint(rightMargin, bottomMargin)); // Right
                gfx.DrawLine(XPens.Black, new XPoint(leftMargin, bottomMargin), new XPoint(rightMargin, bottomMargin)); // Bottom

                // Draw the title at the top
                string title = "Attestation de Présence";
                XSize titleSize = gfx.MeasureString(title, titleFont);
                XRect titleRect = new XRect(rect.Left, rect.Top, rect.Width, titleSize.Height);
                gfx.DrawString(title, titleFont, XBrushes.DarkBlue, titleRect, XStringFormats.Center);

                // Adjust the starting position for content below the title
                double yPos = titleRect.Bottom + lineHeight;

                // Split the description into paragraphs based on line breaks
                string[] paragraphs = description.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.None);

                // Draw each paragraph with lines on the PDF
                foreach (string paragraph in paragraphs)
                {
                    string[] lines = paragraph.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in lines)
                    {
                        XSize textSize = gfx.MeasureString(line, regularFont);
                        XRect layoutRect = new XRect(rect.Left, yPos, textSize.Width, lineHeight);
                        gfx.DrawString(line, regularFont, XBrushes.Black, layoutRect, XStringFormats.TopLeft);
                        yPos += lineHeight;
                    }
                    yPos += lineHeight * 2; // Add extra spacing between paragraphs
                }

                // Draw the signature line
                double signatureLineY = page.Height - 40;
                gfx.DrawLine(XPens.Black, new XPoint(leftMargin, signatureLineY), new XPoint(rightMargin, signatureLineY));

                // Load the image of the signature
                XImage signatureImage = XImage.FromFile(signatureImagePath);
                double imageWidth = 150; // Adjust the image width as needed
                double imageHeight = signatureImage.Height * (imageWidth / signatureImage.PixelWidth);

                // Calculate the coordinates for the bottom right corner
                double imageX = page.Width - imageWidth - 20; // 10 is the right margin
                double imageY = page.Height - imageHeight - 45; // 20 is a vertical margin

                XRect imageRect = new XRect(imageX, imageY, imageWidth, imageHeight);
                gfx.DrawImage(signatureImage, imageRect);

                // Save the document to the memory stream
                document.Save(ms);
                document.Close();

                return ms.ToArray();
            }
        }



        [Authorize(Policy = "EmployeeRead")]

        public ActionResult Index()
    {
        return View(sa.GetMany());
    }

        [Authorize(Policy = "EmployeeRead")]

        public ActionResult IndexFront()
        {
            return View(sta.GetMany());
        }
        // GET: AttestationController/Details/5
        public ActionResult Details(int id)
    {
        return View();
    }

        // GET: AttestationController/Create
        [Authorize(Policy = "EmployeeCreate")]

        public ActionResult Create()
        {
            ViewBag.TypeAttestation = new SelectList(sta.GetMany(), "TypeId", "Type");

            return View();
        }

        // POST: AttestationController/Create
        [HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Create(Attestation collection, IFormFile DocumentUpload)
{
    try
    {
        if (DocumentUpload != null && DocumentUpload.Length > 0)
        {
            var fileName = Path.GetFileName(DocumentUpload.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadsAttestation", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                DocumentUpload.CopyTo(stream);
            }

            collection.DocumentAttestation = fileName;
        }

        // Set the TypeAttestation property based on the selected value in the dropdown
        var selectedTypeAttestation = sta.GetById(collection.TypeAttestationFk);
        if (selectedTypeAttestation != null)
        {
            collection.TypeAttestation = selectedTypeAttestation;
            collection.Description = selectedTypeAttestation.Contenu;

        }
                string mail = User.Identity.Name;
                Employees employee = se.GetMany().FirstOrDefault(e => e.Email == mail);
                collection.Description.Replace("Nom du Participant : [Nom et prénom]", "Nom du Participant : ["+employee.Nom+" et"+employee.Prenom+"]");

        sa.Add(collection);
        sa.Commit();
        return RedirectToAction("IndexFront");
    }
    catch
    {
        // Handle the error appropriately, e.g., by displaying an error message
        return View();
    }
}

        /*public ActionResult Create(Attestation collection , IFormFile DocumentUpload)
        {
            try
            {
                if (DocumentUpload != null && DocumentUpload.Length > 0)
                {
                    var fileName = Path.GetFileName(DocumentUpload.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadsAttestation", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        DocumentUpload.CopyTo(stream);
                    }
                }
                    collection.Description = collection.TypeAttestation.Contenu;
                
                sa.Add(collection);
                sa.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/

        // GET: AttestationController/Edit/5
        [Authorize(Policy = "EmployeeUpdate")]
        public ActionResult Edit(int id)
        {
            var attestation = sa.GetById(id);
            if (attestation == null)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TypeAttestation = new SelectList(sta.GetMany(), "TypeId", "Type");


            return View(attestation);
        }

        // POST: AttestationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Attestation updatedAttestation, IFormFile DocumentUpload)
        {
            try
            {
                var attestation = sa.GetById(id);

                if (attestation == null)
                {
                    return RedirectToAction("Index");
                }

                attestation.Description= updatedAttestation.Description;
                attestation.TypeAttestationFk = updatedAttestation.TypeAttestationFk;
            


                if (DocumentUpload != null && DocumentUpload.Length > 0)
                {
                    var fileName = Path.GetFileName(DocumentUpload.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadsAttestation", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        DocumentUpload.CopyTo(stream);
                    }

                    // Assign the new file name to the Photo property
                    attestation.DocumentAttestation = fileName;
                    ViewBag.TypeAttestationFk = new SelectList(sta.GetMany(), "TypeId", "Type", attestation.TypeAttestationFk);

                }



                sa.Update(attestation);
                sa.Commit();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Policy = "EmployeeDelete")]

        // GET: AttestationController/Delete/5
        public ActionResult Delete(int id)
        {
            var attestation = sa.GetById(id);
            if (attestation == null)
            {
                return RedirectToAction("Index");
            }


            return View(attestation);
        }

        // POST: AttestationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var attestation = sa.GetById(id);
                if (attestation == null)
                {
                    return RedirectToAction("Index");
                }

                sa.Delete(attestation);
                sa.Commit();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //function

    }
}
