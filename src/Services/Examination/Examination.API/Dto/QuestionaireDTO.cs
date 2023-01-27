using System.Collections.Generic;

namespace Examination.API.Dto
{
    public class QuestionaireDTO
    {
        public string question { get; set; }
        public List<string> answers { get; set; }
        public string rightans { get; set; }
    }
}
