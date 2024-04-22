using BitMiracle.Docotic.Pdf;
using PDF_Extraction_WebAPI.Models;

namespace PDF_Extraction_WebAPI.Services
{
    public static class FileUploadHelper
    {
        public static InvoiceModel GetPDFContent(string file)
        {

            using (PdfDocument pdf = new PdfDocument(file))
            {
                string text = pdf.GetText();

                string delimiter = "\r\n";
                string[] contents = text.Split(delimiter);
                string date = "";
                string invoice = "";
                string total = "";
                foreach (string content in contents)
                {
                    if (content.ToLower().Contains("date"))
                    {
                        string delimiter2 = ":";
                        string[] contents2 = content.Split(delimiter2);
                        date = contents2[1];
                    }
                    if (content.ToLower().Contains("invoice #"))
                    {
                        string delimiter2 = "#";
                        string[] contents2 = content.Split(delimiter2);
                        invoice = contents2[1];
                    }
                    if (content.ToLower().Contains("total"))
                    {
                        string delimiter2 = "$";
                        string[] contents2 = content.Split(delimiter2);
                        total = contents2[1];
                    }
                }
                InvoiceModel model = new InvoiceModel();
                model.Date = date;
                model.Invoice = invoice;
                model.Total = total;
                return model;
            }
        }
    }
}
