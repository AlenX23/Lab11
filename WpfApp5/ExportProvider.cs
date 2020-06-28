using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.Linq;

namespace WpfApp5
{
    class ExportProvider<T>
    {
        protected IEnumerable<T> data;
        public ExportProvider(IEnumerable<T> data)
        {
            this.data = data;
        }

        public virtual void ExportTo(string filename) { }
    }

    class StudentsToXLSProvider : ExportProvider<Model.Student>
    {
        public StudentsToXLSProvider(IEnumerable<Model.Student> data) : base(data) { }

        public override void ExportTo(string filename)
        {
            SpreadsheetDocument document = CreateSpreadsheetWorkbook(filename);
            var workbookpart = document.WorkbookPart;
            var sheetData = workbookpart.WorksheetParts.First().Worksheet.GetFirstChild<SheetData>();

            foreach (var value in data)
            {
                Row row = new Row();
                row.Append(new Cell(new CellValue(value.Id.ToString())));

                var name = new Cell(new CellValue(value.Name));
                name.DataType = new EnumValue<CellValues>(CellValues.String);
                row.Append(name);

                var admission = new Cell(new CellValue(value.Admission.ToString()));
                admission.DataType = new EnumValue<CellValues>(CellValues.Date);
                row.Append(admission);

                var track = new Cell(new CellValue(value.Group.Track.ToString()));
                track.DataType = new EnumValue<CellValues>(CellValues.String);
                row.Append(track);

                row.Append(new Cell(new CellValue(value.Group.Course.ToString())));
                row.Append(new Cell(new CellValue(value.Group.Number.ToString())));
                sheetData.Append(row);
            }

            workbookpart.Workbook.Save();
            document.Close();
        }

        public static SpreadsheetDocument CreateSpreadsheetWorkbook(string filepath)
        {
            // Create a spreadsheet document by supplying the filepath.
            // By default, AutoSave = true, Editable = true, and Type = xlsx.
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filepath, SpreadsheetDocumentType.Workbook);

            // Add a WorkbookPart to the document.
            WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();

            // Add a WorksheetPart to the WorkbookPart.
            WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            // Add Sheets to the Workbook.
            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            // Append a new worksheet and associate it with the workbook.
            Sheet sheet = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "mySheet"
            };
            sheets.Append(sheet);

            return spreadsheetDocument;
        }
    }
}