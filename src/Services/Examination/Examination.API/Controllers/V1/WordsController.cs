using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Examination.API.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Examination.API.Controllers.V1
{
    public class WordsController : BaseController
    {
        [HttpGet("ReadToContent/{path}")]
        public async Task<List<string>> GetWordContent(string path)
        {
            List<string> result = new List<string>();
            try
            {
                using (WordprocessingDocument word = WordprocessingDocument.Open(path, false))
                {
                    Body body = word.MainDocumentPart.Document.Body;
                    foreach (var item in body.ChildElements)
                    {
                        result.Add(item.InnerText);
                    }
                }
            }
            catch (Exception ex)
            {
                return new List<string> { ex.Message };
            }
            return result;
        }
        [HttpGet("ReadToObject/{path}")]
        public async Task<ActionResult<List<QuestionaireDTO>>> GetToObject(string path)
        {
            List<QuestionaireDTO> result = new List<QuestionaireDTO>();
            //gọi tới nó rồi đọc thôi
            try
            {
                using (WordprocessingDocument word = WordprocessingDocument.Open(path, false))
                {
                    Body body = word.MainDocumentPart.Document.Body;
                    QuestionaireDTO questionaireDTO = new QuestionaireDTO();
                    foreach (var item in body.ChildElements)
                    {
                        if (item.InnerText.StartsWith("Cau "))
                        {
                            questionaireDTO = new QuestionaireDTO
                            {
                                question = item.InnerText,
                                answers = new List<string>(),
                                rightans = ""
                            };
                            result.Add(questionaireDTO);
                        }
                        else if (item.InnerText.StartsWith("_"))
                        {
                            result[Array.IndexOf(result.ToArray(), questionaireDTO)].rightans = item.InnerText.Substring(1);
                        }
                        else
                        {
                            result[Array.IndexOf(result.ToArray(), questionaireDTO)].answers.Add(item.InnerText);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Conflict(new List<QuestionaireDTO>());
            }
            return Ok(result);
        }
    }
}
