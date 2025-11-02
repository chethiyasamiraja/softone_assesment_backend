using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http; 
using System.Web;  

namespace ReportingApi.Controllers
{
    public class ReportsController : ApiController
    {
        

        [HttpPost]
        [Route("api/reports/printinvoice")]
        public async Task<HttpResponseMessage> PrintInvoice([FromBody] PrintInvoiceDto dto)
        {
            if (dto == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid data.");
            }

            ReportDocument report = new ReportDocument();

            try
            {
                string reportPath = HttpContext.Current.Server.MapPath("~/Reports/InvoiceReport.rpt");
                report.Load(reportPath);

                // Assuming you have a method to convert dto to DataTable or dataset
                var dataTable = ConvertToDataTable(new List<PrintInvoiceDto> { dto });

                report.SetDataSource(dataTable);

                // Export to PDF
                Stream reportStream = report.ExportToStream(ExportFormatType.PortableDocFormat);

                // Prepare response
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(reportStream)
                };

                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "Invoice.pdf"
                };

                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"Error generating report: {ex.Message}");
            }
            finally
            {
                if (report != null)
                {
                    report.Close();
                    report.Dispose();
                }
            }
        }

        // Dummy method: Replace with actual conversion to DataTable
        private object ConvertToDataTable(List<PrintInvoiceDto> invoiceList)
        {
            // Implement your own logic to convert List<PrintInvoiceDto> to DataTable
            // You can use reflection or manually create DataTable and populate rows
            // This is just a placeholder
            return invoiceList; // Replace with actual DataTable
        }
    }
}