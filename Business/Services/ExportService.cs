using Business.Contracts.Models.Input.Export;
using Business.Contracts.Models.Output.Export;
using Business.Contracts.Services;
using Business.Errors;
using Business.Models.Output.Export;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Contracts.Errors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Business.Services
{
    public class ExportService : IExportService
    {
        protected readonly IErrorCollector _errorCollector;

        public ExportService(IErrorCollector errorCollector)
        {
            if (errorCollector == null)
            {
                throw new ArgumentException("ExportService dependencies cannot be NULL.");
            }

            this._errorCollector = errorCollector;
        }

        public IExportOutputModel Export<T>(IExportInputModel<T> inputModel)
        {
            if (inputModel == null || !inputModel.IsValid())
            {
                _errorCollector.Errors.Add(BusinessErrors.InvalidInput);
                return null;
            }

            DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(inputModel.SourceData), (typeof(DataTable)));
            MemoryStream memoryStream = new MemoryStream();
            
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = inputModel.EntityName };

                sheets.Append(sheet);

                Row headerRow = new Row();

                List<string> columns = new List<string>();
                foreach (DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName);

                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(column.ColumnName);
                    headerRow.AppendChild(cell);
                }

                sheetData.AppendChild(headerRow);

                foreach (DataRow dsrow in table.Rows)
                {
                    Row newRow = new Row();
                    foreach (string col in columns)
                    {
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(dsrow[col].ToString());
                        newRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(newRow);
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
            }

            var file = memoryStream.ToArray();
            memoryStream.Dispose();

            //"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            return new ExportOutputModel
            {
                File = file
            };
        }
    }
}
