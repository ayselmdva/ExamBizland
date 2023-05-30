using System.ComponentModel.DataAnnotations;

namespace ExamBizland.View_Models
{
    public class LoginVM
    {
        public int Id { get; set; }
        public string UserNameOrEmail { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
