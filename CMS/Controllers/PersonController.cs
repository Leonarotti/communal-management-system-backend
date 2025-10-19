using CommunalManagementSystem.API.DTOs;
using CommunalManagementSystem.API.Mappers;
using CommunalManagementSystem.BusinessWorkflow.Interfaces.BW;
using Microsoft.AspNetCore.Mvc;

using QuestPDF.Fluent;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;


namespace CommunalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IManagePersonBW _managePersonBW;



        public PersonController(IManagePersonBW managePersonBW)
        {
            _managePersonBW = managePersonBW;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var persons = await _managePersonBW.GetAllAsync();
            var orderedPersons = persons.OrderBy(p => p.Name).ToList(); // Orden A-Z
            var personDTOs = PersonMapper.PersonsToPersonDTOs(orderedPersons);
            return Ok(personDTOs);
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var person = await _managePersonBW.GetByIdAsync(id);
            return person is not null
                ? Ok(PersonMapper.PersonToPersonDTO(person))
                : NotFound();
        }

        [HttpGet("dni/{dni}")]
        public async Task<IActionResult> GetByDni(string dni)
        {
            var person = await _managePersonBW.GetByDniAsync(dni);
            return person is not null
                ? Ok(PersonMapper.PersonToPersonDTO(person))
                : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePersonDTO createPersonDTO)
        {
            var person = PersonMapper.CreatePersonDTOToPerson(createPersonDTO);
            var id = await _managePersonBW.CreateAsync(person);

            // Retornar solo el DTO generado
            var createdPerson = await _managePersonBW.GetByIdAsync(id);
            if (createdPerson == null) return NotFound();

            var dto = PersonMapper.PersonToPersonDTO(createdPerson);
            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreatePersonDTO createPersonDTO)
        {
            var person = PersonMapper.CreatePersonDTOToPerson(createPersonDTO);
            var updated = await _managePersonBW.UpdateAsync(id, person);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _managePersonBW.DeleteAsync(id);
                return deleted ? NoContent() : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error eliminando la persona.", detail = ex.Message });
            }
        }

        [HttpGet("total")]
        public async Task<IActionResult> GetTotalPersons()
        {
            var total = await _managePersonBW.GetTotalPersonsAsync();
            return Ok(total);
        }


/// <summary>
/// ///////////////
/// </summary>
/// <returns></returns>


[HttpGet("export/pdf")]
    public async Task<IActionResult> ExportToPdf()
    {
        var persons = await _managePersonBW.GetAllAsync();
        var orderedPersons = persons.OrderBy(p => p.Name).ToList();

        var stream = new MemoryStream();

        var document = QuestPDF.Fluent.Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Header().Text("Listado de Personas").FontSize(20).Bold().AlignCenter();

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Nombre").Bold();
                        header.Cell().Text("Cedula").Bold();
                        header.Cell().Text("Teléfono").Bold();
                    });

                    foreach (var p in orderedPersons)
                    {
                        table.Cell().Text(p.Name);
                        table.Cell().Text(p.Dni);
                        table.Cell().Text(p.Phone ?? "-");
                    }
                });

                page.Footer().AlignCenter().Text(txt =>
                {
                    txt.Span("Generado el ").FontSize(10);
                    txt.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm")).FontSize(10);
                });
            });
        });

        document.GeneratePdf(stream);
        stream.Position = 0;

        return File(stream, "application/pdf", "Personas.pdf");
    }


        [HttpGet("export/word")]
        public async Task<IActionResult> ExportToWordAsync()
        {
            // 1️⃣ Obtener y ordenar las personas A-Z
            var persons = await _managePersonBW.GetAllAsync();
            var orderedPersons = persons.OrderBy(p => p.Name).ToList();

            // 2️⃣ Crear documento Word en memoria
            using (var memoryStream = new MemoryStream())
            {
                using (var wordDocument = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document, true))
                {
                    var mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
                    var body = new Body();

                    // === TÍTULO ===
                    var titleParagraph = new Paragraph(new Run(new Text("Listado de Personas")));
                    titleParagraph.ParagraphProperties = new ParagraphProperties(
                        new Justification() { Val = JustificationValues.Center });
                    titleParagraph.Descendants<Run>().First().RunProperties = new RunProperties(
                        new Bold(), new FontSize() { Val = "28" });
                    body.Append(titleParagraph);
                    body.Append(new Paragraph(new Run(new Text("")))); // Espacio

                    // === CREAR TABLA ===
                    var table = new Table();

                    // Bordes de tabla
                    TableProperties tblProps = new TableProperties(
                        new TableBorders(
                            new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                            new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                            new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                            new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                            new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                            new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 }
                        )
                    );
                    table.AppendChild(tblProps);

                    // === Fila de encabezado ===
                    var headerRow = new TableRow();

                    string[] headers = { "Nombre", "Cédula", "Teléfono" };
                    foreach (var h in headers)
                    {
                        var cell = new TableCell(
                            new Paragraph(new Run(new Text(h)))
                        );
                        cell.TableCellProperties = new TableCellProperties(
                            new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "3000" });
                        // Negrita
                        cell.Descendants<Run>().First().RunProperties = new RunProperties(new Bold());
                        headerRow.Append(cell);
                    }
                    table.Append(headerRow);

                    // === Filas con datos ===
                    foreach (var p in orderedPersons)
                    {
                        var row = new TableRow();

                        row.Append(
                            new TableCell(new Paragraph(new Run(new Text(p.Name)))),
                            new TableCell(new Paragraph(new Run(new Text(p.Dni)))),
                            new TableCell(new Paragraph(new Run(new Text(p.Phone ?? "-"))))
                        );

                        table.Append(row);
                    }

                    body.Append(table);

                    // === PIE DE PÁGINA ===
                    body.Append(new Paragraph(new Run(new Text(""))));
                    var footer = new Paragraph(new Run(new Text($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}")));
                    footer.ParagraphProperties = new ParagraphProperties(new Justification() { Val = JustificationValues.Center });
                    body.Append(footer);

                    // Guardar documento
                    mainPart.Document.Append(body);
                    mainPart.Document.Save();
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
                return File(memoryStream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "Personas.docx");
            }
        }



    }

}