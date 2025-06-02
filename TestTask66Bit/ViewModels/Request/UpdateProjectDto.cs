namespace TestTask66Bit.ViewModels.Request
{
    public class UpdateProjectDto
    {
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Добавляемые стажеры
        /// </summary>
        public int[] Interns { get; set; } = [];
    }
}
